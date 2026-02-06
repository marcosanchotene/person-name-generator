using DevToys.Api;
using System.ComponentModel.Composition;
using static DevToys.Api.GUI;
using Bogus;

namespace PersonNameGenerator;

public enum Gender
{
    Random,
    Male,
    Female
}

public enum Localization
{
    Random,
    EnglishAmerican,
    EnglishBritish,
    EnglishAustralian,
    German,
    French,
    Spanish,
    PortugueseBrazilian,
    Japanese,
    ChineseSimplified
}

[Export(typeof(IGuiTool))]
[Name("PersonNameGenerator")]                                                         // A unique, internal name of the tool.
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",                                       // This font is available by default in DevToys
    IconGlyph = '\uF5BE',                                                     // Person/contact icon (represents names)
    GroupName = PredefinedCommonToolGroupNames.Generators,                    // The group in which the tool will appear in the side bar.
    ResourceManagerAssemblyIdentifier = nameof(PersonNameGeneratorResourceAssemblyIdentifier), // The Resource Assembly Identifier to use
    ResourceManagerBaseName = "PersonNameGenerator.PersonNameGenerator",                      // The full name (including namespace) of the resource file containing our localized texts
    ShortDisplayTitleResourceName = nameof(PersonNameGeneratorResources.ShortDisplayTitle),    // The name of the resource to use for the short display title
    LongDisplayTitleResourceName = nameof(PersonNameGeneratorResources.LongDisplayTitle),
    DescriptionResourceName = nameof(PersonNameGeneratorResources.Description),
    AccessibleNameResourceName = nameof(PersonNameGeneratorResources.AccessibleName))]
internal sealed class PersonNameGeneratorGui : IGuiTool
{
    private enum GridColumn
    {
        Stretch
    }

    private enum GridRow
    {
        Settings,
        Results
    }

    private static readonly SettingDefinition<Gender> gender
        = new(
            name: $"{nameof(PersonNameGeneratorGui)}.{nameof(gender)}",
            defaultValue: Gender.Random);

    private static readonly SettingDefinition<Localization> localization
        = new(
            name: $"{nameof(PersonNameGeneratorGui)}.{nameof(localization)}",
            defaultValue: Localization.EnglishAmerican);

    private static readonly SettingDefinition<int> numberOfNames
        = new(
            name: $"{nameof(PersonNameGeneratorGui)}.{nameof(numberOfNames)}",
            defaultValue: 1);

    private readonly ISettingsProvider _settingsProvider;
    private readonly IUIMultiLineTextInput _outputText = MultiLineTextInput();
    private readonly Faker _faker = new();

    [System.ComponentModel.Composition.ImportingConstructor]
    public PersonNameGeneratorGui(ISettingsProvider settingsProvider)
    {
        _settingsProvider = settingsProvider;
        // Initialize using the same handler as the Refresh button.
        OnGenerateButtonClick();
    }

