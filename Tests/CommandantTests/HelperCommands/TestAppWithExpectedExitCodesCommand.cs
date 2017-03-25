using Commandant;
using System;

namespace CommandantTests.HelperCommands
{
    class TestAppWithExpectedExitCodesCommand : Command
    {
        #region Constructors

        public TestAppWithExpectedExitCodesCommand()
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.Arguments.Append("EXITCODE");
            this.Arguments.Append("33");
            this.ExpectedExitCodes = new int[] { 0, 33 };
        }

        #endregion
    }
}
