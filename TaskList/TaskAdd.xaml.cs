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
        bool isNewPageInstance = false;
        public TaskAdd()
        {
            InitializeComponent();
            isNewPageInstance = true;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                if (task == null)
                {
                    task = new Task()
                    {
                        Description = txtNameInput.Text,
                        Status = 0
                    };
                    //task.Description = txtNameInput.Text;
                }
                //MessageBox.Show("Des. 2: " + task.Description);
                State["task"] = task;

                //State["task"] = txtNameInput.Text;
            }
        }

        private void btSave_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtNameInput.Text.Length > 0)
            {    
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    if (task != null)
                    {
                        task.Description = txtNameInput.Text;

                        Task taskOld = banco.Tasks.Where(o => o.Id.Equals(task.Id)).First();
                        taskOld.Description = task.Description;
                    }
                    else
                    {
                        task = new Task()
                        {
                            Description = txtNameInput.Text,
                            Status = 0
                        };

                        banco.Tasks.InsertOnSubmit(task);
                    }

                    banco.SubmitChanges();
                }
                txtNameTitle.Text = txtNameInput.Text;
                inputs.Children.Remove(txtNameInput);
                inputs.Children.Remove(btSave);
                addSubTasks.Visibility = System.Windows.Visibility.Visible;
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
            NavigationService.Navigate(new Uri("/SubTaskAdd.xaml?idSubTask=", UriKind.Relative));
        }


        public void CarregarLista()
        {
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                List<SubTask> subTasks = (from subtask in banco.SubTasks where subtask.TaskId == task.Id select subtask).ToList();

                lstResultado.ItemsSource = subTasks;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (isNewPageInstance)
            {
                if (State.ContainsKey("task"))
                {
                    task = (Task)State["task"];
                    MessageBox.Show("id = " + task.Description);
                    //string a = State["task"].ToString();
                    //MessageBox.Show("id = " + a);
                }
                DataContext = task;
            }

            isNewPageInstance = false;

            App app = Application.Current as App;

            if (app.AuxParam != null && app.AuxParam.GetType() == typeof(Task))
            {
                task = (Task)app.AuxParam;

                txtNameInput.Text = task.Description;
                addSubTasks.Visibility = System.Windows.Visibility.Visible;

                CarregarLista();
            }
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
                    task = banco.Tasks.Where(o => o.Id.Equals(task.Id)).First();
                    List<SubTask> subTasks = (from subtask in banco.SubTasks where subtask.TaskId == task.Id select subtask).ToList();

                    if (subTasks.Count > 0) 
                    {
                        foreach (var subTask in subTasks)
                        {
                            banco.SubTasks.DeleteOnSubmit(subTask);
                        }
                    }

                    banco.Tasks.DeleteOnSubmit(task);
                    banco.SubmitChanges();

                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
        }

        private void ApplicationBarIconButton_Click_3(object sender, EventArgs e)
        {

        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;

            SubTask subTask = (SubTask)bt.DataContext;

            NavigationService.Navigate(new Uri("/SubTaskAdd.xaml?idSubTask="+subTask.Id, UriKind.Relative));
        }

        private void btLocation_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/TaskGps.xaml", UriKind.Relative));
        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock bt = (TextBlock)sender;

            SubTask subTask = (SubTask)bt.DataContext;

            NavigationService.Navigate(new Uri("/SubTaskAdd.xaml?idSubTask=" + subTask.Id, UriKind.Relative));
        }
    }
}