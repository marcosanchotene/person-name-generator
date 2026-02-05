using DevToys.Api;
using System.ComponentModel.Composition;

namespace FakerExtension;

[Export(typeof(IResourceAssemblyIdentifier))]
[Name(nameof(FakerExtensionResourceAssemblyIdentifier))]
internal sealed class FakerExtensionResourceAssemblyIdentifier : IResourceAssemblyIdentifier
{
    public ValueTask<FontDefinition[]> GetFontDefinitionsAsync()
    {
        return new ValueTask<FontDefinition[]>(Array.Empty<FontDefinition>());
    }
}