namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=1
    /// </summary>
    public class LineBundleIndex : GenericIndexCommand
    {   
        public LineBundleIndex(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 1, container), "LINEINDEX")
        {
           
        }

        public LineBundleIndex(CGMFile container, int index)
            :this(container)
        {
            SetValue(index);
        }
    }
}