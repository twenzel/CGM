namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=9
    /// </summary>
    public class TextBundleIndex : GenericIndexCommand
    {
        public TextBundleIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 9, container), "TEXTINDEX")
        {

        }

        public TextBundleIndex(CgmFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}
