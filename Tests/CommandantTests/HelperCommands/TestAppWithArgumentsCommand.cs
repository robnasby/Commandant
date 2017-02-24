using Commandant;
using System;
using System.Collections.Generic;

namespace CommandantTests.HelperCommands
{
    class TestAppWithArgumentsCommand : Command
    {
        #region Constructors

        public TestAppWithArgumentsCommand(IEnumerable<Object> arguments)
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            foreach (Object argument in arguments)
                this.Arguments.Append(argument);
        }

        #endregion
    }
}
