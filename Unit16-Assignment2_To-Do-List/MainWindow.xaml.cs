using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Unit_16_A2_To_Do_List
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<TodoListItem> TodoItems { get; set; }
        public ObservableCollection<TodoListItem> DisplayedItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            TodoItems = new ObservableCollection<TodoListItem>();
            DisplayedItems = new ObservableCollection<TodoListItem>(TodoItems);
            masterList.ItemsSource = DisplayedItems;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (masterList.SelectedItem != null)
            {
                // Editing an existing item: Only update Description and DueDate
                var selectedItem = (TodoListItem)masterList.SelectedItem;
                selectedItem.Description = descriptionField.Text;
                selectedItem.DueDate = dueDatePicker.SelectedDate ?? DateTime.Now;
                selectedItem.IsDone = isDoneCheckbox.IsChecked == true;
            }
            else
            {
                // Adding a new item: Allow setting Title, Description, and DueDate
                var newItem = new TodoListItem
                {
                    Title = titleField.Text,
                    Description = descriptionField.Text,
                    DueDate = dueDatePicker.SelectedDate ?? DateTime.Now,
                    IsDone = isDoneCheckbox.IsChecked == true
                };
                TodoItems.Add(newItem);
            }
            UpdateDisplayedItems();
            ClearDetailFields();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (masterList.SelectedItem != null)
            {
                var selectedItem = (TodoListItem)masterList.SelectedItem;
                descriptionField.Text = selectedItem.Description;
                dueDatePicker.SelectedDate = selectedItem.DueDate;
                isDoneCheckbox.IsChecked = selectedItem.IsDone;

                // Make Title and Description fields read-only
                titleField.IsReadOnly = true;
                descriptionField.IsReadOnly = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (masterList.SelectedItem != null)
            {
                TodoItems.Remove((TodoListItem)masterList.SelectedItem);
                UpdateDisplayedItems();
            }
        }

        private void MasterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (masterList.SelectedItem != null)
            {
                var selectedItem = (TodoListItem)masterList.SelectedItem;
                titleField.Text = selectedItem.Title;
                descriptionField.Text = selectedItem.Description;
                dueDatePicker.SelectedDate = selectedItem.DueDate;
                isDoneCheckbox.IsChecked = selectedItem.IsDone;

                // Make Title and Description fields read-only when selecting an item
                titleField.IsReadOnly = true;
                descriptionField.IsReadOnly = true;
            }
        }

        private void ShowAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateDisplayedItems();
        }

        private void UpdateDisplayedItems()
        {
            DisplayedItems.Clear();
            var itemsToShow = showAllCheckBox.IsChecked == true
                ? TodoItems
                : TodoItems.Where(item => !item.IsDone);
            foreach (var item in itemsToShow)
            {
                DisplayedItems.Add(item);
            }
        }

        private void ClearDetailFields()
        {
            titleField.Clear();
            descriptionField.Clear();
            dueDatePicker.SelectedDate = null;
            isDoneCheckbox.IsChecked = false;

            // Make Title and Description fields editable when adding a new task
            titleField.IsReadOnly = false;
            descriptionField.IsReadOnly = false;
            masterList.SelectedItem = null;
        }
    }

    public class TodoListItem : INotifyPropertyChanged
    {
        private string title;
        private string description;
        private DateTime dueDate;
        private bool isDone;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                OnPropertyChanged();
            }
        }

        public bool IsDone
        {
            get => isDone;
            set
            {
                isDone = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

