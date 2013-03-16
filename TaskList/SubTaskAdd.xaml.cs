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
        public SubTaskAdd()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App app = Application.Current as App;
            if (app.AuxParam != null && app.AuxParam.GetType() == typeof(Task))
            {
                SubTask subTask = (SubTask)app.AuxParam;
                txtDescription.Text = subTask.Description;
                if (subTask.Status == 1) {
                    btCheck.IsChecked = true;
                }
                
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            SubTask subTask = new SubTask() 
            { 
                Description = txtDescription.Text,

            };

        }


    }
}