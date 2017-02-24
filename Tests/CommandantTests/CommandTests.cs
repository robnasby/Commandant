using CommandantTests.HelperCommands;
using System;
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
    }
}
