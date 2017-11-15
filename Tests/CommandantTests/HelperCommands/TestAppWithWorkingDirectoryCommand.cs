using Commandant;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandantTests.HelperCommands
{
    class TestAppWithWorkingDirectoryCommand : Command<string>
    {
        #region Constants

        private static readonly Regex workingDirectoryRegex = new Regex("^WORKING DIRECTORY: '(.+?)'$");

        #endregion

        #region Constructors

        public TestAppWithWorkingDirectoryCommand(string workingDirectoryPath)
            : base(@".\TestApp.exe")
        {
            this.Arguments.Append("WORKDIR");

            this.WorkingDirectory = workingDirectoryPath;
        }

        #endregion

        #region "Methods"

        protected override string BuildResult()
        {
            return this.OutputLines
                .Where((l) => { return workingDirectoryRegex.IsMatch(l); })
                .Select((l) => { return workingDirectoryRegex.Match(l).Groups[1].Value; })
                .FirstOrDefault();
        }

        #endregion
    }
}
