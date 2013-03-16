using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TaskList.DatabaseAccess;

namespace TaskList
{
    public partial class TaskAdd : PhoneApplicationPage
    {
        Task task;
        public TaskAdd()
        {
            InitializeComponent();
        }

        private void btSave_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtNameInput.Text.Length > 0)
            {
                task = new Task()
                {
                    Description = txtNameInput.Text,
                    Status = 0
                };
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    banco.Tasks.InsertOnSubmit(task);
                    banco.SubmitChanges();
                }
                txtNameTitle.Text = txtNameInput.Text;
                inputs.Children.Remove(txtNameInput);
                inputs.Children.Remove(btSave);
                Button addSubTask = new Button();
                inputs.Children.Add(addSubTask);
                addSubTask.Content = "Add New Sub-Task";
                addSubTask.Click += addSubTask_Click;
            }
            else
            {
                MessageBox.Show("A task must have a name!");
            }
        }

        void addSubTask_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.AuxParam = task;
            NavigationService.Navigate(new Uri("/SubTaskAdd.xaml", UriKind.Relative));
        }


        public void CarregarLista()
        {
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                List<SubTask> subTasks = (from subtask in banco.SubTasks where subtask.Task == task select subtask).ToList();
                lstResultado.ItemsSource = subTasks;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CarregarLista();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente remover?", "Confirmar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    banco.Tasks.Attach(task);
                    banco.Tasks.DeleteOnSubmit(task);
                    banco.SubmitChanges();
                    NavigationService.GoBack();
                }
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente remover?", "Confirmar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    banco.Tasks.Attach(task);
                    banco.Tasks.DeleteOnSubmit(task);
                    banco.SubmitChanges();
                    NavigationService.GoBack();
                }
            }
        }

        private void ApplicationBarIconButton_Click_3(object sender, EventArgs e)
        {

        }
    }
}