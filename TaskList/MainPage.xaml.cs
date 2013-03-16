using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using TaskList.DatabaseAccess;
using System.Globalization;
using System;
using System.Windows.Controls;

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
            }
            CarregarLista();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CarregarLista();
        }

        public void CarregarLista()
        {
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                List<Task> tasks = (from task in banco.Tasks select task).ToList();
                lstResultado.ItemsSource = tasks;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
           
            Task task = (Task)bt.DataContext; 

            App app = (App)Application.Current;

            app.AuxParam = task;

            NavigationService.Navigate(new Uri("/TaskView.xaml", UriKind.Relative));
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.AuxParam = null;

            NavigationService.Navigate(new Uri("/TaskAdd.xaml", UriKind.Relative));

        }
    }
      
}