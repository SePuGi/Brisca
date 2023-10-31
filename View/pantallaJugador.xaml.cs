using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class pantallaJugador : UserControl
    {


        public pantallaJugador()
        {
            this.InitializeComponent();
            img1BackUC.Visibility = Visibility.Collapsed;
            img2BackUC.Visibility = Visibility.Collapsed;
            img3BackUC.Visibility = Visibility.Collapsed;
        }



        public TextBox txbJugadorUC
        {
            get { return txbJugador; }
            set { txbJugador = value;}
        }

        public Image img1UC
        {   
            get { return img1; } 
            set {  img1 = value;}
        }
        public Image img2UC
        {
            get { return img2; }
            set { img2 = value; }
        }
        public Image img3UC
        {
            get { return img3; }
            set { img3 = value; }
        }

        public Image img1BackUC
        {
            get { return img1Back; }
            set { img1Back = value; }
        }
        public Image img2BackUC
        {
            get { return img2Back; }
            set { img2Back = value; }
        }
        public Image img3BackUC
        {
            get { return img3Back; }
            set { img3Back = value; }
        }

    }
}
