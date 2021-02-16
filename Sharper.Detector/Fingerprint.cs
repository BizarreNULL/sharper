using System;
using System.Reflection;

namespace Sharper.Detector
{
    /// <summary>
    /// Show functionalities to check if given file is a .NET binary.
    /// </summary>
    public static class Fingerprint
    {
        /// <summary>
        /// Check if given file have correct image signature for a .NET binary.
        /// </summary>
        /// <param name="fullPath">Complete path to file to be tested.</param>
        /// <returns>If the binary is a valid .NET one.</returns>
        public static bool IsDotNetBinary(string fullPath)
        {
            try
            {
                // Checks more deeper than just unmanaged binaries magic bytes 4d 5a 90 00 03.  
                _ = AssemblyName.GetAssemblyName(fullPath ?? throw new ArgumentNullException(nameof(fullPath)));
                return true;
            }
            catch (BadImageFormatException)
            {
                return false;
            }
        }
    }
}
