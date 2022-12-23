namespace ATBS.MenuSystem
{
    /// <summary>
    /// Used for handling interactions with an element
    /// <summary>
    public interface IElementHandler
    {
        public Element SelectedElement {get; set;}
        public bool IgnoreDeselect {get; set;}
    }
}
