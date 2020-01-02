using codessentials.CGM.Commands;

namespace codessentials.CGM.Import
{
    /// <summary>
    /// Factory interface to create commands
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Create a new command instance
        /// </summary>
        /// <param name="elementId">The command Id</param>
        /// <param name="elementClass">The command class</param>
        /// <param name="container">The parent container</param>
        /// <returns></returns>
        Command CreateCommand(int elementId, int elementClass, CGMFile container);
    }
}
