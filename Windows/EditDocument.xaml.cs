using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using static MyDiplom.db.dbClass;
using MyDiplom.db;

namespace MyDiplom.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditDocument.xaml
    /// </summary>
    public partial class EditDocument : Window
    {
        int gDocumentId;
        int gUserId;
        string FileName = null;
        MainWindow gPrewWindow;
        public EditDocument(int lDocumentId, int lUserId, MainWindow lPrewWindow)
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

            CBAuthor.ItemsSource = myDB.User.ToList();

            CBType.ItemsSource = myDB.DocumentType.ToList();
            CBType.DisplayMemberPath = "Name";

            CBStatus.ItemsSource = myDB.DocumentStatus.ToList();
            CBStatus.DisplayMemberPath = "Name";

            var document = myDB.Document.Where(i => i.Id == lDocumentId).FirstOrDefault();
            TBNumberDocument.Text = document.Number;
            TBName.Text = document.Name;
            CBAuthor.SelectedIndex = document.AuthorId-1;
            CBType.SelectedIndex = document.DocumentTypeId-1;
            DPCreateDate.SelectedDate = document.CreateDate;
            CBStatus.SelectedIndex = document.DocumentStatusId-1;
            TBDescript.Text = document.Descript;
            byte[] fileAsByte = (byte[])document.File;
            if (fileAsByte != null)
            {
                FileName = fileAsByte.ToString();
                BTNLoadFileLabel.Content = FileName;
            }
            CBIsImportant.IsChecked = document.IsImportant;
            CBIsUrgent.IsChecked = document.IsUrgent;
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gPrewWindow.BackToStart();
        }

        private void BTNLoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
                BTNLoadFileLabel.Content = openFileDialog.FileName;
            }
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            var document = myDB.Document.Single(i => i.Id == gDocumentId);
            document.Number = TBNumberDocument.Text;
            document.Name = TBName.Text;
            var _author = CBAuthor.SelectedItem as User;
            document.AuthorId = _author.Id;
            document.DocumentTypeId = CBType.SelectedIndex + 1;
            document.CreateDate = DPCreateDate.SelectedDate.Value;
            document.DocumentStatusId = CBStatus.SelectedIndex + 1;
            document.Descript = TBDescript.Text;
            document.IsImportant = CBIsImportant.IsChecked.Value;
            document.IsUrgent = CBIsUrgent.IsChecked.Value;
            if(FileName != null && FileName.Length>0)
                if(File.Exists(FileName))
                    document.File = File.ReadAllBytes(FileName);
            myDB.SaveChanges();
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }
    }
}
