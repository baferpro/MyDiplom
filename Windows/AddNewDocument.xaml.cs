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
    /// Логика взаимодействия для AddNewDocument.xaml
    /// </summary>
    public partial class AddNewDocument : Window
    {
        string FileName = null;
        int gUserId;
        MainWindow gPrewWindow;
        public AddNewDocument(int lUserId, MainWindow lPrewWindow)
        {
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

            List<User> list = myDB.User.ToList();
            for(int i = 0; i < list.Count(); i++)
            {
                string FIO = list[i].FirstName + " " + list[i].MiddleName[0] + "." + list[i].LastName[0] + ".";
                CBAuthor.Items.Add(FIO);
            }
            CBAuthor.SelectedIndex = 0;
            
            CBType.ItemsSource = myDB.DocumentType.ToList();
            CBType.SelectedIndex = 0;
            CBType.DisplayMemberPath = "Name";
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gPrewWindow.BackToStart();
        }

        private void BTNLoadFile_Click(object sender, RoutedEventArgs e)
        {
            BTNLoadFile.BorderBrush = new SolidColorBrush(Color.FromRgb(7, 19, 81));
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
                BTNLoadFileLabel.Content = openFileDialog.FileName;
            }
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;
            if (TBNumberDocument.Text.Equals(""))
            {
                TBNumberDocument.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            if (TBName.Text.Equals(""))
            {
                TBName.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            if(DPCreateDate.SelectedDate == null)
            {
                DPCreateDate.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            if (FileName == null)
            {
                BTNLoadFile.BorderBrush = new SolidColorBrush(Colors.Red);
                error = true;
            }
            if (error)
                return;
            int NewDocumentId = myDB.Document.Add(new Document
            {
                Number = TBNumberDocument.Text,
                Name = TBName.Text,
                Descript = TBDescript.Text,
                AuthorId = CBAuthor.SelectedIndex+1,
                DocumentTypeId = CBType.SelectedIndex+1,
                CreateDate = DPCreateDate.SelectedDate.Value,
                DocumentStatusId = 1,
                IsImportant = CBIsImportant.IsChecked.Value,
                IsUrgent = CBIsUrgent.IsChecked.Value,
                File = File.ReadAllBytes(FileName)
            }).Id;
            myDB.Affiliation.Add(new Affiliation
            {
                UserId = gUserId,
                DocumentId = NewDocumentId
            });
            myDB.SaveChanges();
            MessageBox.Show("Новый документ был успешно добавлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void TBNumberDocument_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBNumberDocument.BorderBrush = new SolidColorBrush(Color.FromRgb(7, 19, 81));
        }

        private void TBName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TBName.BorderBrush = new SolidColorBrush(Color.FromRgb(7, 19, 81));
        }

        private void DPCreateDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DPCreateDate.BorderBrush = new SolidColorBrush(Color.FromRgb(7, 19, 81));
        }
    }
}
