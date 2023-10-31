using Brisca.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace Brisca.View
{
    public sealed partial class zonaDeJoc : UserControl
    {
        public zonaDeJoc()
        {
            this.InitializeComponent();

            btnStart.IsEnabledChanged += BtnStart_IsEnabledChanged;
        }

        private void BtnStart_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public Button btnStartUC
        {
            get { return btnStart; }
        }

        public Image imgCartaJugada1UC
        {
            get { return imgCartaJugada1; }
            set { imgCartaJugada1 = value; }
        }
        public Image imgCartaJugada2UC
        {
            get { return imgCartaJugada2; }
            set { imgCartaJugada2 = value; }
        }
        public Image imgCartaJugada3UC
        {
            get { return imgCartaJugada3; }
            set { imgCartaJugada3 = value; }
        }
        public Image imgCartaJugada4UC
        {
            get { return imgCartaJugada4; }
            set { imgCartaJugada4 = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
