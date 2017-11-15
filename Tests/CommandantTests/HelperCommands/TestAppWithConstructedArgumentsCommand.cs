using Commandant;

namespace CommandantTests.HelperCommands
{
    class TestAppWithConstructedArgumentsCommand : AbstractTestAppWithArgumentsCommand
    {
        #region Constructors

        public TestAppWithConstructedArgumentsCommand(Arguments arguments)
        {
            this.Arguments.Append(arguments);
        }

        #endregion
    }
}
