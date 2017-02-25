﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// The internal implementation of command execution.
        /// </summary>
        /// <returns>
        /// The instance of the command being executed (i.e. this).
        /// </returns>
        internal virtual dynamic DoExecute()
        {
            Process process = new Process();

            process.StartInfo.FileName = this.ProgramNameOrPath;
            process.StartInfo.Arguments = this.Arguments.ToString();

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

        #endregion
    }
}
