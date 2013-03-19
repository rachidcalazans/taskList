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
using System.Runtime.Serialization.Json;
using TaskList.WebService;

namespace TaskList
{
    public partial class TaskGps : PhoneApplicationPage
    {
        Task task;
        GpsPoint gpsPoint;

        public TaskGps()
        {
            gpsPoint = new GpsPoint();
            InitializeComponent();
            GeoCoordinateWatcher geo = new GeoCoordinateWatcher();
            geo.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(geo_PositionChanged);
            geo.Start();
        }

        void geo_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            double latitude = e.Position.Location.Latitude;
            double longitude = e.Position.Location.Longitude;
            pgrsGps.Visibility = Visibility.Collapsed;
            if (task != null)
            {
                salvarGps(latitude.ToString(), longitude.ToString());
            }
            gpsPoint.Latitude = latitude.ToString();
            gpsPoint.Longitude = longitude.ToString();
            showAddress(gpsPoint);
            Pushpin pin = new Pushpin();
            pin.Location = e.Position.Location;
            pin.Content = "Aqui";

            mapa.Children.Clear();
            mapa.Children.Add(pin);
            mapa.Center = e.Position.Location;

        }

        private void showAddress(GpsPoint point)
        {
            string URL = "http://dev.virtualearth.net/REST/v1/Locations/" + point.Latitude + "," + point.Longitude + "?o=json&key=AkTjmP71KlW6dUMUX1h7vD98Tpn02-K36swYe1peSSuw12TkpgRG3bmU7GBT612D";
            WebClient client = new WebClient();
            client.OpenReadCompleted += client_OpenReadCompleted;
            client.OpenReadAsync(new Uri(URL, UriKind.Absolute));
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                DataContractJsonSerializer serial = new DataContractJsonSerializer(typeof(GpsLocation));
                GpsLocation gps = (GpsLocation)serial.ReadObject(e.Result);
                txtAddress.Text = gps.ResourceSets.First().Resources.First().Address;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void salvarGps(string latitude, string longitude)
        {
            try
            {
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    if (gpsPoint.TaskId != 0)
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
                    MessageBox.Show("Location finded.");
                }
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show("Ops, We just got an error.");
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
                        showAddress(gpsList.Last());
                    }
                }
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