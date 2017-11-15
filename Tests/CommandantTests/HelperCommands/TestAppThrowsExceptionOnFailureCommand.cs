using Commandant;

namespace CommandantTests.HelperCommands
{
    class TestAppThrowsExceptionOnFailureCommand : Command
    {
        #region Constructors

        public TestAppThrowsExceptionOnFailureCommand()
            : base(@"..\..\..\TestApp\bin\TestApp.exe")
        {
            this.Arguments.Append("EXITCODE");
            this.Arguments.Append("33");

            this.ThrowExceptionOnFailure = true;
        }

        #endregion
    }
}
