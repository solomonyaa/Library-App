using System.Collections.Generic;
using System.Linq;

namespace BookLib
{
    public class ItemCollection1 : ICollection
    {
        private List<AbstractItem> items;
        public List<AbstractItem> Items
        {
            get { return items; }
            set { items = value; }
        }
        public ItemCollection1() => Items = new List<AbstractItem>();
        public ItemCollection1(List<AbstractItem> abstractItems) => Items = abstractItems;
        public bool CheckExistence(AbstractItem item)
        {
            bool isFound = false;
            for (int i = 0; i < Items.Count; i++)
            {
                if (item.Title == Items[i].Title)
                {
                    isFound = true;
                    break;
                }
            }
            return isFound;
        }
        public void AddItem(AbstractItem item) => Items.Add(item);
        public void RemoveItem(AbstractItem item)
        {
            if (Items.Contains(item) == true)
                Items.Remove(item);
            else
                throw new MissingItemException();
        }
        public List<AbstractItem> GetItemsByTitle(string title, List<AbstractItem> selectedItems)
        {
            selectedItems.AddRange(Items.Where(x => x.Title.Equals(title)).ToList());
            return selectedItems;
        }
        public List<AbstractItem> GetItemsByPrice(double fromPrice, double toPrice, List<AbstractItem> selectedItems)
        {
            selectedItems.AddRange(Items.Where(i => i.Price >= fromPrice && i.Price <= toPrice).ToList());
            return selectedItems;
        }
        public List<AbstractItem> GetItemsByTitleAndPrice(string title, double fromPrice, double toPrice, List<AbstractItem> selectedItems)
        {
            selectedItems.AddRange(Items.Where(i => i.Price >= fromPrice && i.Price <= toPrice && i.Title == title).ToList());
            return selectedItems;
        }
        public List<Book> GetBooksByTitleAndGenre(string title, Book.Genre genre, List<Book> selectedItems)
        {
            selectedItems.AddRange(GetBooks().Where(b => b.Title == title && b.BookGenre.HasFlag(genre)).ToList());
            return selectedItems;
        }
        public List<Book> GetBooksByPriceAndGenre(double fromPrice, double toPrice, Book.Genre genre, List<Book> selectedItems)
        {
            selectedItems.AddRange(GetBooks().Where(b => b.Price >= fromPrice && b.Price <= toPrice && b.BookGenre.HasFlag(genre)).ToList());
            return selectedItems;
        }
        public List<Journal> GetJournalsByTitleAndCategory(string title, Journal.Category category, List<Journal> selectedItems)
        {
            selectedItems.AddRange(GetJournals().Where(j => j.Title == title && j.JournalCategory.HasFlag(category)).ToList());
            return selectedItems;
        }
        public List<Journal> GetJournalsByPriceAndCategory(double fromPrice, double toPrice, Journal.Category category, List<Journal> selectedItems)
        {
            selectedItems.AddRange(GetJournals().Where(j => j.Price >= fromPrice && j.Price <= toPrice && j.JournalCategory.HasFlag(category)).ToList());
            return selectedItems;
        }

        public bool ExtractByName(string title, out AbstractItem abstractItem)
        {
            foreach (AbstractItem item in Items)
            {
                if (item.Title == title)
                {
                    abstractItem = item;
                    return true;
                }
            }
            abstractItem = default;
            return false;
        }
        public List<AbstractItem> ExtractByPrice(double fromPrice, double toPrice)
        {
            List<AbstractItem> tempList = new List<AbstractItem>();

            foreach (var item in Items)
            {
                if (item.Price >= fromPrice && item.Price <= toPrice)
                {
                    tempList.Add(item);
                }
            }
            if (tempList.Count == 0)
            {
                throw new NoMatchException("No matching items were found.");
            }
            else
            {
                return tempList;
            }
        }
        public AbstractItem this[string key] // indexer
        {
            get
            {
                foreach (var item in Items)
                {
                    if (key == item.Title)
                    {
                        return item;
                    }
                }
                foreach (var item in Items)
                {
                    if (item is Book)
                    {
                        Book b = item as Book;
                        if (key == b.Isbn)
                        {
                            return b;
                        }
                    }
                    else if (item is Journal)
                    {
                        Journal j = item as Journal;
                        if (key == j.Issn)
                        {
                            return j;
                        }
                    }
                }
                throw new KeyNotFoundException("Key was not found");
            }
        }
        public List<Book> GetBooks() => Items.OfType<Book>().ToList();
        public List<Journal> GetJournals() => Items.OfType<Journal>().ToList();
        public List<Book> GetBooks(List<AbstractItem> abstractItems)
        {
            return abstractItems.OfType<Book>().ToList();
        }
        public List<Journal> GetJournals(List<AbstractItem> abstractItems)
        {
            return abstractItems.OfType<Journal>().ToList();
        }
        public void SetDiscount(AbstractItem item, double discount)
        {
            if (discount >= 0 && discount <= 100)
            {
                if (item.GetType() == typeof(Book))
                {
                    Book b = item as Book;
                    b.SetDiscount(discount);
                }
                else if (item.GetType() == typeof(Journal))
                {
                    Journal j = item as Journal;
                    j.SetDiscount(discount);
                }
            }
            else
            {
                throw new DiscountException();
            }
        }
    }
}