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
    /// Логика взаимодействия для ContactPersonal.xaml
    /// </summary>
    public partial class ContactPersonal : Window
    {
        Authorization gOwnWindow;
        MainWindow gPrewWindow;
        public static MyDBEntities db = new MyDBEntities();
        public ContactPersonal(Authorization lOwnWindow, MainWindow lPrewWindow, int lUserId)
        {
            gOwnWindow = lOwnWindow;
            gPrewWindow = lPrewWindow;
            InitializeComponent();
            string FirstName = db.User.Where(i => i.Id == lUserId).Select(i => i.FirstName).First();
            string MiddleName = db.User.Where(i => i.Id == lUserId).Select(i => i.MiddleName).First();
            MiddleName = MiddleName[0] + ".";
            string LastName = db.User.Where(i => i.Id == lUserId).Select(i => i.LastName).First();
            if (LastName.Length > 0)
                LastName = LastName[0] + ".";
            LBLFio.Content = $"{FirstName} {MiddleName}{LastName}";

            var list = db.User.ToList();
            LVMain.ItemsSource = list;
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
