using System;
using System.Collections.Generic;

namespace CommandantTests.HelperCommands
{
    class CommandEnvironmentVariables : List<KeyValuePair<String, String>>
    {
        #region Constructors

        public CommandEnvironmentVariables(IEnumerable<KeyValuePair<String, String>> environmentVariables)
            : base(environmentVariables)
        { }

        #endregion
    }
}
