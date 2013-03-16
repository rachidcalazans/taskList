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
using System.Device.Location;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using TaskList.DatabaseAccess;

namespace TaskList
{
    public partial class TaskGps : PhoneApplicationPage
    {
        Task task;
        GpsPoint gpsPoint;

        public TaskGps()
        {
            InitializeComponent();
            GeoCoordinateWatcher geo = new GeoCoordinateWatcher();
            geo.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(geo_PositionChanged);
            geo.Start();
        }

        void geo_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            double latitude = e.Position.Location.Latitude;
            double longitude = e.Position.Location.Longitude;

            salvarGps(latitude.ToString(), longitude.ToString());

            txtLat.Text = "Lat.: " + latitude;
            txtLong.Text ="Long.: " + longitude;

            Pushpin pin = new Pushpin();
            pin.Location = e.Position.Location;
            pin.Content = "Aqui";

            mapa.Children.Clear();
            mapa.Children.Add(pin);
            mapa.Center = e.Position.Location;

            MessageBox.Show("Location saved with sucess.");
        }

        private void salvarGps(string latitude, string longitude)
        {
            using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
            {
                 if (gpsPoint != null)
                    {
                        gpsPoint.Latitude = latitude;
                        gpsPoint.Longitude = longitude;

                        GpsPoint gpsPointOld = banco.GpsPoints.Where(o => o.Id.Equals(gpsPoint.Id)).First();

                        gpsPointOld.Latitude = gpsPoint.Latitude;
                        gpsPointOld.Longitude = gpsPoint.Longitude;
                    }
                    else
                    {
                        gpsPoint = new GpsPoint()
                        {
                           Latitude = latitude,
                           Longitude = longitude,
                           TaskId = task.Id
                        };

                        banco.GpsPoints.InsertOnSubmit(gpsPoint);
                    }
                   
                    banco.SubmitChanges();
                
            }

           
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App app = Application.Current as App;
            if (app.AuxParam != null && app.AuxParam.GetType() == typeof(Task))
            {
                task = (Task)app.AuxParam;
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    List<GpsPoint> gpsList = (from gpspoint in banco.GpsPoints where gpspoint.TaskId == task.Id select gpspoint).ToList();

                    if (gpsList.Count > 0)
                    {
                        txtLat.Text = "Lat.: " + gpsList[0].Latitude;
                        txtLong.Text = "Long.: " + gpsList[0].Longitude;
                    }
                }
            }
        }
    }
}