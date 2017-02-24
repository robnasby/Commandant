using Commandant;
using System;

namespace CommandantTests.HelperCommands
{
    class TestAppCommand : Command
    {
        #region Constructors

        public TestAppCommand()
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        { }

        #endregion
    }
}
