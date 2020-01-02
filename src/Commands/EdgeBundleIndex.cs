namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=26
    /// </summary>
    public class EdgeBundleIndex : GenericIndexCommand
    {
        public EdgeBundleIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 26, container), "EDGEINDEX")
        {
        }

        public EdgeBundleIndex(CgmFile container, int index)
            : this(container)
        {
            Index = index;
        }
    }
}