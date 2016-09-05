using System;
using System.IO;

namespace LanguageFormatRequirements
{
    public abstract class LanguageFormat
    {
        // The name of the language, and the paths used with the application.
        private string _languageName;
        protected readonly string SourcePath;
        protected readonly string OutputPath;

        // The abstract method used to format all related code files, uniquely implemented in each language.
        public abstract void FormatFiles();

        public LanguageFormat(string UniqueName) {
            // Save the name of the language.
            this._languageName = UniqueName;

            // Use the language name to make the input and output folders.
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            this.SourcePath = startupPath + "source\\" + this._languageName + "\\";
            this.OutputPath = startupPath + "output\\" + this._languageName + "\\";

            // Check to see if those folders exist.
            this.CheckFolderSystem();
        }

        private void CheckFolderSystem() {
            // Check if the source path folder exists. If not, create it.
            if (!Directory.Exists(this.SourcePath)) {
                Directory.CreateDirectory(this.SourcePath);
            }

            // Check if the output path folder exists. If not, create it.
            if (!Directory.Exists(this.OutputPath)) {
                Directory.CreateDirectory(this.OutputPath);
            }
        }
    }
}