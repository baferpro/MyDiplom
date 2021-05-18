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
        public AddApprovalDocument(int lDocumentId, int lUserId)
        {
            gUserId = lUserId;
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
        }

        private void BTNAddApprovaler_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
