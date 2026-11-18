using BookLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ItemCollection1 = BookLib.ItemCollection1;

namespace WpfLibrary
{
    public partial class MainWindow : Window
    {
        private readonly ItemCollection1 itemCollc;
        internal readonly ReportWindow report;
        public MainWindow()
        {
            InitializeComponent();

            itemCollc = new ItemCollection1();
            report = new ReportWindow();

            cmbxMainGenreBook.ItemsSource = Enum.GetValues(typeof(Book.Genre)).Cast<Book.Genre>();
            cmbxSubGenreBook.ItemsSource = Enum.GetValues(typeof(Book.Genre)).Cast<Book.Genre>();
            cmbxMainCategoryJournal.ItemsSource = Enum.GetValues(typeof(Journal.Category)).Cast<Journal.Category>();
            cmbxSubCategoryJournal.ItemsSource = Enum.GetValues(typeof(Journal.Category)).Cast<Journal.Category>();

            itemCollc.AddItem(new Book("The Lord of the Rings", "J.R.R. Tolkien", 91.5, 1, "Allen & Unwin", new DateTime(1954, 7, 29), Book.Genre.Fantasy | Book.Genre.Adventure));
            itemCollc.AddItem(new Book("Harry Potter and the Philosopher's Stone", "J.K. Rowling", 88, 1, "Bloomsbury", new DateTime(1997, 6, 26), Book.Genre.Fantasy));
            itemCollc.AddItem(new Book("The Hitchhiker's Guide to the Galaxy", "Douglas Adams", 79, 1, "Pan Books", new DateTime(1979, 10, 12), Book.Genre.Comedy));

            itemCollc.AddItem(new Journal("National Geographic", 45, 1271, "NG Media", new DateTime(2019, 10, 1), Journal.Category.Geography | Journal.Category.Nature));
            itemCollc.AddItem(new Journal("Scientific American", 55, 1523, "Springer Nature", new DateTime(2020, 7, 1), Journal.Category.Science));
            itemCollc.AddItem(new Journal("Rolling Stone", 61.90, 837, "Penske Media Corporation", new DateTime(2016, 1, 1), Journal.Category.Culture));

            dgBooks.ItemsSource = itemCollc.GetBooks();
            dgJournals.ItemsSource = itemCollc.GetJournals();
        }

