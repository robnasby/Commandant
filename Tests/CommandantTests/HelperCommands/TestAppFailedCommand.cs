using Commandant;
using System;

namespace CommandantTests.HelperCommands
{
    class TestAppFailedCommand : Command
    {
        #region Constructors

        public TestAppFailedCommand()
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.Arguments.Append("FAIL");
        }

        #endregion
    }
}
