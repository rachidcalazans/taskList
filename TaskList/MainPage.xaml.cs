using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using TaskList.DatabaseAccess;
using System.Globalization;
using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Microsoft.Phone.Shell;

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
            //UpdateLiveTiles();
        }

       


        private void UpdateLiveTiles()
        {
            ShellTile currentTiles = ShellTile.ActiveTiles.First();
            StandardTileData tilesUpdatedData = new StandardTileData
            {
                Title = "Live Tiles ",
                Count = 10,
                BackTitle = "Back Title"
            };

            currentTiles.Update(tilesUpdatedData);
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

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock txt = (TextBlock)sender;

            Task task = (Task)txt.DataContext;

            App app = (App)Application.Current;

            app.AuxParam = task;

            NavigationService.Navigate(new Uri("/TaskView.xaml", UriKind.Relative));
        }
    }
      
}