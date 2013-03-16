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

namespace TaskList
{
    public partial class TaskGps : PhoneApplicationPage
    {
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

            txtLat.Text = "Lat.: " + latitude;
            txtLong.Text ="Long.: " + longitude;

            Pushpin pin = new Pushpin();
            pin.Location = e.Position.Location;
            pin.Content = "Aqui";

            mapa.Children.Clear();
            mapa.Children.Add(pin);
            mapa.Center = e.Position.Location;
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}