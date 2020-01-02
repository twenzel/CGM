using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class ExternalElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch ((ExternalElement)elementId)
            {
                case ExternalElement.MESSAGE:
                    return new MessageCommand(container);
                case ExternalElement.APPLICATION_DATA:
                    return new ApplicationData(container);
                default:
                    return new UnknownCommand(elementId, elementClass, container);
            }
        }
    }
}
