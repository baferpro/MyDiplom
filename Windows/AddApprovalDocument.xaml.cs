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
using MyDiplom.db;

namespace MyDiplom.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddApprovalDocument.xaml
    /// </summary>
    public partial class AddApprovalDocument : Window
    {
        public static MyDBEntities db = new MyDBEntities();
        int gUserId;
        int gDocumentId;
        MainWindow gPrewWindow;
        public AddApprovalDocument(int lDocumentId, int lUserId, MainWindow lPrewWindow)
        {
            gDocumentId = lDocumentId;
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

            var document = db.Document.Where(i => i.Id == lDocumentId).FirstOrDefault();
            TBNumber.Content = document.Number;
            TBDescript.Text = document.Descript;

            var documentType = db.DocumentType.Where(i => i.Id == document.DocumentTypeId).FirstOrDefault();
            TBDocumentType.Content = documentType.Name;

            var author = db.User.Where(i => i.Id == document.AuthorId).FirstOrDefault();
            TBAuthor.Content = author.FirstName + " " + author.MiddleName[0] + "." + author.LastName[0] + ".";

            TBCreateDate.Content = document.CreateDate.ToString();

            var status = db.DocumentStatus.Where(i => i.Id == document.DocumentStatusId).FirstOrDefault();
            TBStatus.Content = status.Name;

            if (document.IsImportant)
                TBIsImportant.Visibility = Visibility.Visible;
            else
                TBIsImportant.Visibility = Visibility.Hidden;

            if (document.IsUrgent)
                TBIsUrgent.Visibility = Visibility.Visible;
            else
                TBIsUrgent.Visibility = Visibility.Hidden;

            CBUsers.ItemsSource = db.User.ToList();
            CBUsers.SelectedIndex = 0;
            foreach (User user in CBUsers.Items)
            {
                user.FirstName = user.FirstName + " " + user.MiddleName[0] + "." + user.LastName[0] + ".";
            }
            CBUsers.DisplayMemberPath = "FirstName";

            if (document.DocumentStatusId == 3)
            {
                BTNSave.Background = new SolidColorBrush(Color.FromRgb(229, 229, 229));
                BTNSave.IsEnabled = false;
                BTNSave.Foreground = new SolidColorBrush(Color.FromRgb(7, 19, 81));
            }
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gPrewWindow.BackToStart();
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            db.Approval.Add(new Approval
            {
                DocumentId = gDocumentId,
                UserId = CBUsers.SelectedIndex + 1
            });
            var document = db.Document.Where(i => i.Id == gDocumentId).FirstOrDefault();
            document.DocumentStatusId = 2;
            db.SaveChanges();
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e.Cancel)
            {
                gPrewWindow.FullExit();
            }
        }
    }
}
