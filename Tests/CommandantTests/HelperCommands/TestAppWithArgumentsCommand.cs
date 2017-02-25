using Commandant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandantTests.HelperCommands
{
    class TestAppWithArgumentsCommand : Command<CommandArguments>
    {
        #region Constants

        private static readonly Regex ArgumentRegex = new Regex("^ARG: '(.+?)'$");

        #endregion

        #region Constructors

        public TestAppWithArgumentsCommand(IEnumerable<Object> arguments)
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.Arguments.Append("ARGS");
            foreach (Object argument in arguments)
                this.Arguments.Append(argument);
        }

        #endregion

        #region Methods

        protected override CommandArguments BuildResult()
        {
            IEnumerable<String> arguments = this.CombinedOutputLines
                .Where((l) => { return ArgumentRegex.IsMatch(l); })
                .Select((l) => { return ArgumentRegex.Match(l).Groups[1].Value; })
                .ToList();

            return new CommandArguments(arguments);
        }

        #endregion
    }
}
