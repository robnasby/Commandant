using System;

namespace Commandant
{
    /// <summary>
    /// The program and arguments to use when executing the command.
    /// </summary>
    internal class ProgramAndArguments
    {
        #region Properties

        /// <summary>
        /// The arguments to use when executing the command.
        /// </summary>
        public String Arguments { get; private set; }

        /// <summary>
        /// The program to use when executing the command.
        /// </summary>
        public String Program { get; private set; }

        #endregion

        #region Construcors

        /// <summary>
        /// Instantiates an instance of <see cref="ProgramAndArguments"/>.
        /// </summary>
        /// <param name="program">
        /// The program to use when executing the command.
        /// </param>
        /// <param name="arguments">
        /// The arguments to use when executing the command.
        /// </param>
        public ProgramAndArguments(String program,
                                   String arguments)
        {
            this.Arguments = arguments;
            this.Program = program;
        }

        #endregion
    }
}
