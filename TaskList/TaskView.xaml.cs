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
        Task task;
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
                task = (Task)app.AuxParam;
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
            rotateAnimation(bt);

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

            if (s.Status == 0)
            {
                bt.Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                bt.Background = new SolidColorBrush(Colors.Green);
            }

        }

        public void rotateAnimation(Button bt)
        {
            DoubleAnimation anima = new DoubleAnimation();
            anima.From = 0;
            anima.To = 180;
            anima.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            SkewTransform skew = bt.RenderTransform as SkewTransform;
            Storyboard.SetTarget(anima, skew);
            Storyboard.SetTargetProperty(anima,
            new PropertyPath(SkewTransform.AngleXProperty));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(anima);
            storyboard.Begin();
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
            App app = (App)Application.Current;

            app.AuxParam = task;

            NavigationService.Navigate(new Uri("/TaskAdd.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_3(object sender, EventArgs e)
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
    }
}