using System;
using System.Text;

namespace BookLib
{
    public class Journal : AbstractItem
    {
        public string Issn { get; private set; }
        public Category JournalCategory { get; private set; }
        public Journal(string title, double price, int edition, string publisher, DateTime publicationDate, Category category)
            : base(title, price, edition, publisher, publicationDate) 
        {
            Issn = Guid.NewGuid().ToString();
            JournalCategory = category;
        }
        [Flags]
        public enum Category
        {
            Science = 1,
            Nature = 2,
            Computers = 4,
            Geography = 8,
            Medicine = 16,
            Culture = 32,
        }

        internal override void SetDiscount(double discountPercents)
        {
            base.SetDiscount(discountPercents + 5);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || this.GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                Journal journal = (Journal)obj;
                return this.Issn == journal.Issn;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string GetDetailes()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Category: " + JournalCategory);
            sb.AppendLine("ISSN: " + Issn);

            return base.GetDetailes() + sb.ToString();
        }
    }
}
