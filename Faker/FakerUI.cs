using DevToys.Api;
using System.ComponentModel.Composition;
using static DevToys.Api.GUI;

namespace FakerExtension;

[Export(typeof(IGuiTool))]
[Name("FakerExtension")]                                                         // A unique, internal name of the tool.
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",                                       // This font is available by default in DevToys
    IconGlyph = '\uE670',                                                     // An icon that represents a pizza
    GroupName = PredefinedCommonToolGroupNames.Converters,                    // The group in which the tool will appear in the side bar.
    ResourceManagerAssemblyIdentifier = nameof(FakerExtensionResourceAssemblyIdentifier), // The Resource Assembly Identifier to use
    ResourceManagerBaseName = "Faker.Faker",                      // The full name (including namespace) of the resource file containing our localized texts
    ShortDisplayTitleResourceName = nameof(FakerExtension.ShortDisplayTitle),    // The name of the resource to use for the short display title
    LongDisplayTitleResourceName = nameof(FakerExtension.LongDisplayTitle),
    DescriptionResourceName = nameof(FakerExtension.Description),
    AccessibleNameResourceName = nameof(FakerExtension.AccessibleName))]
internal sealed class FakerExtensionGui : IGuiTool
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
    public FakerExtensionGui()
    {
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

                                    Label().Text(FakerExtension.FakerLabel)))),

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

    private static readonly string[] FirstNames = { "Alice", "Bob", "Charlie", "Dana", "Eve" };
    private static readonly string[] LastNames = { "Smith", "Johnson", "Brown", "Garcia", "Lee" };
    private readonly Random _rng = new();

    private string GenerateRandomName()
    {
        var first = FirstNames[_rng.Next(FirstNames.Length)];
        var last = LastNames[_rng.Next(LastNames.Length)];
        return $"{first} {last}";
    }

    private System.Threading.Tasks.ValueTask OnGenerateButtonClick()
    {
        var name = GenerateRandomName();
        _outputText.Text(name);
        return default;
    }

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }
}
