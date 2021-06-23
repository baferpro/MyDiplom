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
    /// Логика взаимодействия для ContactPersonal.xaml
    /// </summary>
    public partial class ContactPersonal : Window
    {
        MainWindow gPrewWindow;
        public ContactPersonal(int lUserId, MainWindow lPrewWindow)
        {
            gPrewWindow = lPrewWindow;
            InitializeComponent();
            string FirstName = myDB.User.Where(i => i.Id == lUserId).Select(i => i.FirstName).First();
            string MiddleName = myDB.User.Where(i => i.Id == lUserId).Select(i => i.MiddleName).First();
            MiddleName = MiddleName[0] + ".";
            string LastName = myDB.User.Where(i => i.Id == lUserId).Select(i => i.LastName).First();
            if (LastName.Length > 0)
                LastName = LastName[0] + ".";
            LBLFio.Content = $"{FirstName} {MiddleName}{LastName}";

            Filter();
        }

        private void BTNExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            gPrewWindow.BackToStart();
        }

        private void BTNBack_Click(object sender, RoutedEventArgs e)
        {
            gPrewWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void BTNClearFilter_Click(object sender, RoutedEventArgs e)
        {
            TBPhoneFilter.Text = "Телефон";
            TBPostFilter.Text = "Должность";
            TBFioFilter.Text = "ФИО";
            Filter();
        }

        private void TBPhoneFilter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TBPhoneFilter.Text.Equals("Телефон"))
                TBPhoneFilter.Text = "";
        }

        private void TBPostFilter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TBPostFilter.Text.Equals("Должность"))
                TBPostFilter.Text = "";
        }

        private void TBFioFilter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TBFioFilter.Text.Equals("ФИО"))
                TBFioFilter.Text = "";
        }

        private void TBFioFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        private void TBPostFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }

        private void TBPhoneFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                Filter();
            }
        }
        public void Filter()
        {
            var list = myDB.User.ToList();

            if (TBFioFilter.Text.Length > 0 && TBFioFilter.Text.ToLower().Equals("ФИО".ToLower()) == false)
                list = list.Where(i => i.FirstName.ToLower().Contains(TBFioFilter.Text.ToLower()) || i.MiddleName.ToLower().Contains(TBFioFilter.Text.ToLower()) || i.LastName.ToLower().Contains(TBFioFilter.Text.ToLower())).ToList();
            if (TBPostFilter.Text.Length > 0 && TBPostFilter.Text.ToLower().Equals("Должность".ToLower()) == false)
                list = list.Where(i => i.Post.Name.ToLower().Contains(TBPostFilter.Text.ToLower())).ToList();
            if (TBPhoneFilter.Text.Length > 0 && TBPhoneFilter.Text.ToLower().Equals("Телефон".ToLower()) == false)
                list = list.Where(i => i.Phone.ToLower().Contains(TBPhoneFilter.Text.ToLower())).ToList();

            LVMain.ItemsSource = list;
        }
    }
}
