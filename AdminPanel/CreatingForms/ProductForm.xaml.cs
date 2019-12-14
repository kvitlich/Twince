using AdminPanel.Domain;
using System;
using System.Collections.Generic;
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

namespace AdminPanel.CreatingForms
{
    /// <summary>
    /// Interaction logic for ProductForm.xaml
    /// </summary>
    public partial class ProductForm : Page
    {
        private delegate void AddProduct(Product name);
        private Category currentCategory = null;
        private string nameTextContains = "Имя продукта";
        private delegate void DbCategoryAction();


        public ProductForm()
        {
            InitializeComponent();
            DbCategoryAction dbCategoryAction = new DbCategoryAction(RefreshCategories);
            dbCategoryAction.BeginInvoke(null, null);
            nameTextBox.Text = nameTextContains;
        }

        private void NameTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            if (nameTextBox.Text == nameTextContains)
            {
                nameTextBox.Clear();
            }
        }

        private void NameTextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (nameTextBox.Text.StartsWith(" ") || nameTextBox.Text.EndsWith(" ") || nameTextBox.Text.Length == 0)
            {
                nameTextBox.Text = nameTextContains;
                MessageBox.Show("Введите корректные данные");
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text == nameTextContains || currentCategory == null)
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            Product product = new Product
            {
                Name = nameTextBox.Text,
                Category = currentCategory,

            };
            AddProduct addProduct = new AddProduct(AsyncAddProduct);
            addProduct.BeginInvoke(product, null, null);
        }

        private void AsyncAddProduct(Product product)
        {
            using (var context = new ManagerDbContext())
            {
                product.Category = context.Categories.Where(x => x.Id.Equals(product.Category.Id)).FirstOrDefault();
                context.Products.Add(product);
                context.SaveChanges();
            }
            MessageBox.Show("Продукт успешно добавлен, обновите список");

        }

        private void categoryComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentCategory = (Category)categoryComboBox.SelectedItem;
        }

        private void RefreshCategories()
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
            {
                using (var context = new ManagerDbContext())
                {
                    categoryComboBox.ItemsSource = context.Categories.ToList();
                }
            });
        }
    }
}
