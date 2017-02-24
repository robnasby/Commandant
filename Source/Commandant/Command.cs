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
        /// The name of (or path to) the program to execute.
        /// </summary>
        private String ProgramNameOrPath { get; set; }

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
        public void Execute()
        {
            Process process = new Process();

            process.StartInfo.FileName = this.ProgramNameOrPath;

            process.StartInfo.UseShellExecute = false;

            process.Start();

            process.WaitForExit();
        }

        #endregion
    }
}
