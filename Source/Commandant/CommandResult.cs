using System;

namespace Commandant
{
    /// <summary>
    /// An abstract class representing a program to execute.
    /// </summary>
    public abstract class Command<TResult> : Command
    {
        #region Properties

        /// <summary>
        /// The result of executing the command.
        /// </summary>
        public TResult Result { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an instance of <see cref="Command"/>.
        /// </summary>
        /// <param name="programNameOrPath">
        /// The name of (or path to) the program to execute.
        /// </param>
        public Command(String programNameOrPath)
            : base(programNameOrPath)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// The internal implementation of command execution.
        /// </summary>
        /// <returns>
        /// The instance of the command being executed (i.e. this).
        /// </returns>
        internal override dynamic DoExecute()
        {
            base.DoExecute();

            this.Result = BuildResult();

            return this;
        }

        #endregion

        #region Abstract and Virtual Methods

        /// <summary>
        /// Build the result to be returned after command execution.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="TResult"/>.
        /// </returns>
        protected abstract TResult BuildResult();

        #endregion
    }
}
