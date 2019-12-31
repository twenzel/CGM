namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=9
    /// </summary>
    public class TextBundleIndex : GenericIndexCommand
    {        
        public TextBundleIndex(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 9, container), "TEXTINDEX")
        {
           
        }

        public TextBundleIndex(CGMFile container, int index)
            :this(container)
        {
            SetValue(index);
        }
    }
}