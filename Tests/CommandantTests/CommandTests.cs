using CommandantTests.HelperCommands;
using System;
using System.Collections.Generic;
using Xunit;

namespace CommandantTests
{
    public class CommandTests
    {
        [Fact]
        private void ExecuteCommand()
        {
            new TestAppCommand().Execute();
        }

        [Fact]
        private void ExecuteCommandWithArguments()
        {
            var arguments = new List<Object> { "foo", 2 };
            new TestAppWithArgumentsCommand(arguments).Execute();
        }
    }
}
