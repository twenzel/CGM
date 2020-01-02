namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=5
    /// </summary>
    public class MarkerBundleIndex : GenericIndexCommand
    {
        public MarkerBundleIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 5, container), "MARKERINDEX")
        {
        }

        public MarkerBundleIndex(CgmFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}