        private void AddItemClick(object sender, RoutedEventArgs e)
        {
            if (tbxTitle.Text == "")
            {
                MessageBox.Show("Please insert a title");
                return;
            }
            if (tbxPrice.Text == "")
            {
                MessageBox.Show("Please insert a price");
                return;
            }
            if (tbxEdition.Text == "")
            {
                MessageBox.Show("Please insert an edition");
                return;
            }
            if (tbxPublisher.Text == "")
            {
                MessageBox.Show("Please insert a publisher");
                return;
            }
            if (datePicker1.SelectedDate is null)
            {
                MessageBox.Show("Please select a date");
                return;
            }

            string title = tbxTitle.Text;
            string author = tbxAuthor.Text;
            double.TryParse(tbxPrice.Text, out double price);
            int.TryParse(tbxEdition.Text, out int edition);
            string publisher = tbxPublisher.Text;
            DateTime date = datePicker1.SelectedDate.Value.Date;
            double.TryParse(tbxDiscount.Text, out double discount);
            bool isAdded = false;

            if (rdbtnBook.IsChecked == true)
            {
                if (cmbxMainGenreBook.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select the main genre for the book");
                    return;
                }
                Book.Genre bookGenre = (Book.Genre)cmbxMainGenreBook.SelectedItem;

                if (cmbxSubGenreBook.SelectedIndex > 0)
                {
                    bookGenre |= (Book.Genre)cmbxSubGenreBook.SelectedItem;
                }

                try
                {
                    Book book = new Book(title, author, price, edition, publisher, date, bookGenre);
                    try { itemCollc.SetDiscount(book, discount); }
                    catch (DiscountException) { MessageBox.Show("Discount percentage is not between 0 and 100"); }

                    if (!itemCollc.CheckExistence(book))
                    {
                        itemCollc.AddItem(book);
                        isAdded = true;
                        dgBooks.ItemsSource = itemCollc.GetBooks();
                    }
                    else
                    {
                        MessageBox.Show($"The Book '{book.Title}' already exists in the library.");
                        return;
                    }
                }
                catch (MissingDetailesException)
                {
                    MessageBox.Show("Some detailes are missing.\nPlease try again.");
                }
            }
            else if (rdbtnJournal.IsChecked == true)
            {
                if (cmbxMainCategoryJournal.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select the main genre for the journal");
                    return;
                }
                Journal.Category categoryJournal = (Journal.Category)cmbxMainCategoryJournal.SelectedItem;

                if (cmbxSubCategoryJournal.SelectedIndex > 0)
                {
                    categoryJournal |= (Journal.Category)cmbxSubCategoryJournal.SelectedItem;
                }

                try
                {
                    Journal journal = new Journal(title, price, edition, publisher, date, categoryJournal);
                    try { itemCollc.SetDiscount(journal, discount); }
                    catch (DiscountException) { MessageBox.Show("Discount percentage is not between 0 and 100 is not between 0 and 100"); }

                    if (!itemCollc.CheckExistence(journal))
                    {
                        itemCollc.AddItem(journal);
                        isAdded = true;
                        dgJournals.ItemsSource = itemCollc.GetJournals();
                    }
                    else
                    {
                        MessageBox.Show($"The Journal '{journal.Title}' already exists in the library.");
                        return;
                    }
                }
                catch (MissingDetailesException)
                {
                    MessageBox.Show("Some detailes are missing.\nPlease try again.");
                }
            }
            else
            {
                MessageBox.Show("Please select the item type.");
                return;
            }
            if (isAdded)
            {
                rdbtnBook.IsChecked = false;
                rdbtnJournal.IsChecked = false;
                tbxTitle.Text = "";
                tbxAuthor.Text = "";
                tbxPrice.Text = "";
                tbxEdition.Text = "";
                tbxPublisher.Text = "";
                datePicker1.SelectedDate = null;
                tbxDiscount.Text = "";
                cmbxMainGenreBook.SelectedItem = null;
                cmbxSubGenreBook.SelectedItem = null;
                cmbxMainCategoryJournal.SelectedItem = null;
                cmbxSubCategoryJournal.SelectedItem = null;
            }
        }
        private void RemoveBook_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem == null)
            {
                MessageBox.Show("Please select an item from the books shelf");
                return;
            }

