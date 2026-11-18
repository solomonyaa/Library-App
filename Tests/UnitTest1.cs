using System;
using System.Collections.Generic;
using BookLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        readonly ItemCollection1 ic1 = new ItemCollection1();

        readonly Book b = new Book("title", "author", 100, 1, "publisher", new DateTime(2000, 1, 1), Book.Genre.Romance);
        readonly Book b2 = new Book("title2", "author2", 200, 1, "publisher", new DateTime(2000, 1, 1), Book.Genre.Horror);

        readonly Journal j = new Journal("title", 50, 999, "publisher", new DateTime(2020, 1, 1), Journal.Category.Medicine);
        readonly Journal j2 = new Journal("title", 29, 999, "publisher", new DateTime(2020, 1, 1), Journal.Category.Culture);

        [TestMethod]
        public void TestAddAndRemoveItem()
        {
            ic1.AddItem(b);
            ic1.AddItem(j);
            Assert.IsTrue(ic1.Items.Contains(b));
            Assert.IsTrue(ic1.Items.Contains(j));

            ic1.RemoveItem(b);
            ic1.RemoveItem(j);
            Assert.IsFalse(ic1.Items.Contains(b));
            Assert.IsFalse(ic1.Items.Contains(j));
        }

        [TestMethod]
        public void TestRemoveItem()
        {
            Assert.ThrowsException<MissingItemException>(() => ic1.RemoveItem(b));
        }

        [TestMethod]
        public void TestCheckExistence()
        {
            Assert.IsFalse(ic1.CheckExistence(j));
            ic1.AddItem(j);
            Assert.IsTrue(ic1.CheckExistence(j));
        }

        [TestMethod]
        public void TestGetItemsByTitle()
        {
            ic1.AddItem(b);
            ic1.AddItem(j);

            List<AbstractItem> tempList = ic1.GetItemsByTitle("title", ic1.Items);
            Assert.IsTrue(tempList[0].Title == "title");
            Assert.IsTrue(tempList[1].Title == "title");
        }

        [TestMethod]
        public void TestGetItemsByPrice()
        {
            ic1.AddItem(b);
            ic1.AddItem(j);

            List<AbstractItem> tempList = ic1.GetItemsByPrice(50, 100, ic1.Items);
            Assert.IsTrue(tempList[0].Price >= 50 && tempList[0].Price <= 100);
            Assert.IsTrue(tempList[1].Price >= 50 && tempList[1].Price <= 100);

            Assert.IsFalse(tempList[1].Price >= 0 && tempList[1].Price <= 49);
            Assert.IsFalse(tempList[1].Price >= 101 && tempList[1].Price <= double.MaxValue);
        }

        [TestMethod]
        public void TestGetItemsByTitleAndPrice()
        {
            ic1.AddItem(b);
            ic1.AddItem(j);

            List<AbstractItem> tempList = ic1.GetItemsByTitleAndPrice("title", 50, 100, ic1.Items);
            Assert.IsTrue(tempList[0].Title == "title" && tempList[0].Price >= 50 && tempList[0].Price <= 100);
            Assert.IsTrue(tempList[1].Title == "title" && tempList[1].Price >= 50 && tempList[1].Price <= 100);

            Assert.IsFalse(tempList[1].Title == "title" && tempList[1].Price >= 0 && tempList[1].Price <= 49);
            Assert.IsFalse(tempList[1].Title == "title" && tempList[1].Price >= 101 && tempList[1].Price <= double.MaxValue);
        }

        [TestMethod]
        public void TestGetBooksByTitleAndGenre()
        {
            List<Book> bookList;
            ic1.AddItem(b);

            bookList = ic1.GetBooksByTitleAndGenre("title", Book.Genre.Romance, ic1.GetBooks());
            Assert.IsTrue(bookList[0].Title == "title" && bookList[0].BookGenre == Book.Genre.Romance);
            Assert.IsFalse(bookList[0].Title == "title" && bookList[0].BookGenre == Book.Genre.Comedy);
            Assert.IsFalse(bookList[0].Title == "title2" && bookList[0].BookGenre == Book.Genre.Romance);
        }

        [TestMethod]
        public void TestGetBooksByPriceAndGenre()
        {
            List<Book> bookList;
            ic1.AddItem(b);
            ic1.AddItem(b2);


            bookList = ic1.GetBooksByPriceAndGenre(50, 100, Book.Genre.Romance, ic1.GetBooks());
            Assert.IsTrue(bookList[0].Price >= 50 && bookList[0].Price <= 100 && bookList[0].BookGenre == Book.Genre.Romance);
            Assert.IsTrue(bookList[1].Price >= 200 && bookList[1].Price <= 200 && bookList[1].BookGenre == Book.Genre.Horror);
            Assert.IsFalse(bookList[1].Price >= 200 && bookList[1].Price <= 200 && bookList[1].BookGenre == Book.Genre.Romance);
            Assert.IsFalse(bookList[1].Price >= 0 && bookList[1].Price <= 100 && bookList[1].BookGenre == Book.Genre.Romance);
        }

        [TestMethod]
        public void TestGetJournalsByTitleAndCategory()
        {
            List<Journal> journalList;
            ic1.AddItem(j);
            ic1.AddItem(j2);

            journalList = ic1.GetJournalsByTitleAndCategory("title" ,Journal.Category.Culture, ic1.GetJournals());
            Assert.IsTrue(journalList[0].Title == "title" && journalList[0].JournalCategory == Journal.Category.Medicine);
            Assert.IsTrue(journalList[1].Title == "title" && journalList[1].JournalCategory == Journal.Category.Culture);
            Assert.IsFalse(journalList[1].Title == "title2" && journalList[1].JournalCategory == Journal.Category.Culture);
            Assert.IsFalse(journalList[0].Title == "title" && journalList[0].JournalCategory == Journal.Category.Computers);
        }

        [TestMethod]
        public void TestGetJournalsByPriceAndCategory()
        {
            List<Journal> journalList;
            ic1.AddItem(j);
            ic1.AddItem(j2);

            journalList = ic1.GetJournalsByPriceAndCategory(0, 30, Journal.Category.Culture, ic1.GetJournals());
            Assert.IsFalse(journalList[0].Price >= 0 && journalList[0].Price <= 30 && journalList[0].JournalCategory == Journal.Category.Medicine);
            Assert.IsFalse(journalList[0].Price >= 30 && journalList[0].Price <= 200 && journalList[0].JournalCategory == Journal.Category.Culture);
            Assert.IsTrue(journalList[0].Price >= 0 && journalList[0].Price <= 50 && journalList[0].JournalCategory == Journal.Category.Medicine);
            Assert.IsTrue(journalList[1].Price >= 0 && journalList[1].Price <= 30 && journalList[1].JournalCategory == Journal.Category.Culture);
        }

        [TestMethod]
        public void TestExtractByName()
        {
            ic1.AddItem(j);
            ic1.AddItem(j2);

            Assert.IsTrue(ic1.ExtractByName("title", out AbstractItem ai));
            Assert.IsTrue(ai.Title == "title");

            Assert.IsFalse(ic1.ExtractByName("title2", out AbstractItem ai2));
            Assert.IsTrue(ai2 == null);
        }

        [TestMethod]
        public void TestExtractByPrice()
        {
            ic1.AddItem(b);
            ic1.AddItem(b2);

            List<AbstractItem> tempList = ic1.ExtractByPrice(100, 200);
            Assert.IsTrue(tempList.Contains(b) && tempList.Contains(b2));

            Assert.ThrowsException<NoMatchException>(() => ic1.ExtractByPrice(0, 99));
        }

        [TestMethod]
        public void TestIndexer()
        {
            ic1.AddItem(b);
            ic1.AddItem(b2);

            Assert.IsTrue(ic1["title"] == b);
            Assert.IsTrue(ic1["title2"] == b2);

            Assert.ThrowsException<KeyNotFoundException>(() => ic1["title3"]);
        }

        [TestMethod]
        public void TestSetDiscount()
        {
            ic1.AddItem(b);
            ic1.AddItem(j);

            ic1.SetDiscount(b, 10);
            Assert.IsTrue(b.Price == 90);

            ic1.SetDiscount(j, 10);
            Assert.IsTrue(j.Price == 42.5); // Disount of 15% is expected

            Assert.ThrowsException<DiscountException>(() => ic1.SetDiscount(b, 110));
            Assert.ThrowsException<DiscountException>(() => ic1.SetDiscount(b, -5));
        }

        [TestMethod]
        public void TestEquals()
        {
            Assert.IsTrue(b.Equals(b));
            Assert.IsFalse(b.Equals(b2));

            Assert.IsTrue(j.Equals(j));
            Assert.IsFalse(j.Equals(j2));

            Assert.IsFalse(b.Equals(j));
            Assert.IsFalse(j.Equals(b));
        }
    }
}
