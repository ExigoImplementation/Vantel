using Common;
using System;

namespace Backoffice
{
    /// <summary>
    /// Site-specific settings
    /// </summary>
    public static partial class Settings
    {
        public static bool UseGateKeeper = false;

        public static bool GeneratePasswordForExistingHostess = false;

        // To assist with cache busting on new deployments
        public static int StyleVersionNumber = DateTime.Now.Millisecond;
    }
}