            AbstractItem b = (AbstractItem)dgBooks.SelectedItem;
            if (b.NumOfCopies > 1)
            {
                b.RemoveCopies(1);
            }
            else
            {
                try
                {
                    itemCollc.RemoveItem(b);
                }
                catch (MissingItemException)
                {
                    MessageBox.Show("Such item does not exist in the library");
                    return;
                }
            }
            dgBooks.ItemsSource = itemCollc.GetBooks();
        }
        private void RemoveJournal_Click(object sender, RoutedEventArgs e)
        {
            if (dgJournals.SelectedItem == null)
            {
                MessageBox.Show("Please select an item from the journals shelf");
                return;
            }

            AbstractItem j = (AbstractItem)dgJournals.SelectedItem;
            if (j.NumOfCopies > 1)
            {
                j.RemoveCopies(1);
            }
            else
            {
                try
                {
                    itemCollc.RemoveItem(j);
                }
                catch (MissingItemException)
                {
                    MessageBox.Show("Such item does not exist in the library");
                    return;
                }
            }
            dgJournals.ItemsSource = itemCollc.GetJournals();
        }

        private List<AbstractItem> SortByTitle(string titleToFind, List<AbstractItem> selectedItems)
        {
            if (!string.IsNullOrEmpty(titleToFind))
                selectedItems = itemCollc.GetItemsByTitle(titleToFind, selectedItems);
            return selectedItems;
        }
        private List<AbstractItem> SortByPrice(string tbxFromPrice, string tbxToPrice, double fromPrice, double toPrice, List<AbstractItem> selectedItems)
        {
            if (!string.IsNullOrEmpty(tbxFromPrice) || !string.IsNullOrEmpty(tbxToPrice))
            {
                if (!string.IsNullOrEmpty(tbxFromPrice) && !string.IsNullOrEmpty(tbxToPrice))
                    selectedItems = itemCollc.GetItemsByPrice(fromPrice, toPrice, selectedItems);
                else if (!string.IsNullOrEmpty(tbxFromPrice) && string.IsNullOrEmpty(tbxToPrice))
                    selectedItems = itemCollc.GetItemsByPrice(fromPrice, Double.MaxValue, selectedItems);
                else
                    selectedItems = itemCollc.GetItemsByPrice(0, toPrice, selectedItems);
            }
            return selectedItems;
        }
        private List<AbstractItem> SortByGenre(List<AbstractItem> selectedItems)
        {
            if (chkbxAdventure.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetBooks().Where(b => b.BookGenre.HasFlag(Book.Genre.Adventure)).ToList());
            }
            if (chkbxComedy.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetBooks().Where(b => b.BookGenre.HasFlag(Book.Genre.Comedy)).ToList());
            }
            if (chkbxFantasy.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetBooks().Where(b => b.BookGenre.HasFlag(Book.Genre.Fantasy)).ToList());
            }
            if (chkbxHorror.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetBooks().Where(b => b.BookGenre.HasFlag(Book.Genre.Horror)).ToList());
            }
            if (chkbxMystery.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetBooks().Where(b => b.BookGenre.HasFlag(Book.Genre.Mystery)).ToList());
            }
            if (chkbxRomance.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetBooks().Where(b => b.BookGenre.HasFlag(Book.Genre.Romance)).ToList());
            }
            return selectedItems;
        }
        private List<AbstractItem> SortByTitleAndPrice(string titleToFind, string tbxFromPrice, string tbxToPrice, double fromPrice, double toPrice, List<AbstractItem> selectedItems)
        {
            if (!string.IsNullOrEmpty(titleToFind))
            {
                if (!string.IsNullOrEmpty(tbxFromPrice) || !string.IsNullOrEmpty(tbxToPrice))
                {
                    if (!string.IsNullOrEmpty(tbxFromPrice) && !string.IsNullOrEmpty(tbxToPrice))
                        selectedItems = itemCollc.GetItemsByTitleAndPrice(titleToFind, fromPrice, toPrice, selectedItems);
                    else if (!string.IsNullOrEmpty(tbxFromPrice) && string.IsNullOrEmpty(tbxToPrice))
                        selectedItems = itemCollc.GetItemsByTitleAndPrice(titleToFind, fromPrice, Double.MaxValue, selectedItems);
                    else
                        selectedItems = itemCollc.GetItemsByTitleAndPrice(titleToFind, 0, toPrice, selectedItems);
                }
            }
            return selectedItems;
        }
        private List<Book> SortByTitleAndGenre(string titleToFind, List<Book> selectedBooks)
        {
            if (chkbxAdventure.IsChecked == true)
            {
                selectedBooks.AddRange(itemCollc.GetBooksByTitleAndGenre(titleToFind, Book.Genre.Adventure, selectedBooks));
            }
            if (chkbxComedy.IsChecked == true)
            {
                selectedBooks.AddRange(itemCollc.GetBooksByTitleAndGenre(titleToFind, Book.Genre.Comedy, selectedBooks));
            }
            if (chkbxFantasy.IsChecked == true)
            {
                selectedBooks.AddRange(itemCollc.GetBooksByTitleAndGenre(titleToFind, Book.Genre.Fantasy, selectedBooks));
            }
            if (chkbxHorror.IsChecked == true)
            {
                selectedBooks.AddRange(itemCollc.GetBooksByTitleAndGenre(titleToFind, Book.Genre.Horror, selectedBooks));
            }
            if (chkbxMystery.IsChecked == true)
            {
                selectedBooks.AddRange(itemCollc.GetBooksByTitleAndGenre(titleToFind, Book.Genre.Mystery, selectedBooks));
            }
            if (chkbxRomance.IsChecked == true)
            {
                selectedBooks.AddRange(itemCollc.GetBooksByTitleAndGenre(titleToFind, Book.Genre.Romance, selectedBooks));
            }
            return selectedBooks;
        }
        private List<Book> SortByPriceAndGenre(double fromPrice, double toPrice, List<Book> selectedBooks)
        {
            if (chkbxAdventure.IsChecked == true)
            {
                selectedBooks = itemCollc.GetBooksByPriceAndGenre(fromPrice, toPrice, Book.Genre.Adventure, selectedBooks);
            }
            if (chkbxComedy.IsChecked == true)
            {
                selectedBooks = itemCollc.GetBooksByPriceAndGenre(fromPrice, toPrice, Book.Genre.Comedy, selectedBooks);
            }
            if (chkbxFantasy.IsChecked == true)
            {
                selectedBooks = itemCollc.GetBooksByPriceAndGenre(fromPrice, toPrice, Book.Genre.Fantasy, selectedBooks);
            }
            if (chkbxHorror.IsChecked == true)
            {
                selectedBooks = itemCollc.GetBooksByPriceAndGenre(fromPrice, toPrice, Book.Genre.Horror, selectedBooks);
            }
            if (chkbxMystery.IsChecked == true)
            {
                selectedBooks = itemCollc.GetBooksByPriceAndGenre(fromPrice, toPrice, Book.Genre.Mystery, selectedBooks);
            }
            if (chkbxRomance.IsChecked == true)
            {
                selectedBooks = itemCollc.GetBooksByPriceAndGenre(fromPrice, toPrice, Book.Genre.Romance, selectedBooks);
            }
            return selectedBooks;
        }
        private List<AbstractItem> SortByCategory(List<AbstractItem> selectedItems)
        {
            if (chkbxScience.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetJournals().Where(j => j.JournalCategory.HasFlag(Journal.Category.Science)).ToList());
            }
            if (chkbxNature.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetJournals().Where(j => j.JournalCategory.HasFlag(Journal.Category.Nature)).ToList());
            }
            if (chkbxComputers.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetJournals().Where(j => j.JournalCategory.HasFlag(Journal.Category.Computers)).ToList());
            }
            if (chkbxGeography.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetJournals().Where(j => j.JournalCategory.HasFlag(Journal.Category.Geography)).ToList());
            }
            if (chkbxMedicine.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetJournals().Where(j => j.JournalCategory.HasFlag(Journal.Category.Medicine)).ToList());
            }
            if (chkbxCulture.IsChecked == true)
            {
                selectedItems.AddRange(itemCollc.GetJournals().Where(j => j.JournalCategory.HasFlag(Journal.Category.Culture)).ToList());
            }
            return selectedItems;
        }
        private List<Journal> SortByTitleAndCategory(string titleToFind, List<Journal> selectedJournals)
        {
            if (chkbxScience.IsChecked == true)
            {
                selectedJournals.AddRange(itemCollc.GetJournalsByTitleAndCategory(titleToFind, Journal.Category.Science, selectedJournals));
            }
            if (chkbxNature.IsChecked == true)
            {
                selectedJournals.AddRange(itemCollc.GetJournalsByTitleAndCategory(titleToFind, Journal.Category.Nature, selectedJournals));
            }
            if (chkbxComputers.IsChecked == true)
            {
                selectedJournals.AddRange(itemCollc.GetJournalsByTitleAndCategory(titleToFind, Journal.Category.Computers, selectedJournals));
            }
            if (chkbxGeography.IsChecked == true)
            {
                selectedJournals.AddRange(itemCollc.GetJournalsByTitleAndCategory(titleToFind, Journal.Category.Geography, selectedJournals));
            }
            if (chkbxMedicine.IsChecked == true)
            {
                selectedJournals.AddRange(itemCollc.GetJournalsByTitleAndCategory(titleToFind, Journal.Category.Medicine, selectedJournals));
            }
            if (chkbxCulture.IsChecked == true)
            {
                selectedJournals.AddRange(itemCollc.GetJournalsByTitleAndCategory(titleToFind, Journal.Category.Culture, selectedJournals));
            }
            return selectedJournals;
        }
        private List<Journal> SortByPriceAndCategory(double fromPrice, double toPrice, List<Journal> selectedJournals)
        {
            if (chkbxScience.IsChecked == true)
            {
                selectedJournals = itemCollc.GetJournalsByPriceAndCategory(fromPrice, toPrice, Journal.Category.Science, selectedJournals);
            }
            if (chkbxNature.IsChecked == true)
            {
                selectedJournals = itemCollc.GetJournalsByPriceAndCategory(fromPrice, toPrice, Journal.Category.Nature, selectedJournals);
            }
            if (chkbxComputers.IsChecked == true)
            {
                selectedJournals = itemCollc.GetJournalsByPriceAndCategory(fromPrice, toPrice, Journal.Category.Computers, selectedJournals);
            }
            if (chkbxGeography.IsChecked == true)
            {
                selectedJournals = itemCollc.GetJournalsByPriceAndCategory(fromPrice, toPrice, Journal.Category.Geography, selectedJournals);
            }
            if (chkbxMedicine.IsChecked == true)
            {
                selectedJournals = itemCollc.GetJournalsByPriceAndCategory(fromPrice, toPrice, Journal.Category.Medicine, selectedJournals);
            }
            if (chkbxCulture.IsChecked == true)
            {
                selectedJournals = itemCollc.GetJournalsByPriceAndCategory(fromPrice, toPrice, Journal.Category.Culture, selectedJournals);
            }
            return selectedJournals;
        }

        private void GenerateReportOfBooks(object sender, RoutedEventArgs e)
        {
            if (cmbxBookReport.SelectedItem == null)
            {
                MessageBox.Show("Please choose a book report type");
                return;
            }
            report.dgBooksReport.Visibility = Visibility.Visible;
            report.dgJournalsReport.Visibility = Visibility.Collapsed;

            List<AbstractItem> selectedItems = new List<AbstractItem>();
            List<Book> selectedBooks = new List<Book>();

            string titleToFind = tbxTitleToFind.Text;
            double.TryParse(tbxFromPrice.Text, out double fromPrice);
            double.TryParse(tbxToPrice.Text, out double toPrice);
            string reportType = cmbxBookReport.SelectedItem.ToString().Remove(0, 38);

            switch (reportType)
            {
                case "By Title":
                    selectedItems = SortByTitle(titleToFind, selectedItems);
                    break;
                case "By Price":
                    selectedItems = SortByPrice(tbxFromPrice.Text, tbxToPrice.Text, fromPrice, toPrice, selectedItems);
                    break;
                case "By Genre":
                    selectedItems = SortByGenre(selectedItems);
                    break;
                case "By Title and Price":
                    selectedItems = SortByTitleAndPrice(titleToFind, tbxFromPrice.Text, tbxToPrice.Text, fromPrice, toPrice, selectedItems);
                    break;
                case "By Title and Genre":
                    selectedBooks = SortByTitleAndGenre(titleToFind, selectedBooks);
                    break;
                case "By Price and Genre":
                    if (!string.IsNullOrEmpty(tbxFromPrice.Text) || !string.IsNullOrEmpty(tbxToPrice.Text))
                    {
                        if (!string.IsNullOrEmpty(tbxFromPrice.Text) && !string.IsNullOrEmpty(tbxToPrice.Text))
                            selectedBooks = SortByPriceAndGenre(fromPrice, toPrice, selectedBooks);
                        else if (!string.IsNullOrEmpty(tbxFromPrice.Text) && string.IsNullOrEmpty(tbxToPrice.Text))
                            selectedBooks = SortByPriceAndGenre(fromPrice, Double.MaxValue, selectedBooks);
                        else
                            selectedBooks = SortByPriceAndGenre(0, toPrice, selectedBooks);
                    }
                    break;
                default:
                    break;
            }

            if ((reportType == "By Title and Genre") || (reportType == "By Price and Genre"))
                report.SelectedBooks = selectedBooks.Distinct().ToList();
            else
                report.SelectedBooks = selectedItems.OfType<Book>().Distinct().ToList();

            report.Show();
        }
        private void GenerateReportOfJournals(object sender, RoutedEventArgs e)
        {
            if (cmbxJournalReport.SelectedItem == null)
            {
                MessageBox.Show("Please choose a journal report type");
                return;
            }
            report.dgJournalsReport.Visibility = Visibility.Visible;
            report.dgBooksReport.Visibility = Visibility.Collapsed;

            List<AbstractItem> selectedItems = new List<AbstractItem>();
            List<Journal> selectedJournals = new List<Journal>();

            string titleToFind = tbxTitleToFind.Text;
            double.TryParse(tbxFromPrice.Text, out double fromPrice);
            double.TryParse(tbxToPrice.Text, out double toPrice);
            string reportType = cmbxJournalReport.SelectedItem.ToString().Remove(0, 38);

            switch (reportType)
            {
                case "By Title":
                    selectedItems = SortByTitle(titleToFind, selectedItems);
                    break;
                case "By Price":
                    selectedItems = SortByPrice(tbxFromPrice.Text, tbxToPrice.Text, fromPrice, toPrice, selectedItems);
                    break;
                case "By Category":
                    selectedItems = SortByCategory(selectedItems);
                    break;
                case "By Title and Price":
                    selectedItems = SortByTitleAndPrice(titleToFind, tbxFromPrice.Text, tbxToPrice.Text, fromPrice, toPrice, selectedItems);
                    break;
                case "By Title and Category":
                    selectedJournals = SortByTitleAndCategory(titleToFind, selectedJournals);
                    break;
                case "By Price and Category":
                    if (!string.IsNullOrEmpty(tbxFromPrice.Text) || !string.IsNullOrEmpty(tbxToPrice.Text))
                    {
                        if (!string.IsNullOrEmpty(tbxFromPrice.Text) && !string.IsNullOrEmpty(tbxToPrice.Text))
                            selectedJournals = SortByPriceAndCategory(fromPrice, toPrice, selectedJournals);
                        else if (!string.IsNullOrEmpty(tbxFromPrice.Text) && string.IsNullOrEmpty(tbxToPrice.Text))
                            selectedJournals = SortByPriceAndCategory(fromPrice, Double.MaxValue, selectedJournals);
                        else
                            selectedJournals = SortByPriceAndCategory(0, toPrice, selectedJournals);
                    }
                    break;
                default:
                    break;
            }

            if ((reportType == "By Title and Category") || (reportType == "By Price and Category"))
                report.SelectedJournals = selectedJournals.Distinct().ToList();
            else
                report.SelectedJournals = selectedItems.OfType<Journal>().Distinct().ToList();

            report.Show();
        }
        private void ClearReportFields(object sender, RoutedEventArgs e)
        {
            tbxTitleToFind.Text = null;
            tbxFromPrice.Text = null;
            tbxToPrice.Text = null;
            chkbxAdventure.IsChecked = false;
            chkbxComedy.IsChecked = false;
            chkbxFantasy.IsChecked = false;
            chkbxHorror.IsChecked = false;
            chkbxMystery.IsChecked = false;
            chkbxRomance.IsChecked = false;
            cmbxBookReport.SelectedItem = null;
            chkbxScience.IsChecked = false;
            chkbxNature.IsChecked = false;
            chkbxComputers.IsChecked = false;
            chkbxGeography.IsChecked = false;
            chkbxMedicine.IsChecked = false;
            chkbxCulture.IsChecked = false;
            cmbxJournalReport.SelectedItem = null;
        }
        private void Add_Book_Copies(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtbxNumOfBookCopies.Text, out int numOfCopies);
            AbstractItem ai = (AbstractItem)dgBooks.SelectedItem;

            if (numOfCopies < 0)
            {
                MessageBox.Show("Please enter a positive number");
                return;
            }
            if (ai == null)
            {
                MessageBox.Show("Please choose an item from the shelf");
                return;
            }

            ai.AddCopies(numOfCopies);
            dgBooks.ItemsSource = itemCollc.GetBooks();
            txtbxNumOfBookCopies.Text = "";
            dgBooks.SelectedItem = null;
        }
        private void Add_Journal_Copies(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtbxNumOfJournalCopies.Text, out int numOfCopies);
            AbstractItem ai = (AbstractItem)dgJournals.SelectedItem;

            if (numOfCopies < 0)
            {
                MessageBox.Show("Please enter a positive number");
                return;
            }
            if (ai == null)
            {
                MessageBox.Show("Please choose an item from the shelf");
                return;
            }

            ai.AddCopies(numOfCopies);
            dgJournals.ItemsSource = itemCollc.GetJournals();
            txtbxNumOfJournalCopies.Text = "";
            dgJournals.SelectedItem = null;
        }
        private void Invoke_Indexer(object sender, RoutedEventArgs e)
        {
            string key = txtbxSearch.Text;
            if (string.IsNullOrEmpty(key))
            {
                MessageBox.Show("Please enter a parameter to search by");
                return;
            }
            try
            {
                MessageBox.Show("Item Found!\n\n" + itemCollc[key].GetDetailes());
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Item was not found");
            }

            txtbxSearch.Text = "";
        }
        private void DG_Books_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Book b = dgBooks.SelectedItem as Book;
            if (dgBooks.SelectedItem != null)
                MessageBox.Show(b.GetDetailes());
            dgBooks.SelectedItem = null;
        }
        private void DG_Journals_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Journal j = dgJournals.SelectedItem as Journal;
            if (dgJournals.SelectedItem != null)
                MessageBox.Show(j.GetDetailes());
            dgJournals.SelectedItem = null;
        }
    }
}
