using Commandant;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandantTests.HelperCommands
{
    class TestAppWithPreAndPostExecuteCommand : Command<String>
    {
        #region Constants

        private static readonly Regex fileContentRegex = new Regex("^FILE CONTENT: '(.+?)'$");

        #endregion

        #region Properties

        private String FileContents { get; set; }

        internal String FilePath { get; private set; }

        #endregion

        #region Constructors

        public TestAppWithPreAndPostExecuteCommand(String fileContents)
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.FileContents = fileContents;

            this.FilePath = Path.GetTempFileName();
            this.Arguments.Append("FILE");
            this.Arguments.Append(this.FilePath);
        }

        #endregion

        #region Methods

        protected override string BuildResult()
        {
            return this.OutputLines
                .Where((l) => { return fileContentRegex.IsMatch(l); })
                .Select((l) => { return fileContentRegex.Match(l).Groups[1].Value; })
                .FirstOrDefault();
        }

        protected override void PostExecute()
        {
            File.Delete(this.FilePath);
        }


        protected override void PreExecute()
        {
            File.WriteAllText(this.FilePath, this.FileContents);
        }

        #endregion
    }
}
