﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Padersprinter.ViewModels;
using PadersprinterService;
using PadersprinterService.Parsing;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Padersprinter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RealTimeStop : Page
    {
        public RealTimeStop()
        {
            this.InitializeComponent();
            //this.GetDummyStop();
            this.DataContext = new RealTimeStopViewModel(1001);
        }

        private async void GetDummyStop()
        {
            Stop s = (await Parser.FindStopsByNameAsync("Hauptbahnhof")).FirstOrDefault<Stop>();
            string x = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
