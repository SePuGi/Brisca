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
    public sealed partial class triomfVista : UserControl
    {
        public triomfVista()
        {
            this.InitializeComponent();
            imgBackTriomf.Visibility = (Visibility)1;
        }

        public Image imgTriomfUC
        {
            get { return imgTriomf; }
            set { imgTriomf = value; }
        }

        public Image imgTriomfBackUC
        {
            get { return imgBackTriomf; }
            set { imgBackTriomf = value; }
        }
    }    
}
