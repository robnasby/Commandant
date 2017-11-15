using System.Collections.Generic;

namespace CommandantTests.HelperCommands
{
    class TestAppWithArgumentsCommand : AbstractTestAppWithArgumentsCommand
    {
        #region Constructors

        public TestAppWithArgumentsCommand(IEnumerable<object> arguments)
        {
            foreach (object argument in arguments)
                this.Arguments.Append(argument);
        }

        #endregion
    }
}
