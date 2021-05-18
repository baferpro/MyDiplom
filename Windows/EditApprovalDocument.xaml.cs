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
    /// Логика взаимодействия для EditApprovalDocument.xaml
    /// </summary>
    public partial class EditApprovalDocument : Window
    {
        public static MyDBEntities db = new MyDBEntities();
        int gUserId;
        List<Approval> needSave;
        public EditApprovalDocument(int lUserId)
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

            LVMain.ItemsSource = db.Approval.Where(i => i.UserId == gUserId).ToList();
        }

        private void BTNSave_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < needSave.Count; i++)
            {
                var document = db.Document.Where(g => g.Id == needSave[i].DocumentId).FirstOrDefault();
                document.DocumentStatusId = 3;
                db.Approval.Remove(needSave[i]);
                db.SaveChanges();
            }
                
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
