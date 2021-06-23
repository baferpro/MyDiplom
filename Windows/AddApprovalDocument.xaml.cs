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
using static MyDiplom.db.dbClass;
using MyDiplom.db;

namespace MyDiplom.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddApprovalDocument.xaml
    /// </summary>
    public partial class AddApprovalDocument : Window
    {
        int gUserId;
        int gDocumentId;
        MainWindow gPrewWindow;
        List<User> gNeedSave = new List<User>();
        public AddApprovalDocument(int lDocumentId, int lUserId, MainWindow lPrewWindow)
        {
            gDocumentId = lDocumentId;
            gUserId = lUserId;
            gPrewWindow = lPrewWindow;
            InitializeComponent();
            string FirstName = myDB.User.Where(i => i.Id == gUserId).Select(i => i.FirstName).First();
            string MiddleName = myDB.User.Where(i => i.Id == gUserId).Select(i => i.MiddleName).First();
            MiddleName = MiddleName[0] + ".";
            string LastName = myDB.User.Where(i => i.Id == gUserId).Select(i => i.LastName).First();
            if (LastName.Length > 0)
                LastName = LastName[0] + ".";
            LBLFio.Content = $"{FirstName} {MiddleName}{LastName}";

            var document = myDB.Document.Where(i => i.Id == lDocumentId).FirstOrDefault();
            TBNumber.Content = document.Number;
            TBDescript.Text = document.Descript;

            var documentType = myDB.DocumentType.Where(i => i.Id == document.DocumentTypeId).FirstOrDefault();
            TBDocumentType.Content = documentType.Name;

            var author = myDB.User.Where(i => i.Id == document.AuthorId).FirstOrDefault();
            TBAuthor.Content = author.FirstName + " " + author.MiddleName[0] + "." + author.LastName[0] + ".";

            TBCreateDate.Content = document.CreateDate.ToString();

            var status = myDB.DocumentStatus.Where(i => i.Id == document.DocumentStatusId).FirstOrDefault();
            TBStatus.Content = status.Name;

            if (document.IsImportant)
                TBIsImportant.Visibility = Visibility.Visible;
            else
                TBIsImportant.Visibility = Visibility.Hidden;

            if (document.IsUrgent)
                TBIsUrgent.Visibility = Visibility.Visible;
            else
                TBIsUrgent.Visibility = Visibility.Hidden;

            if (document.DocumentStatusId == 3)
            {
                BTNSave.Visibility = Visibility.Hidden;
                LVMain.Visibility = Visibility.Hidden;
                TBApproval.Visibility = Visibility.Hidden;
            }
            else
            {
                var list = myDB.User.ToList();
                LVMain.ItemsSource = list;
                Check();
            }
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gPrewWindow.BackToStart();
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < gNeedSave.Count; i++)
            {
                myDB.Approval.Add(new Approval
                {
                    DocumentId = gDocumentId,
                    UserId = gNeedSave[i].Id
                });
            }
            var document = myDB.Document.Where(i => i.Id == gDocumentId).FirstOrDefault();
            document.DocumentStatusId = 2;
            myDB.SaveChanges();
            MessageBox.Show("Документ успешно отправлен на согласование!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void CBIsApprovalChecked_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null)
                return;
            var user = checkBox.DataContext as User;
            gNeedSave.Add(user);
            Check();
        }

        private void CBIsApprovalChecked_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null)
                return;
            var user = checkBox.DataContext as User;
            for (int i = 0; i < gNeedSave.Count; i++)
                if (gNeedSave[i] == user)
                    gNeedSave.RemoveAt(i);
            Check();
        }

        public void Check()
        {
            if(gNeedSave.Count == 0)
            {
                BTNSave.Background = new SolidColorBrush(Color.FromRgb(229, 229, 229));
                BTNSave.IsEnabled = false;
            }
            else
            {
                BTNSave.Background = new SolidColorBrush(Color.FromRgb(7, 19, 81));
                BTNSave.IsEnabled = true;
            }
        }
    }
}
