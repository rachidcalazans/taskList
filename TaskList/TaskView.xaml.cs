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
    public partial class TaskView : PhoneApplicationPage
    {
        public TaskView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App app = Application.Current as App;
            if (app.AuxParam != null && app.AuxParam.GetType() == typeof(Task))
            {
                Task task = (Task)app.AuxParam;
                txtDescription.Text = task.Description;
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    List<SubTask> subTasks = (from subtask in banco.SubTasks where subtask.TaskId == task.Id select subtask).ToList();

                    foreach (var subTask in subTasks)
                    {
                        if (subTask.Status == 0)
                        {
                            subTask.Color = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            subTask.Color = new SolidColorBrush(Colors.Green);
                        }
                    }

                    lstResultado.ItemsSource = subTasks;
                    
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;

            SubTask subTask = (SubTask)bt.DataContext;

            if (subTask.Status == 0)
            {
                subTask.Status = 1;
                bt.Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                subTask.Status = 0;
                bt.Background = new SolidColorBrush(Colors.Green);
            }

            SubTask s;
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                s = banco.SubTasks.Where(o => o.Id.Equals(subTask.Id)).First();
                s.Status = subTask.Status;

                banco.SubmitChanges();
            }

            MessageBox.Show("status 2 -> " + s.Status);
            if (s.Status == 0)
            {
                bt.Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                bt.Background = new SolidColorBrush(Colors.Green);
            }

        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;

            App app = (App)Application.Current;
            Task task = (Task)app.AuxParam;

            NavigationService.Navigate(new Uri("/TaskAdd.xaml", UriKind.Relative));
        }
    }
}