namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=21
    /// </summary>
    public class FillBundleIndex : GenericIndexCommand
    {
        public FillBundleIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 21, container), "FILLINDEX")
        {
        }

        public FillBundleIndex(CgmFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}