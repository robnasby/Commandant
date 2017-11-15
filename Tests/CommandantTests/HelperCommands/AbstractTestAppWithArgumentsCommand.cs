using Commandant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandantTests.HelperCommands
{
    abstract class AbstractTestAppWithArgumentsCommand : Command<CommandArguments>
    {
        #region Constants

        private static readonly Regex ArgumentRegex = new Regex("^ARG: '(.+?)'$");

        #endregion

        #region Constructors

        public AbstractTestAppWithArgumentsCommand()
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.Arguments.Append("ARGS");
        }

        #endregion

        #region Methods

        protected override CommandArguments BuildResult()
        {
            IEnumerable<String> arguments = this.OutputLines
                .Where((l) => { return ArgumentRegex.IsMatch(l); })
                .Select((l) => { return ArgumentRegex.Match(l).Groups[1].Value; })
                .ToList();

            return new CommandArguments(arguments);
        }

        #endregion
    }
}
