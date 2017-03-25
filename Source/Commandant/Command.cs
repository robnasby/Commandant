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
        [Obsolete("This property will be removed in version 2.0.0.  Use the OutputLines property instead.")]
        protected IEnumerable<String> CombinedOutputLines { get { return GetOutputLines().ToList(); } }

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
        /// The expected exit codes from executing the program.
        /// </summary>
        /// <remarks>
        /// The default value is { 0 }.
        /// </remarks>
        protected IEnumerable<int> ExpectedExitCodes { get; set; }

        /// <summary>
        /// The internal cache of the output from executing the command.
        /// </summary>
        private List<Output> InternalOutputCache { get { return _InternalOutputCache; } }
        private List<Output> _InternalOutputCache = new List<Output>();

        /// <summary>
        /// The output lines from executing the command.
        /// </summary>
        protected IEnumerable<String> OutputLines { get { return GetOutputLines().ToList(); } }

        /// <summary>
        /// The output text from executing the command.
        /// </summary>
        protected String OutputText { get { return String.Join(Environment.NewLine, GetOutputLines()); } }

        /// <summary>
        /// The name of (or path to) the program to execute.
        /// </summary>
        private String ProgramNameOrPath { get; set; }

        /// <summary>
        /// The STDERR lines from executing the command.
        /// </summary>
        protected IEnumerable<String> StandardErrorLines { get { return GetOutputLines(OutputType.STDERR).ToList(); } }

        /// <summary>
        /// The STDERR text from executing the command.
        /// </summary>
        protected String StandardErrorText { get { return String.Join(Environment.NewLine, GetOutputLines(OutputType.STDERR)); } }

        /// <summary>
        /// The STDOUT lines from executing the command.
        /// </summary>
        protected IEnumerable<String> StandardOutputLines { get { return GetOutputLines(OutputType.STDOUT).ToList(); } }

        /// <summary>
        /// The STDOUT text from executing the command.
        /// </summary>
        protected String StandardOutputText { get { return String.Join(Environment.NewLine, GetOutputLines(OutputType.STDOUT)); } }

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
            this.ExpectedExitCodes = new int[] { 0 };
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
        /// Gets the output lines of text.
        /// </summary>
        /// <returns>
        /// The output lines of text.
        /// </returns>
        private IEnumerable<String> GetOutputLines()
        {
            return this.InternalOutputCache.Select((o) => { return o.Text; });
        }

        /// <summary>
        /// Gets the output lines of text that match the specified type.
        /// </summary>
        /// <param name="type">
        /// The type by which to filter the output.
        /// </param>
        /// <returns>
        /// The output lines of text.
        /// </returns>
        private IEnumerable<String> GetOutputLines(OutputType type)
        {
            return this.InternalOutputCache.Where((o) => { return o.Type == type; }).Select((o) => { return o.Text; });
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
                this.InternalOutputCache.Add(new Output(outputText, type));
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
        /// if <see cref="ExitCode"/> is one of the values in <see cref="ExpectedExitCode"/>,
        /// or <see cref="CommandStatus.FAILED"/> otherwise.
        /// </returns>
        protected virtual CommandStatus DetermineStatus()
        {
            return this.ExpectedExitCodes.Contains(this.ExitCode) ? CommandStatus.SUCCEEDED : CommandStatus.FAILED;
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
