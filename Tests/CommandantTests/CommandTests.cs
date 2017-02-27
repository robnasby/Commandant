using Commandant;
using CommandantTests.HelperCommands;
using System;
using System.Collections.Generic;
using System.IO;
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
        private void ExecuteCommandFails()
        {
            Assert.Equal<CommandStatus>(CommandStatus.FAILED, new TestAppFailedCommand().Execute().Status);
        }

        [Fact]
        private void ExecuteCommandSucceeds()
        {
            Assert.Equal<CommandStatus>(CommandStatus.SUCCEEDED, new TestAppSucceededCommand().Execute().Status);
        }

        [Fact]
        private void ExecuteCommandWithArguments()
        {
            var arguments = new List<Object> { "foo", 2 };
            TestAppWithArgumentsCommand command = new TestAppWithArgumentsCommand(arguments).Execute();

            foreach (Object argument in arguments)
                Assert.Contains<String>(argument.ToString(), command.Result);
        }

        [Fact]
        private void ExecuteCommandWithEnvironmentVariables()
        {
            var environmentVariables = new Dictionary<String, String> { {"ENV1", "foo"}, {"ENV2", "2"} };
            TestAppWithEnvironmentVariablesCommand command = new TestAppWithEnvironmentVariablesCommand(environmentVariables).Execute();

            foreach (KeyValuePair<String, String> environmentVariable in environmentVariables)
                Assert.Contains<KeyValuePair<String, String>>(environmentVariable, command.Result);
        }

        [Fact]
        private void ExecuteCommandWithPreAndPostExecute()
        {
            String fileContents = "TEST FILE CONTENTS";
            TestAppWithPreAndPostExecuteCommand command = new TestAppWithPreAndPostExecuteCommand(fileContents).Execute();

            Assert.Equal(fileContents, command.Result);
            Assert.False(File.Exists(command.FilePath));
        }
    }
}
