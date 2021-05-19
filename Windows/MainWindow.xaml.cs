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
        Authorization gLoginPage;
        public MainWindow(int lUserId, Authorization lLoginPage)
        {
            gUserId = lUserId;
            gLoginPage = lLoginPage;
            InitializeComponent();
            string FirstName = db.User.Where(i => i.Id == gUserId).Select(i => i.FirstName).First();
            string MiddleName = db.User.Where(i => i.Id == gUserId).Select(i => i.MiddleName).First();
            MiddleName = MiddleName[0] + ".";
            string LastName = db.User.Where(i => i.Id == gUserId).Select(i => i.LastName).First();
            if (LastName.Length > 0)
                LastName = LastName[0] + ".";
            LBLFio.Content = $"{FirstName} {MiddleName}{LastName}";

            var count = db.Approval.Where(i => i.UserId == gUserId).Count();
            TBApprovalsCount.Content = "Согласования (" + count + ")";

            Filter();
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            BackToStart();
        }

        private void BTNContacts_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            ContactPersonal contactPersonal = new ContactPersonal(gUserId, this);
            contactPersonal.ShowDialog();
        }

        private void BTNAddNewDocument_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            AddNewDocument addNewDocument = new AddNewDocument(gUserId, this);
            addNewDocument.ShowDialog();

            Filter();
        }

        private void BTNEditDocument_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var affiliation = button.DataContext as Affiliation;

            this.Visibility = Visibility.Hidden;
            EditDocument editDocument = new EditDocument(affiliation.DocumentId, gUserId, this);
            editDocument.ShowDialog();

            Filter();
        }

        private void LVMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListView;
            var affiliation = item.SelectedItem as Affiliation;

            if (affiliation != null)
            {
                this.Visibility = Visibility.Hidden;
                AddApprovalDocument addApprovalDocument = new AddApprovalDocument(affiliation.DocumentId, gUserId, this);
                addApprovalDocument.ShowDialog();
                var count = db.Approval.Where(i => i.UserId == gUserId).Count();
                TBApprovalsCount.Content = "Согласования (" + count + ")";
                Filter();
            }
        }

        private void BTNMyApprovalsDocument_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            EditApprovalDocument editApprovalDocument = new EditApprovalDocument(gUserId, this);
            editApprovalDocument.ShowDialog();
            var count = db.Approval.Where(i => i.UserId == gUserId).Count();
            TBApprovalsCount.Content = "Согласования (" + count + ")";
            Filter();
        }

        private void TBNumberFilter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TBNumberFilter.Text.Equals("№ документа"))
                TBNumberFilter.Text = "";
        }

        private void TBStatusFilter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TBStatusFilter.Text.Equals("Статус"))
                TBStatusFilter.Text = "";
        }

        private void TBAurhorFilter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TBAurhorFilter.Text.Equals("Автор"))
                TBAurhorFilter.Text = "";
        }

        private void TBNameFilter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TBNameFilter.Text.Equals("Название документа"))
                TBNameFilter.Text = "";
        }

        public void Filter()
        {
            var list = db.Affiliation.Where(i => i.UserId == gUserId).ToList();

            if (TBNumberFilter.Text.Length > 0 && TBNumberFilter.Text.ToLower().Equals("№ документа".ToLower()) == false)
                list = list.Where(i => i.Document.Number.ToLower().Contains(TBNumberFilter.Text.ToLower())).ToList();
            if (TBStatusFilter.Text.Length > 0 && TBStatusFilter.Text.ToLower().Equals("Статус".ToLower()) == false)
                list = list.Where(i => i.Document.DocumentStatus.Name.ToLower().Contains(TBStatusFilter.Text.ToLower())).ToList();
            if (TBAurhorFilter.Text.Length > 0 && TBAurhorFilter.Text.ToLower().Equals("Автор".ToLower()) == false)
                list = list.Where(i => i.Document.User.FirstName.ToLower().Contains(TBAurhorFilter.Text.ToLower()) || i.Document.User.MiddleName.ToLower().Contains(TBAurhorFilter.Text.ToLower()) || i.Document.User.LastName.ToLower().Contains(TBAurhorFilter.Text.ToLower())).ToList();
            if (TBNameFilter.Text.Length > 0 && TBNameFilter.Text.ToLower().Equals("Название документа".ToLower()) == false)
                list = list.Where(i => i.Document.Name.ToLower().Contains(TBNameFilter.Text.ToLower())).ToList();

            LVMain.ItemsSource = list;
        }

        private void BTNClearFilter_Click(object sender, RoutedEventArgs e)
        {
            TBNumberFilter.Text = "№ документа";
            TBStatusFilter.Text = "Статус";
            TBAurhorFilter.Text = "Автор";
            TBNameFilter.Text = "Название документа";
            Filter();
        }

        private void TBNumberFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        private void TBStatusFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        private void TBAurhorFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        private void TBNameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        public void BackToStart()
        {
            gLoginPage.Visibility = Visibility.Visible;
            this.Close();
        }

        public void FullExit()
        {
            this.Close();
            gLoginPage.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(e.Cancel)
            {
                gLoginPage.Close();
            }
        }
    }
}
