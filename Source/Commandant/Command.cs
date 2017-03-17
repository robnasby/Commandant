using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Commandant
{
    /// <summary>
    /// An abstract class representing a program to execute.
    /// </summary>
    public abstract class Command
    {
        #region Properties

        /// <summary>
        /// The arguments to pass to the program.
        /// </summary>
        protected Arguments Arguments{ get { return _Arguments; } }
        private Arguments _Arguments = new Arguments();

        /// <summary>
        /// The combined output of STDOUT and STDERR from executing the command.
        /// </summary>
        protected IEnumerable<String> CombinedOutputLines { get { return GetTextFromOutput(this.OutputLines); } }

        /// <summary>
        /// The environment variables to set before command executuion.
        /// </summary>
        protected Dictionary<String, String> EnvironmentVariables { get { return _EnvironmentVariables; } }
        private Dictionary<String, String> _EnvironmentVariables = new Dictionary<String, String>();

        /// <summary>
        /// The exit code from executing the program.
        /// </summary>
        protected int ExitCode { get; private set; }

        /// <summary>
        /// The output from executing the command.
        /// </summary>
        private List<Output> OutputLines { get { return _OutputLines; } }
        private List<Output> _OutputLines = new List<Output>();

        /// <summary>
        /// The name of (or path to) the program to execute.
        /// </summary>
        private String ProgramNameOrPath { get; set; }

        /// <summary>
        /// The STDERR from executing the command.
        /// </summary>
        protected IEnumerable<String> StandardErrorLines { get { return GetTextFromOutput(FilteredOutputText(OutputType.STDERR)); } }

        /// <summary>
        /// The STDOUT from executing the command.
        /// </summary>
        protected IEnumerable<String> StandardOutputLines { get { return GetTextFromOutput(FilteredOutputText(OutputType.STDOUT)); } }

        /// <summary>
        /// The status of executing the command.
        /// </summary>
        public CommandStatus Status { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an instance of <see cref="Command"/>.
        /// </summary>
        /// <param name="programNameOrPath">
        /// The name of (or path to) the program to execute.
        /// </param>
        public Command(String programNameOrPath)
        {
            this.ProgramNameOrPath = programNameOrPath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determine the program and arguments to use when executing the command.
        /// </summary>
        /// <returns>
        /// The program and arguments to use when executing the command.
        /// </returns>
        private ProgramAndArguments DetermineProgramAndArguments()
        {
            if (Path.IsPathRooted(this.ProgramNameOrPath))
            {
                // If a full path is provided, just run the program.
                return new ProgramAndArguments(this.ProgramNameOrPath, this.Arguments.ToString());
            }
            else if (this.ProgramNameOrPath.Contains(" "))
            {
                // If the program name or path contains a space, we can't use the cmd trick below.
                return new ProgramAndArguments(this.ProgramNameOrPath, this.Arguments.ToString());
            }
            else
            {
                // Executing the command using cmd correctly sets the program path in the arguments.
                // This can be important to Batch scripts or some applications.
                return new ProgramAndArguments("cmd", String.Format("/c {0} {1}", this.ProgramNameOrPath, this.Arguments.ToString()));
            }
        }

        /// <summary>
        /// The internal implementation of command execution.
        /// </summary>
        /// <returns>
        /// The instance of the command being executed (i.e. this).
        /// </returns>
        internal virtual dynamic DoExecute()
        {
            this.PreExecute();

            Process process = new Process();

            ProgramAndArguments programAndArguments = DetermineProgramAndArguments();
            process.StartInfo.FileName = programAndArguments.Program;
            process.StartInfo.Arguments = programAndArguments.Arguments;

            foreach (KeyValuePair<String, String> environmentVariablePair in this.EnvironmentVariables)
                process.StartInfo.EnvironmentVariables.Add(environmentVariablePair.Key, environmentVariablePair.Value);

            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += ReceiveStandardOutput;
            process.StartInfo.RedirectStandardError = true;
            process.ErrorDataReceived += ReceiveStandardError;

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            this.ExitCode = process.ExitCode;
            this.Status = DetermineStatus();

            this.PostExecute();

            return this;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <returns>
        /// The instance of the command being executed (i.e. this).
        /// </returns>
        public dynamic Execute()
        {
            return DoExecute();
        }

        /// <summary>
        /// The output from the command, filtered by the specified type.
        /// </summary>
        /// <param name="filter">
        /// The type by which to filter the output.
        /// </param>
        /// <returns>
        /// The matching output lines.
        /// </returns>
        private IEnumerable<Output> FilteredOutputText(OutputType filter)
        {
            return this.OutputLines.Where((ol) => { return ol.Type == filter; });
        }

        /// <summary>
        /// Gets the text of the provided output lines.
        /// </summary>
        /// <param name="output">
        /// The lines from which to get text.
        /// </param>
        /// <returns>
        /// The test from the provided lines.
        /// </returns>
        private IEnumerable<String> GetTextFromOutput(IEnumerable<Output> output)
        {
            return output.Select((ol) => { return ol.Text; });
        }

        /// <summary>
        /// Receive output text from the command during execution and store it.
        /// </summary>
        /// <param name="outputText">
        /// The output text.
        /// </param>
        /// <param name="type">
        /// The type of the output.
        /// </param>
        private void ReceiveOutput(String outputText,
                                   OutputType type)
        {
            if (outputText != null)
                this.OutputLines.Add(new Output(outputText, type));
        }

        /// <summary>
        /// Receive STDERR output from the command during execition and store it.
        /// </summary>
        /// <param name="sendingProcess">
        /// The process that sent the output.
        /// </param>
        /// <param name="output">
        /// The outout received.
        /// </param>
        private void ReceiveStandardError(Object sendingProcess,
                                          DataReceivedEventArgs output)
        {
            ReceiveOutput(output.Data, OutputType.STDERR);
        }

        /// <summary>
        /// Receive STDOUT output from the command during execition and store it.
        /// </summary>
        /// <param name="sendingProcess">
        /// The process that sent the output.
        /// </param>
        /// <param name="output">
        /// The outout received.
        /// </param>
        private void ReceiveStandardOutput(Object sendingProcess,
                                           DataReceivedEventArgs output)
        {
            ReceiveOutput(output.Data, OutputType.STDOUT);
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Determine the status of executing the command.
        /// </summary>
        /// <returns>
        /// The default implementation of this method returns <see cref="CommandStatus.SUCCEEDED"/>
        /// if <see cref="ExitCode"/> is 0, or <see cref="CommandStatus.FAILED"/> otherwise.
        /// </returns>
        protected virtual CommandStatus DetermineStatus()
        {
            return this.ExitCode == 0 ? CommandStatus.SUCCEEDED : CommandStatus.FAILED;
        }

        /// <summary>
        /// Actions to perform after executing the command.
        /// </summary>
        protected virtual void PostExecute() { }

        /// <summary>
        /// Actions to perform before executing the command.
        /// </summary>
        protected virtual void PreExecute() { }

        #endregion
    }
}
