using System;
using System.Text;

namespace BookLib
{
    public class Book : AbstractItem
    {
        public string Author { get; private set; }
        public string Isbn { get; private set; }
        public Genre BookGenre { get; private set; }

        public Book(string title, string author, double price, int edition, string publisher, DateTime publicationDate, Genre genre)
            : base(title, price, edition, publisher, publicationDate)
        {
            this.Author = author;
            Isbn = Guid.NewGuid().ToString();
            BookGenre = genre;
        }

        [Flags]
        public enum Genre
        {
            Adventure = 1,
            Comedy = 2,
            Fantasy = 4,
            Horror = 8,
            Mystery = 16,
            Romance = 32,
        }

        internal override void SetDiscount(double discountPercents)
        {
            base.SetDiscount(discountPercents);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || this.GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                Book book = (Book)obj;
                return this.Isbn == book.Isbn;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string GetDetailes()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Author: " + Author);
            sb.AppendLine("Genre: " + BookGenre);
            sb.AppendLine("ISBN: " + Isbn);

            return base.GetDetailes() + sb.ToString();
        }
    }
}
