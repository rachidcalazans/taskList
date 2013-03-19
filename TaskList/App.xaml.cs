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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using System.Windows.Threading;
using TaskList.DatabaseAccess;

namespace TaskList
{
    public partial class App : Application
    {
        public object AuxParam;
        private DispatcherTimer timer = new DispatcherTimer();
        private GeoCoordinateWatcher geo = new GeoCoordinateWatcher();

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            geo.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            GeoCoordinate coord = geo.Position.Location;
            string msgAlert = "Task has to be done at this location:";
            bool podeMandarMsg = false;
            if (coord.IsUnknown != true)
            {
                using (MyLocalDatabase banco = new MyLocalDatabase(MyLocalDatabase.ConnectionString))
                {
                    List<GpsPoint> gpsList = (from gpspoint in banco.GpsPoints select gpspoint).ToList();
                    //MessageBox.Show("geoList= " + gpsList.Count());

                    foreach (var item in gpsList)
                    {
                        double metros = coord.GetDistanceTo(new GeoCoordinate(Double.Parse(item.Latitude), Double.Parse(item.Longitude)));
                        if (metros < 40.0)
                        {
                            Task t = banco.Tasks.Where(o => o.Id.Equals(item.TaskId)).First();
                            List<SubTask> subTasks = (from subtask in banco.SubTasks where subtask.TaskId == t.Id select subtask).ToList();
                   
                            msgAlert = msgAlert + Environment.NewLine + " Task: " + t.Description;

                            foreach (var subTask in subTasks)
                            {
                                if (subTask.Alert == 1)
                                {
                                    podeMandarMsg = true;
                                    msgAlert = msgAlert + Environment.NewLine + "   SubTask => " + subTask.Description;
                                    subTask.Alert = 0;
                                    banco.SubmitChanges();
                                }
                            }

                        }
                    }

                }
            }
            if (podeMandarMsg)
            {
                MessageBox.Show(msgAlert);
            }
            podeMandarMsg = false;
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
           

            if (ShellTile.ActiveTiles.Count() <= 0)
            {
                StandardTileData data = new StandardTileData()
                {
                    Title = "TaskList",
                    BackgroundImage = new Uri("background.png", UriKind.Relative)
                };

                ShellTile.Create(new Uri("/MainPage.xaml", UriKind.Relative), data);
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            timer.Start();

            geo.Start();

        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            timer.Start();

            geo.Start();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}