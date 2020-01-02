using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class ApplicationStructureDescriptorElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch ((ApplicationStructureDescriptorElement)elementId)
            {
                case ApplicationStructureDescriptorElement.APPLICATION_STRUCTURE_ATTRIBUTE:
                    return new ApplicationStructureAttribute(container);
                default:
                    return new UnknownCommand(elementId, elementClass, container);
            }
        }
    }
}
