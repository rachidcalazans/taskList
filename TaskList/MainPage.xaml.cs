using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using TaskList.DatabaseAccess;
using System.Globalization;
using System;

namespace TaskList
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                if (!banco.DatabaseExists())
                {
                    banco.CreateDatabase();
                }
                CarregarLista();
            }
        }

        public void CarregarLista()
        {
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                List<SubTask> subtasks = (from subtask in banco.SubTasks select subtask).ToList();
                listaTasks.ItemsSource = subtasks;
            }
        }

        private void btSalvar_Click_1(object sender, RoutedEventArgs e)
        {

            DateTimeFormatInfo dateInfoBr = new DateTimeFormatInfo();
            dateInfoBr.ShortDatePattern = "dd/MM/yyyy";

            DateTime dataInicio = Convert.ToDateTime(datInicio.Value, dateInfoBr);
            DateTime dataTermino = Convert.ToDateTime(datTermino.Value, dateInfoBr);
            
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                Task task = new Task()
                {
                    Description = txtDescricao.Text,
                    StartDate   = dataInicio.Date,
                    FinishDate  = dataTermino.Date,
                    Status      = 0
                };
                banco.Tasks.InsertOnSubmit(task);

                SubTask subtask = new SubTask()
                {
                    Description = "Super subtask",
                    Status = 0,
                    Task = task
                };
                banco.SubTasks.InsertOnSubmit(subtask);
                banco.SubmitChanges();
                CarregarLista();
            }
        }
    }
      
}