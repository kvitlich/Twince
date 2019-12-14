using AdminPanel.Domain;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminPanel.CreatingForms
{
    /// <summary>
    /// Interaction logic for CategoryForm.xaml
    /// </summary>
    public partial class CategoryForm : Page
    {
        private string nameTextContains = "Введите имя категории";
        private delegate void AddCategoty(Category name);
        public CategoryForm()
        {
            InitializeComponent();
            nameTextBox.Text = nameTextContains;
        }

        private void NameTextBoxGotMouseCapture(object sender, MouseEventArgs e)
        {
            if (nameTextBox.Text == nameTextContains)
            {
                nameTextBox.Clear();
            }
        }

        private void NameTextBoxPreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (nameTextBox.Text.StartsWith(" ") || nameTextBox.Text.EndsWith(" ") || nameTextBox.Text.Length == 0)
            {
                nameTextBox.Text = nameTextContains;
                MessageBox.Show("Введите корректные данные");
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text == nameTextContains)
            {
                MessageBox.Show("Введите название категории");
            }

            Category category = new Category
            {
                Name = nameTextBox.Text
            };
            AddCategoty addCategoty = new AddCategoty(AsyncAddCategory);
            addCategoty.BeginInvoke(category, null, null);
        }

        private void AsyncAddCategory(Category category)
        {
            using (var context = new ManagerDbContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
            MessageBox.Show("Категория успешно добавлена, обновите список");
        }
    }
}
