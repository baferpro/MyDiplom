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
    /// Логика взаимодействия для EditApprovalDocument.xaml
    /// </summary>
    public partial class EditApprovalDocument : Window
    {
        int gUserId;
        List<Approval> needSave = new List<Approval>();
        MainWindow gPrewWindow;
        public EditApprovalDocument(int lUserId, MainWindow lPrewWindow)
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

            LVMain.ItemsSource = myDB.Approval.Where(i => i.UserId == gUserId).ToList();
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < needSave.Count; i++)
            {
                int documentId = needSave[i].DocumentId;
                myDB.Approval.Remove(needSave[i]);
                if(myDB.Approval.Where(g => g.DocumentId == documentId).Count() == 0)
                {
                    var document = myDB.Document.Where(g => g.Id == documentId).FirstOrDefault();
                    document.DocumentStatusId = 3;
                }
            }
            myDB.SaveChanges();
            MessageBox.Show("Вы успешно согласовали выбранные документы!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gPrewWindow.BackToStart();
        }

        private void CBIsChecked_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null)
                return;
            var approval = checkBox.DataContext as Approval;
            needSave.Add(approval);
        }

        private void CBIsChecked_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null)
                return;
            var approval = checkBox.DataContext as Approval;
            for (int i = 0; i < needSave.Count; i++)
                if (needSave[i] == approval)
                    needSave.RemoveAt(i);
        }
    }
}
