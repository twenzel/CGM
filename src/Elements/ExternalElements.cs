using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class ExternalElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CgmFile container)
        {
            return ((ExternalElement)elementId) switch
            {
                ExternalElement.MESSAGE => new MessageCommand(container),
                ExternalElement.APPLICATION_DATA => new ApplicationData(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}
