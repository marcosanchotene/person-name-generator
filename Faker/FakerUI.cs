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
    // public UIToolView View => new(Label().Style(UILabelStyle.BodyStrong).Text(FakerExtension.FakerLabel));

    public UIToolView View
    {
        get
        {
            var root = DevToys.Api.GUI.Stack();
            DevToys.Api.GUI.Vertical(root);
            DevToys.Api.GUI.MediumSpacing(root);

            var title = DevToys.Api.GUI.Label();
            DevToys.Api.GUI.Text(title, FakerExtension.FakerLabel);
            DevToys.Api.GUI.Style(title, UILabelStyle.BodyStrong);

            var buttons = DevToys.Api.GUI.Stack();
            DevToys.Api.GUI.Horizontal(buttons);
            DevToys.Api.GUI.SmallSpacing(buttons);

            var generate = DevToys.Api.GUI.Button();
            DevToys.Api.GUI.Text(generate, "Generate");

            var copy = DevToys.Api.GUI.Button();
            DevToys.Api.GUI.Text(copy, "Copy");

            var output = DevToys.Api.GUI.Label();
            DevToys.Api.GUI.Text(output, "Generated name will appear here");
            DevToys.Api.GUI.Style(output, UILabelStyle.Body);

            DevToys.Api.GUI.OnClick(generate, () =>
            {
                var name = GenerateRandomName();
                _lastGeneratedName = name;
                DevToys.Api.GUI.Text(output, name);
                return new System.Threading.Tasks.ValueTask();
            });

            DevToys.Api.GUI.OnClick(copy, () =>
            {
                // Clipboard integration can be added later via IClipboard service.
                // For now just re-display the last generated name.
                DevToys.Api.GUI.Text(output, _lastGeneratedName ?? string.Empty);
                return new System.Threading.Tasks.ValueTask();
            });

            DevToys.Api.GUI.WithChildren(buttons, generate, copy);
            DevToys.Api.GUI.WithChildren(root, title, buttons, output);

            return new UIToolView(root);
        }
    }

    private static readonly string[] FirstNames = { "Alice", "Bob", "Charlie", "Dana", "Eve" };
    private static readonly string[] LastNames = { "Smith", "Johnson", "Brown", "Garcia", "Lee" };
    private readonly Random _rng = new();
    private string? _lastGeneratedName;

    private string GenerateRandomName()
    {
        var first = FirstNames[_rng.Next(FirstNames.Length)];
        var last = LastNames[_rng.Next(LastNames.Length)];
        return $"{first} {last}";
    }

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }
}
