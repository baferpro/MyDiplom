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
using MyDiplom.Windows;

namespace MyDiplom.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public static MyDBEntities db = new MyDBEntities();
        public Authorization()
        {
            InitializeComponent();
        }

        private void BTNEnter_Click(object sender, RoutedEventArgs e)
        {
            int loginsCount = db.User.Where(i => i.Login.Equals(TBLogin.Text)).Count();
            if(loginsCount == 1)
            {
                int passwordsCount = db.User.Where(i => i.Login.Equals(TBLogin.Text) && i.Password.Equals(PBPassword.Password)).Count();
                if(passwordsCount == 1)
                {
                    int UserId = db.User.Where(i => i.Login.Equals(TBLogin.Text) && i.Password.Equals(PBPassword.Password)).Select(i => i.Id).First();
                    this.Visibility = Visibility.Collapsed;
                    MainWindow mainWindow = new MainWindow(this, UserId);
                    mainWindow.ShowDialog();
                    this.Visibility = Visibility.Visible;
                }
                else
                {
                    BRDPassword.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                BRDLogin.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void BTNCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TBLogin_TextChanged(object sender, TextChangedEventArgs e)
        {

            BRDLogin.BorderBrush = new SolidColorBrush(Color.FromRgb(7, 19, 81));
        }

        private void PBPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {

            BRDPassword.BorderBrush = new SolidColorBrush(Color.FromRgb(7, 19, 81));
        }
    }
}
