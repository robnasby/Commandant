using System;
using System.Collections.Generic;

namespace CommandantTests.HelperCommands
{
    class CommandArguments : List<String>
    {
        #region Constructors

        public CommandArguments(IEnumerable<String> arguments)
            : base(arguments)
        {}

        #endregion
    }
}
