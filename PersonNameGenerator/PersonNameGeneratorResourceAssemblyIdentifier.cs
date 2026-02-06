using DevToys.Api;
using System.ComponentModel.Composition;

namespace PersonNameGenerator;

[Export(typeof(IResourceAssemblyIdentifier))]
[Name(nameof(PersonNameGeneratorResourceAssemblyIdentifier))]
internal sealed class PersonNameGeneratorResourceAssemblyIdentifier : IResourceAssemblyIdentifier
{
    public ValueTask<FontDefinition[]> GetFontDefinitionsAsync()
    {
        return new ValueTask<FontDefinition[]>(Array.Empty<FontDefinition>());
    }
}