namespace codessentials.CGM
{
    /// <summary>
    /// Writer interface to write clear text values
    /// </summary>
    public interface IClearTextWriter
    {
        /// <summary>
        /// Writes the line to the text file
        /// </summary>
        /// <param name="line">The line to write</param>
        void WriteLine(string line);

        // <summary>
        /// Writes the text at the current position
        /// </summary>
        /// <param name="text">The text to write</param>
        void Write(string text);

        /// <summary>
        /// Logs a info message
        /// </summary>
        /// <param name="message">The message to log</param>
        void Info(string message);
    }
}
