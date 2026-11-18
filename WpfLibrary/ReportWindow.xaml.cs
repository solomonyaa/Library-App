using BookLib;
using System.Collections.Generic;
using System.Windows;

namespace WpfLibrary
{
    public partial class ReportWindow : Window
    {
        private List<Book> selectedBooks;
        public List<Book> SelectedBooks
        {
            get
            {
                return selectedBooks;
            }
            set
            {
                selectedBooks = value;
                dgBooksReport.ItemsSource = value;
            }
        }

        private List<Journal> selectedJournals;
        public List<Journal> SelectedJournals
        {
            get
            {
                return selectedJournals;
            }
            set
            {
                selectedJournals = value;
                dgJournalsReport.ItemsSource = value;
            }
        }
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void Btn_Close(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
