using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ColorCoded
{
    public static class Program
    {
        // Application related folders.
        private static readonly string StartupPath = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string FormatFolder = Program.StartupPath + "dll\\";

        // The main point of entry for the application.
        private static void Main(string[] args) {

            // Make sure the dll folder exists.
            if (!Directory.Exists(Program.FormatFolder)) {
                Console.WriteLine("Created dll/ folder.");
                Directory.CreateDirectory(Program.FormatFolder);
            }
            
            // Loop through every dll present in the folder.
            foreach (string dllFile in Directory.GetFiles(Program.FormatFolder, "*.dll")) {

                // Load the DLL, and get all the types that inherit from the LanguageFormat class.
                var dll = Assembly.LoadFile(dllFile);
                var types = dll.GetTypes().Where(type => type.BaseType?.Name == "LanguageFormat");

                // Run each formatter.
                foreach (var type in types) {
                    dynamic format = Activator.CreateInstance(type);
                    format.FormatFiles();
                }
            }

            // Denote that the application has finished.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
