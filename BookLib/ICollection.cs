using System.Collections.Generic;

namespace BookLib
{
    public interface ICollection
    {
        List<AbstractItem> Items { get; set; }
        void AddItem(AbstractItem item);
        void RemoveItem(AbstractItem item);
    }
}
