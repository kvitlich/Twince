using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace AdminPanel
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            if ((passwordTextBox.Text.StartsWith(" ") && passwordTextBox.Text.EndsWith(" ")) || (loginTextBox.Text.StartsWith(" ") && loginTextBox.Text.EndsWith(" ")))
            {
                MessageBox.Show("Поля не должны быть пустыми");
                return;
            }
            using (var context = new ManagerDbContext())
            {
                var user = await context.Users.Where(x => x.Name.Equals(loginTextBox.Text)).FirstOrDefaultAsync();
                if (user == null)
                {
                    MessageBox.Show("Введите верный логин");
                    return;
                }
                if (user.Password.Equals(passwordTextBox.Text))
                {                   
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Введен неверный пароль");
                }
            }
        }
    }
}
