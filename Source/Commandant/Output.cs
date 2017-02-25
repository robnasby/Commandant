using System;

namespace Commandant
{
    /// <summary>
    /// A line of output from executing a command.
    /// </summary>
    internal class Output
    {
        #region Properties

        /// <summary>
        /// The text of the output.
        /// </summary>
        public String Text { get; private set; }

        /// <summary>
        /// The type of the output.
        /// </summary>
        public OutputType Type { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates an instance of <see cref="Output"/>.
        /// </summary>
        /// <param name="text">
        /// The text of the output.
        /// </param>
        /// <param name="type">
        /// The type of the output.
        /// </param>
        public Output(String text,
                      OutputType type)
        {
            this.Text = text;
            this.Type = type;
        }

        #endregion
    }
}
