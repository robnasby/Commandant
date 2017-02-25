using System;
using System.Diagnostics;

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
        /// The exit code from executing the program.
        /// </summary>
        protected int ExitCode { get; private set; }

        /// <summary>
        /// The name of (or path to) the program to execute.
        /// </summary>
        private String ProgramNameOrPath { get; set; }

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
        /// Execute the command.
        /// </summary>
        /// <returns>
        /// The instance of the command being executed (i.e. this).
        /// </returns>
        public dynamic Execute()
        {
            Process process = new Process();

            process.StartInfo.FileName = this.ProgramNameOrPath;
            process.StartInfo.Arguments = this.Arguments.ToString();

            process.StartInfo.UseShellExecute = false;

            process.Start();

            process.WaitForExit();

            this.ExitCode = process.ExitCode;
            this.Status = DetermineStatus();

            return this;
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
