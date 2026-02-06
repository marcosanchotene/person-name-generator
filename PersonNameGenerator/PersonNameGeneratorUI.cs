using DevToys.Api;
using System.ComponentModel.Composition;
using static DevToys.Api.GUI;
using Bogus;

namespace PersonNameGenerator;

[Export(typeof(IGuiTool))]
[Name("PersonNameGenerator")]                                                         // A unique, internal name of the tool.
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",                                       // This font is available by default in DevToys
    IconGlyph = '\uF5BE',                                                     // Person/contact icon (represents names)
    GroupName = PredefinedCommonToolGroupNames.Converters,                    // The group in which the tool will appear in the side bar.
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

    private readonly IUIMultiLineTextInput _outputText = MultiLineTextInput();

    [System.ComponentModel.Composition.ImportingConstructor]
    public PersonNameGeneratorGui()
    {
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
                            Label().Text(PersonNameGeneratorResources.FakerLabel))),

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
                                .Text("Refresh")
                                .OnClick(OnGenerateButtonClick)))));

    private readonly Faker _faker = new();

    private string GenerateRandomName()
    {
        return _faker.Name.FullName();
    }

    private void OnGenerateButtonClick()
    {
        var name = GenerateRandomName();
        _outputText.Text(name);
    }

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }
}
