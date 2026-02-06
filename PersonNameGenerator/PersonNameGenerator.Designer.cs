namespace PersonNameGenerator;
using System;

internal class PersonNameGeneratorResources {

    private static global::System.Resources.ResourceManager resourceMan;

    private static global::System.Globalization.CultureInfo resourceCulture;

    [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal PersonNameGeneratorResources() {
    }

    /// <summary>
    ///   Returns the cached ResourceManager instance used by this class.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Resources.ResourceManager ResourceManager {
        get {
                if (object.ReferenceEquals(resourceMan, null)) {
                global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PersonNameGenerator.PersonNameGenerator", typeof(PersonNameGeneratorResources).Assembly);
                resourceMan = temp;
            }
            return resourceMan;
        }
    }

    /// <summary>
    ///   Overrides the current thread's CurrentUICulture property for all
    ///   resource lookups using this strongly typed resource class.
    /// </summary>
    [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
    internal static global::System.Globalization.CultureInfo Culture {
        get {
            return resourceCulture;
        }
        set {
            resourceCulture = value;
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to A &quot;Hello World&quot; extension for DevToys.
    /// </summary>
    internal static string AccessibleName {
        get {
            return ResourceManager.GetString("AccessibleName", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to A sample extension.
    /// </summary>
    internal static string Description {
        get {
            return ResourceManager.GetString("Description", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Generate a random name.
    /// </summary>
    internal static string PersonNameGeneratorLabel {
        get {
            return ResourceManager.GetString("PersonNameGeneratorLabel", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Configuration.
    /// </summary>
    internal static string ConfigurationTitle {
        get {
            return ResourceManager.GetString("ConfigurationTitle", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Gender.
    /// </summary>
    internal static string GenderTitle {
        get {
            return ResourceManager.GetString("GenderTitle", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Select the gender for the generated name.
    /// </summary>
    internal static string GenderDescription {
        get {
            return ResourceManager.GetString("GenderDescription", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Random.
    /// </summary>
    internal static string GenderRandom {
        get {
            return ResourceManager.GetString("GenderRandom", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Male.
    /// </summary>
    internal static string GenderMale {
        get {
            return ResourceManager.GetString("GenderMale", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Female.
    /// </summary>
    internal static string GenderFemale {
        get {
            return ResourceManager.GetString("GenderFemale", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Refresh.
    /// </summary>
    internal static string Refresh {
        get {
            return ResourceManager.GetString("Refresh", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to My Awesome Extension.
    /// </summary>
    internal static string LongDisplayTitle {
        get {
            return ResourceManager.GetString("LongDisplayTitle", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to My Extension.
    /// </summary>
    internal static string ShortDisplayTitle {
        get {
            return ResourceManager.GetString("ShortDisplayTitle", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to Number of Names.
    /// </summary>
    internal static string NumberOfNamesTitle {
        get {
            return ResourceManager.GetString("NumberOfNamesTitle", resourceCulture);
        }
    }

    /// <summary>
    ///   Looks up a localized string similar to How many names to generate.
    /// </summary>
    internal static string NumberOfNamesDescription {
        get {
            return ResourceManager.GetString("NumberOfNamesDescription", resourceCulture);
        }
    }
}