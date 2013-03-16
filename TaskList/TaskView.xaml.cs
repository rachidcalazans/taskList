﻿using System;
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
            CarregarSubTask();
        }

        public void CarregarSubTask()
        {
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                List<SubTask> subtasks = (from subtask in banco.SubTasks select subtask).ToList();
                lstResultado.ItemsSource = subtasks;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App app = Application.Current as App;
            if (app.AuxParam != null && app.AuxParam.GetType() == typeof(Task))
            {
                Task task = (Task)app.AuxParam;
                
                lstResultado.ItemsSource = task.SubTasks;
            }
        }
    }
}