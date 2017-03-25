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
        private void ExecuteCommandWithExpectedExitCodes()
        {
            Assert.Equal<CommandStatus>(CommandStatus.SUCCEEDED, new TestAppWithExpectedExitCodesCommand().Execute().Status);
        }

        [Fact]
        private void ExecuteCommandWithPreAndPostExecute()
        {
            String fileContents = "TEST FILE CONTENTS";
            TestAppWithPreAndPostExecuteCommand command = new TestAppWithPreAndPostExecuteCommand(fileContents).Execute();

            Assert.Equal(fileContents, command.Result);
            Assert.False(File.Exists(command.FilePath));
        }

        [Fact]
        private void OutputLinesMatchOutputText()
        {
            IEnumerable<String> outputLines = new String[] {
                "OUT:Hello.",
                "ERR:Hi.",
                "ERR:How are you?",
                "OUT:I'm doing good.",
                "OUT:How are you?",
                "ERR:Very well.",
                "ERR:Thanks for asking."
            };

            TestAppWithOutputCommand command = new TestAppWithOutputCommand(outputLines).Execute();
            CommandOutput output = command.Result;

            Assert.Equal(String.Join(Environment.NewLine, output.StandardErrorLines), output.StandardErrorText);
            Assert.Equal(String.Join(Environment.NewLine, output.StandardOutputLines), output.StandardOutputText);
            Assert.Equal(String.Join(Environment.NewLine, output.OutputLines), output.OutputText);
        }
    }
}
