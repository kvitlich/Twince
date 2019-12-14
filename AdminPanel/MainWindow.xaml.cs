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

namespace AdminPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void DbProductAction(Category categoty);
        private delegate void DbCategoryAction();
    
        public MainWindow()
        {
            InitializeComponent();
            DbCategoryAction dbCategoryAction = new DbCategoryAction(RefreshCategories);
            dbCategoryAction.BeginInvoke(null, null);
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
            DbCategoryAction dbCategoryAction = new DbCategoryAction(RefreshCategories);
            dbCategoryAction.BeginInvoke(null, null);
            if (categoriesComboBox.SelectedItem != null)
            {
                DbProductAction dbProductAction = new DbProductAction(RefreshProducts);
                dbProductAction.BeginInvoke((Category)categoriesComboBox.SelectedItem, null, null);
            }
        }

        private void RefreshProducts(Category category)
        {

            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
            {
                using (var context = new ManagerDbContext())
                {
                    productsComboBox.ItemsSource = context.Products.Where(x => x.Category.Id.Equals(category.Id)).ToList();
                }

            });
        }

        private void RefreshCategories()
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate
            {
                using (var context = new ManagerDbContext())
                {
                    categoriesComboBox.ItemsSource = context.Categories.ToList();
                }
            });
        }

        private void CategoriesComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoriesComboBox.SelectedItem != null)
            {
                DbProductAction dbProductAction = new DbProductAction(RefreshProducts);
                dbProductAction.BeginInvoke((Category)categoriesComboBox.SelectedItem, null, null);
            }
            else
            {
                MessageBox.Show("Выберите категорию");
            }
        }
    }
}
