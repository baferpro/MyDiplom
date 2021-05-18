using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyDiplom.Windows;
using MyDiplom.db;

namespace MyDiplom.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MyDBEntities db = new MyDBEntities();
        int gUserId;
        Authorization gPrewWindow;
        public MainWindow(Authorization lPrewWindow, int lUserId)
        {
            gUserId = lUserId;
            gPrewWindow = lPrewWindow;
            InitializeComponent();
            string FirstName = db.User.Where(i => i.Id == gUserId).Select(i => i.FirstName).First();
            string MiddleName = db.User.Where(i => i.Id == gUserId).Select(i => i.MiddleName).First();
            MiddleName = MiddleName[0] + ".";
            string LastName = db.User.Where(i => i.Id == gUserId).Select(i => i.LastName).First();
            if (LastName.Length > 0)
                LastName = LastName[0] + ".";
            LBLFio.Content = $"{FirstName} {MiddleName}{LastName}";

            var list = db.Affiliation.Where(i => i.UserId == gUserId).ToList();
            LVMain.ItemsSource = list;

            var count = db.Approval.Where(i => i.UserId == gUserId).Count();
            TBApprovalsCount.Content = "Согласования (" + count + ")";
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BTNContacts_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            ContactPersonal contactPersonal = new ContactPersonal(gPrewWindow, this, gUserId);
            contactPersonal.ShowDialog();
            this.Visibility = Visibility.Visible;
        }

        private void BTNAddNewDocument_Click(object sender, RoutedEventArgs e)
        {

            this.Visibility = Visibility.Collapsed;
            AddNewDocument addNewDocument = new AddNewDocument(gUserId);
            addNewDocument.ShowDialog();
            this.Visibility = Visibility.Visible;
            var list = db.Affiliation.Where(i => i.UserId == gUserId).ToList();
            LVMain.ItemsSource = list;
        }

        private void BTNEditDocument_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var affiliation = button.DataContext as Affiliation;

            this.Visibility = Visibility.Collapsed;
            EditDocument editDocument = new EditDocument(affiliation.DocumentId, gUserId);
            editDocument.ShowDialog();
            this.Visibility = Visibility.Visible;
        }

        private void LVMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListView;
            var affiliation = item.SelectedItem as Affiliation;

            this.Visibility = Visibility.Collapsed;
            AddApprovalDocument addApprovalDocument = new AddApprovalDocument(affiliation.DocumentId, gUserId);
            addApprovalDocument.ShowDialog();
            this.Visibility = Visibility.Visible;
        }

        private void BTNMyApprovalsDocument_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            EditApprovalDocument editApprovalDocument = new EditApprovalDocument(gUserId);
            editApprovalDocument.ShowDialog();
            this.Visibility = Visibility.Visible;
        }
    }
}
