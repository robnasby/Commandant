using Commandant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandantTests.HelperCommands
{
    class TestAppWithEnvironmentVariablesCommand : Command<CommandEnvironmentVariables>
    {
        #region Constants

        private static readonly Regex EnvironmentVariableRegex = new Regex("^ENV: '(.+?)' = '(.+?)'$");

        #endregion

        #region Constructors

        public TestAppWithEnvironmentVariablesCommand(Dictionary<String, String> environmentVariables)
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.Arguments.Append("ENVS");
            foreach (KeyValuePair<String, String> environmentVariable in environmentVariables)
                this.EnvironmentVariables.Add(environmentVariable.Key, environmentVariable.Value);
        }

        #endregion

        #region Methods

        protected override CommandEnvironmentVariables BuildResult()
        {
            IEnumerable<KeyValuePair<String, String>> environmentVariables = this.OutputLines
                .Where((l) => { return EnvironmentVariableRegex.IsMatch(l); })
                .Select((l) => { 
                    Match match = EnvironmentVariableRegex.Match(l);
                    return new KeyValuePair<String, String>(match.Groups[1].Value, match.Groups[2].Value);
                })
                .ToList();

            return new CommandEnvironmentVariables(environmentVariables);
        }

        #endregion
    }
}
