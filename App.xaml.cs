﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_SplashScreen
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {  
            this.InitializeComponent();
            LaunchTask();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;
        public SplashScreen m_sc;

        private async void LaunchTask()
        {
            m_sc = new SplashScreen();
            m_sc.Initialize();
            IntPtr hBitmap = await m_sc.GetBitmap(@"Assets\Butterfly_Brown.png");
            m_sc.DisplaySplash(IntPtr.Zero, hBitmap, null);
            // m_sc.DisplaySplash(IntPtr.Zero, IntPtr.Zero, @"Assets\XboxSplashScreen.mp4");
            // m_sc.DisplaySplash(IntPtr.Zero, IntPtr.Zero, @"Assets\Firework_black_background_640x400.mp4");
        }
    }
}
