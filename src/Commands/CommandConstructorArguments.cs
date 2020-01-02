namespace codessentials.CGM.Commands
{
    public class CommandConstructorArguments
    {
        public ClassCode ElementClass { get; set; }
        public int ElementId { get; set; }
        public CGMFile Container { get; set; }

        public CommandConstructorArguments()
        {

        }

        public CommandConstructorArguments(ClassCode elementClass, int elementId, CGMFile container)
        {
            ElementClass = elementClass;
            ElementId = elementId;
            Container = container;
        }
    }
}
