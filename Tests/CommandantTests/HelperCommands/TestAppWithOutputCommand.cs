using Commandant;
using System;
using System.Collections.Generic;

namespace CommandantTests.HelperCommands
{
    class TestAppWithOutputCommand : Command<CommandOutput>
    {
        #region Constructors

        public TestAppWithOutputCommand(IEnumerable<String> outputLines)
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.Arguments.Append("OUTPUT");
            foreach (String outputLine in outputLines)
                this.Arguments.Append(outputLine);
        }

        #endregion

        #region Methods

        protected override CommandOutput BuildResult()
        {
            return new CommandOutput(this.OutputLines,
                                     this.StandardErrorLines,
                                     this.StandardOutputLines,
                                     this.OutputText,
                                     this.StandardErrorText,
                                     this.StandardOutputText);
        }

        #endregion
    }
}
