using System;
using System.Collections.Generic;

namespace CommandantTests.HelperCommands
{
    class CommandOutput
    {
        #region Properties

        public IEnumerable<String> OutputLines { get; private set; }

        public String OutputText { get; private set; }

        public IEnumerable<String> StandardErrorLines { get; private set; }

        public String StandardErrorText { get; private set; }

        public IEnumerable<String> StandardOutputLines { get; private set; }

        public String StandardOutputText { get; private set; }

        #endregion

        #region Constructors

        public CommandOutput(IEnumerable<String> outputLines,
                             IEnumerable<String> standardErrorLines,
                             IEnumerable<String> standardOutputLines,
                             String outputText,
                             String standardErrorText,
                             String standardOutputText)
        {
            this.OutputLines = outputLines;
            this.OutputText = outputText;
            this.StandardErrorLines = standardErrorLines;
            this.StandardErrorText = standardErrorText;
            this.StandardOutputLines = standardOutputLines;
            this.StandardOutputText = standardOutputText;
        }

        #endregion
    }
}
