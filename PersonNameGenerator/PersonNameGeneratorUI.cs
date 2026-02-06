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
                                            Item(PersonNameGeneratorResources.GenderFemale, Gender.Female))))),

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
        
        var bogusGender = selectedGender switch
        {
            Gender.Male => Bogus.DataSets.Name.Gender.Male,
            Gender.Female => Bogus.DataSets.Name.Gender.Female,
            _ => _faker.Random.Bool() ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female
        };

        var firstName = _faker.Name.FirstName(bogusGender);
        var lastName = _faker.Name.LastName();

        return $"{firstName} {lastName}";
    }

    private void OnGenerateButtonClick()
    {
        var name = GenerateRandomName();
        _outputText.Text(name);
    }

    private void OnGenderChanged(Gender value)
    {
        OnGenerateButtonClick();
    }

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }
}
