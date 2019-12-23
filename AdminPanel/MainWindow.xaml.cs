using AdminPanel.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;

namespace AdminPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    
        public MainWindow()
        {
            InitializeComponent();
            InitDb();
            SignIn signIn = new SignIn();
            signIn.Activate();
            signIn.Show();
            signIn.Closed += SignInAccept;
            RefreshCategories();
            mainGrid.Visibility = Visibility.Hidden;
        }

        private void SignInAccept(object sender, EventArgs e)
        {
            mainGrid.Visibility = Visibility.Visible;
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            if (categoriesComboBox.SelectedItem == null)
                mainDataFrame.Source = new Uri("CreatingForms/CategoryForm.xaml", UriKind.Relative);
            else
                mainDataFrame.Source = new Uri("CreatingForms/ProductForm.xaml", UriKind.Relative);
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            RefreshCategories();
            if (categoriesComboBox.SelectedItem != null)
            {
                RefreshProducts((Category)categoriesComboBox.SelectedItem);
            }
        }

        private async void RefreshProducts(Category category)
        {
                using (var context = new ManagerDbContext())
                {
                    productsComboBox.ItemsSource = await context.Products.Where(x => x.Category.Id.Equals(category.Id)).ToListAsync();
                }
        }

        private async void InitDb()
        {
            using (var context = new ManagerDbContext())
            {
                var user = await context.Users.Where(x => x.Name == "Urencul").FirstOrDefaultAsync();
                if (user == null)
                {
                    var newUser = new User
                    {
                        Name = "Urencul",
                        Password = "12345678",
                    };
                    context.Users.Add(newUser);
                }
                await context.SaveChangesAsync();
            }
        }

        private async void RefreshCategories()
        {
            using (var context = new ManagerDbContext())
             {
               categoriesComboBox.ItemsSource = await context.Categories.ToListAsync();
             
            }
        }

        private void CategoriesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoriesComboBox.SelectedItem != null)
            {
                RefreshProducts((Category)categoriesComboBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("Выберите категорию");
            }
        }
    }
}
