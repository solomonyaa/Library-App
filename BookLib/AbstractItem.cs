using System;
using System.Globalization;
using System.Text;

namespace BookLib
{
    public abstract class AbstractItem : IComparable<AbstractItem>
    {
        public string Title { get; private set; }
        public double Price { get; private set; }
        public int Edition { get; private set; }
        public string Publisher { get; private set; }
        public string PublicationDate { get; private set; }
        public int NumOfCopies { get; private set; }

        public AbstractItem(string title, double price, int edition, string publisher, DateTime publicationDate)
        {
            this.Title = title;
            this.Price = price;
            this.Edition = edition;
            this.Publisher = publisher;
            this.PublicationDate = publicationDate.ToShortDateString();

            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Publisher) || string.IsNullOrEmpty(PublicationDate) || Price <= 0)
                throw new MissingDetailesException();

            NumOfCopies++;
        }

        internal virtual void SetDiscount(double discountPercents = 0)
        {
            Price -= Price * discountPercents / 100;
        }

        public void AddCopies(int copiesToAdd)
        {
            NumOfCopies += copiesToAdd;
        }
        public void RemoveCopies(int copiesToRemove)
        {
            NumOfCopies -= copiesToRemove;
        }
        public virtual string GetDetailes()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Type: " + GetType().ToString().Remove(0, 8));
            sb.AppendLine("Title: " + Title);
            sb.AppendLine("Price: " + Price.ToString("C", CultureInfo.CurrentCulture));
            sb.AppendLine("Edition: " + Edition);
            sb.AppendLine("Publisher: " + Publisher);
            sb.AppendLine("Publication Date: " + PublicationDate);
            sb.AppendLine("Number of Copies: " + NumOfCopies);

            return sb.ToString();
        }
        public int CompareTo(AbstractItem other) => this.Price.CompareTo(other.Price);
    }
}
