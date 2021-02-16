using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Console;

using Sharper.Detector;

namespace Sharper.Console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            #region Command Line Argument Parsing

            if (!args.Any())
            {
                WriteLine("[!] No arguments are given! Try --help.");
                Environment.Exit(1);
            }

            if (args.Any(a =>
                a.ToLowerInvariant() == "--help" ||
                a.ToLowerInvariant() == "-h" ||
                a.ToLowerInvariant() == "-?"))
            {
                WriteLine("Uses Assembly Reflection to check if a binary is a valid .NET one");
                WriteLine("Usage: sharper <path>");
                WriteLine();
                WriteLine("Sharper is a very simple utility to check if a file is a valid .NET binary.");
                WriteLine();
                WriteLine("-h|--help    Show command line help.");
                WriteLine("-t|--types   Exports types for given .NET assembly.");

                Environment.Exit(0);
            }

            #endregion

            var binaryPath = Path.GetFullPath(args.First());

            if (!File.Exists(binaryPath))
            {
                WriteLine($"[!] {binaryPath} point to an nonexistent file!");
                Environment.Exit(1);
            }

            if (!Fingerprint.IsDotNetBinary(binaryPath))
            {
                WriteLine($"[!] {binaryPath} is not a .NET binary!");
                Environment.Exit(1);
            }

            WriteLine($"[+] {binaryPath} is a valid .NET binary");
            if (args.Any(a =>
                a.ToLowerInvariant() == "-t" ||
                a.ToLowerInvariant() == "--types"))
            {
                var assembly = Assembly.LoadFrom(binaryPath);
                var types = assembly.GetTypes().Distinct().ToList();
                
                if (!types.Any())
                {
                    WriteLine("[+] There is not available types");
                    Environment.Exit(0);
                }
                
                WriteLine($"[+] There is available {types.Count} type(s):");
                types.ForEach(t => WriteLine($" + {t.FullName}"));
            }
        }
    }
}
