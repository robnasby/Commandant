using Commandant;
using System;

namespace CommandantTests.HelperCommands
{
    class TestAppSucceededCommand : Command
    {
        #region Constructors

        public TestAppSucceededCommand()
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.Arguments.Append("SUCCEED");
        }

        #endregion
    }
}
