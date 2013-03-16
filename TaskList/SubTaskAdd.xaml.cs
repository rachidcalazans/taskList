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
        SubTask subTask;

        public SubTaskAdd()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
            {
                State["subTaskName"] = txtDescription.Text;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (State.ContainsKey("subTaskName"))
            {
                txtDescription.Text = State["subTaskName"].ToString();

            }

            string idSubTask = NavigationContext.QueryString["idSubTask"];
            
            //Caso seja uma edicao de subTask
            if (idSubTask != "")
            {
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    //Busca no BD pelo id passado por parametro e seta o objeto de classe subTask
                    subTask = banco.SubTasks.Where(o => o.Id.Equals(idSubTask)).First();
                    //Seta o txt da view
                    txtDescription.Text = subTask.Description;

                    //Verifica se o Alert esta true no BD
                    if (subTask.Alert == 1)
                    {
                        btCheck.IsChecked = true;
                    }
                }
            }

            App app = Application.Current as App;
            if (app.AuxParam != null && app.AuxParam.GetType() == typeof(Task))
            {
                task = (Task)app.AuxParam;
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtDescription.Text != "")
            {
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    int boolChecked = (Convert.ToBoolean(btCheck.IsChecked)) ? 1 : 0;
                    if (subTask != null)
                    {
                        subTask.Description = txtDescription.Text;
                        subTask.Alert = boolChecked;

                        SubTask subTaskOld = banco.SubTasks.Where(o => o.Id.Equals(subTask.Id)).First();

                        subTaskOld.Description = subTask.Description;
                        subTaskOld.Alert = subTask.Alert;
                    }
                    else
                    {
                        subTask = new SubTask()
                        {
                            Description = txtDescription.Text,
                            Status = 0,
                            Alert = boolChecked,
                            TaskId = task.Id
                        };

                        banco.SubTasks.InsertOnSubmit(subTask);
                    }
                   
                    banco.SubmitChanges();
                }
                NavigationService.GoBack();
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void btIcoDelete_Click(object sender, EventArgs e)
        {
            if (subTask != null)
            {
                if (MessageBox.Show("Deseja realmente remover?", "Confirmar", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                    {
                        subTask = banco.SubTasks.Where(o => o.Id.Equals(subTask.Id)).First();

                        banco.SubTasks.DeleteOnSubmit(subTask);
                        banco.SubmitChanges();

                        NavigationService.GoBack();
                    }
                }
            }
            else
            {
                NavigationService.GoBack();
            }
            
        }
    }
}