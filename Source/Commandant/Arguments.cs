using System;
using System.Collections.Generic;

namespace Commandant
{
    public class Arguments
    {
        #region Properties

        /// <summary>
        /// The cache of arguments.
        /// </summary>
        private List<String> ArgumentsCache { get { return _ArgumentsCache; } }
        private List<String> _ArgumentsCache = new List<String>();

        #endregion

        #region Methods

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(Boolean argument)
        {
            this.Append(argument.ToString());
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(Char argument)
        {
            this.Append(argument.ToString());
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(Decimal argument)
        {
            this.Append(argument.ToString());
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(Double argument)
        {
            this.Append(argument.ToString());
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(int argument)
        {
            this.Append(argument.ToString());
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(Object argument)
        {
            this.Append(argument.ToString());
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(Single argument)
        {
            this.Append(argument.ToString());
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(String argument)
        {
            this.ArgumentsCache.Add(argument);
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="argument">
        /// The argument to append.
        /// </param>
        public void Append(uint argument)
        {
            this.Append(argument.ToString());
        }

        /// <summary>
        /// Append an argument.
        /// </summary>
        /// <param name="format">
        /// A composite format string.
        /// </param>
        /// <param name="args">
        /// An object array that contains zero or more objects to format.
        /// </param>
        public void Append(String format,
                    params Object[] args)
        {
            this.Append(String.Format(format, args));
        }

        /// <summary>
        /// Append a set of arguments.
        /// </summary>
        /// <param name="arguments">
        /// The arguments to append.
        /// </param>
        public void Append(Arguments arguments)
        {
            foreach (String argument in arguments.ArgumentsCache)
                this.Append(argument);
        }

        /// <summary>
        /// Returns the set of arguments as a <see cref="String"/>.
        /// </summary>
        /// <returns>
        /// The argument set as a <see cref="String"/>.
        /// </returns>
        public override string ToString()
        {
            return String.Join(" ", this.ArgumentsCache);
        }

        #endregion
    }
}
