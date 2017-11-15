using System;

namespace Commandant
{
    public class CommandFailedException : Exception
    {
        #region Constructors

        internal CommandFailedException(String programName,
                                        String arguments,
                                        int exitCode,
                                        String outputText)
            : base(FormatMessage(programName, arguments, exitCode, outputText))
        {}

        #endregion

        #region Methods

        private static String FormatMessage(String programName,
                                            String arguments,
                                            int exitCode,
                                            String outputText)
        {
            return String.Format("The command failed.\n" +
                                 "\n" +
                                 "Command: {0} {1}\n" +
                                 "Exit code: {2}\n" +
                                 "Output:\n" +
                                 "{3}",
                                 programName,
                                 arguments,
                                 exitCode,
                                 outputText);
        }

        #endregion
    }
}
