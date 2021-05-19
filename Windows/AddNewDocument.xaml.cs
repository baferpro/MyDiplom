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
using MyDiplom.db;

namespace MyDiplom.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddNewDocument.xaml
    /// </summary>
    public partial class AddNewDocument : Window
    {
        public static MyDBEntities db = new MyDBEntities();
        string FileName = null;
        int gUserId;
        MainWindow gPrewWindow;
        public AddNewDocument(int lUserId, MainWindow lPrewWindow)
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

            CBAuthor.ItemsSource = db.User.ToList();
            CBAuthor.SelectedIndex = 0;
            foreach (User user in CBAuthor.Items)
            {
                user.FirstName = user.FirstName + " " + user.MiddleName[0] + "." + user.LastName[0] + ".";
            }
            CBAuthor.DisplayMemberPath = "FirstName";
            
            CBType.ItemsSource = db.DocumentType.ToList();
            CBType.SelectedIndex = 0;
            CBType.DisplayMemberPath = "Name";

            CBStatus.ItemsSource = db.DocumentStatus.ToList();
            CBStatus.SelectedIndex = 0;
            CBStatus.DisplayMemberPath = "Name";
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
            int NewDocumentId = db.Document.Add(new Document
            {
                Number = TBNumberDocument.Text,
                Name = TBName.Text,
                Descript = TBDescript.Text,
                AuthorId = CBAuthor.SelectedIndex+1,
                DocumentTypeId = CBType.SelectedIndex+1,
                CreateDate = DPCreateDate.SelectedDate.Value,
                DocumentStatusId = CBStatus.SelectedIndex+1,
                IsImportant = CBIsImportant.IsChecked.Value,
                IsUrgent = CBIsUrgent.IsChecked.Value,
                File = File.ReadAllBytes(FileName)
            }).Id;
            db.Affiliation.Add(new Affiliation
            {
                UserId = gUserId,
                DocumentId = NewDocumentId
            });
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
