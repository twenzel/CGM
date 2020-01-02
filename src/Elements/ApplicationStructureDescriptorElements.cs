using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class ApplicationStructureDescriptorElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            return ((ApplicationStructureDescriptorElement)elementId) switch
            {
                ApplicationStructureDescriptorElement.APPLICATION_STRUCTURE_ATTRIBUTE => new ApplicationStructureAttribute(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}