    public UIToolView View
        => new(
            isScrollable: true,
            Grid()
                .ColumnLargeSpacing()
                .RowLargeSpacing()
                .Rows(
                    (GridRow.Settings, Auto),
                    (GridRow.Results, new UIGridLength(1, UIGridUnitType.Fraction)))
                .Columns(
                    (GridColumn.Stretch, new UIGridLength(1, UIGridUnitType.Fraction)))

            .Cells(
                Cell(
                    GridRow.Settings,
                    GridColumn.Stretch,
                    Stack()
                        .Vertical()
                        .LargeSpacing()
                        .WithChildren(
                            Stack()
                                .Vertical()
                                .WithChildren(
                                    Label().Text(PersonNameGeneratorResources.ConfigurationTitle),
                                    Setting()
                                        .Icon("FluentSystemIcons", '\uEB1F')
                                        .Title(PersonNameGeneratorResources.GenderTitle)
                                        .Description(PersonNameGeneratorResources.GenderDescription)
                                        .Handle(
                                            _settingsProvider,
                                            gender,
                                            OnGenderChanged,
                                            Item(PersonNameGeneratorResources.GenderRandom, Gender.Random),
                                            Item(PersonNameGeneratorResources.GenderMale, Gender.Male),
                                            Item(PersonNameGeneratorResources.GenderFemale, Gender.Female)),
                                    Setting()
                                        .Icon("FluentSystemIcons", '\uF0A7')
                                        .Title(PersonNameGeneratorResources.LocalizationTitle)
                                        .Description(PersonNameGeneratorResources.LocalizationDescription)
                                        .Handle(
                                            _settingsProvider,
                                            localization,
                                            OnLocalizationChanged,
                                            Item(PersonNameGeneratorResources.LocalizationRandom, Localization.Random),
                                            Item(PersonNameGeneratorResources.LocalizationEnglishAmerican, Localization.EnglishAmerican),
                                            Item(PersonNameGeneratorResources.LocalizationEnglishBritish, Localization.EnglishBritish),
                                            Item(PersonNameGeneratorResources.LocalizationEnglishAustralian, Localization.EnglishAustralian),
                                            Item(PersonNameGeneratorResources.LocalizationGerman, Localization.German),
                                            Item(PersonNameGeneratorResources.LocalizationFrench, Localization.French),
                                            Item(PersonNameGeneratorResources.LocalizationSpanish, Localization.Spanish),
                                            Item(PersonNameGeneratorResources.LocalizationPortugueseBrazilian, Localization.PortugueseBrazilian),
                                            Item(PersonNameGeneratorResources.LocalizationJapanese, Localization.Japanese),
                                            Item(PersonNameGeneratorResources.LocalizationChineseSimplified, Localization.ChineseSimplified)),
                                    Setting()
                                        .Icon("FluentSystemIcons", '\uF57D')
                                        .Title(PersonNameGeneratorResources.NumberOfNamesTitle)
                                        .Description(PersonNameGeneratorResources.NumberOfNamesDescription)
                                        .InteractiveElement(
                                            NumberInput()
                                                .HideCommandBar()
                                                .Minimum(1)
                                                .Maximum(1000)
                                                .OnValueChanged(OnNumberOfNamesChanged)
                                                .Value(_settingsProvider.GetSetting(numberOfNames)))))),

                Cell(
                    GridRow.Results,
                    GridColumn.Stretch,
                    _outputText
                        .Title("Output")
                        .ReadOnly()
                        .AlwaysWrap()
                        .CommandBarExtraContent(
                            Button()
                                .Icon("FluentSystemIcons", '\uF13D')
                                .Text(PersonNameGeneratorResources.Refresh)
                                .OnClick(OnGenerateButtonClick)))));

    private string GenerateRandomName()
    {
        var selectedGender = _settingsProvider.GetSetting(gender);
        var selectedLocalization = _settingsProvider.GetSetting(localization);
        
        // Determine the locale string
        var locale = selectedLocalization switch
        {
            Localization.Random => GetRandomLocale(),
            Localization.EnglishAmerican => "en_US",
            Localization.EnglishBritish => "en_GB",
            Localization.EnglishAustralian => "en_AU",
            Localization.German => "de",
            Localization.French => "fr",
            Localization.Spanish => "es",
            Localization.PortugueseBrazilian => "pt_BR",
            Localization.Japanese => "ja",
            Localization.ChineseSimplified => "zh_CN",
            _ => "en_US"
        };

        // Create a new Faker instance with the selected locale
        var faker = new Faker(locale);
        
        var bogusGender = selectedGender switch
        {
            Gender.Male => Bogus.DataSets.Name.Gender.Male,
            Gender.Female => Bogus.DataSets.Name.Gender.Female,
            _ => faker.Random.Bool() ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female
        };

        var firstName = faker.Name.FirstName(bogusGender);
        var lastName = faker.Name.LastName();

        return $"{firstName} {lastName}";
    }

    private string GetRandomLocale()
    {
        var locales = new[] { "en_US", "en_GB", "en_AU", "de", "fr", "es", "pt_BR", "ja", "zh_CN" };
        return locales[_faker.Random.Int(0, locales.Length - 1)];
    }

    private void OnGenerateButtonClick()
    {
        int count = _settingsProvider.GetSetting(numberOfNames);
        GenerateNamesInternal(count);
    }

    private void OnGenderChanged(Gender value)
    {
        OnGenerateButtonClick();
    }

    private void OnLocalizationChanged(Localization value)
    {
        OnGenerateButtonClick();
    }

    private void OnNumberOfNamesChanged(double value)
    {
        int count = (int)value;
        _settingsProvider.SetSetting(numberOfNames, count);
        GenerateNamesInternal(count);
    }

    private void GenerateNamesInternal(int count)
    {
        var names = new List<string>();
        
        for (int i = 0; i < count; i++)
        {
            names.Add(GenerateRandomName());
        }
        
        _outputText.Text(string.Join(Environment.NewLine, names));
    }

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }
}
