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
    public partial class SubTaskAdd : PhoneApplicationPage
    {
        Task task;
        public SubTaskAdd()
        {
            InitializeComponent();
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

            App app = Application.Current as App;
            if (app.AuxParam != null && app.AuxParam.GetType() == typeof(Task))
            {
                task = (Task)app.AuxParam;
                CarregarLista();
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtDescription.Text != "")
            {
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    int boolChecked = (Convert.ToBoolean(btCheck.IsChecked)) ? 1 : 0;
                    SubTask subTask = new SubTask()
                    {
                        Description = txtDescription.Text,
                        Status = 0,
                        Alert = boolChecked,
                        TaskId = task.Id
                    };
                    banco.SubTasks.InsertOnSubmit(subTask);
                    banco.SubmitChanges();
                }
                txtDescription.Text = "";
                btCheck.IsChecked = false;
                CarregarLista();
            }
        }

        private void btRemove_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente remover?", "Confirmar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    Button bt = (Button)sender;
                    SubTask subtask = (SubTask) bt.DataContext;
                    banco.SubTasks.Attach(subtask);
                    banco.SubTasks.DeleteOnSubmit(subtask);
                    banco.SubmitChanges();
                }
                CarregarLista();
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}