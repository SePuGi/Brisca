using Brisca.Model;
using Brisca.View;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace Brisca
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const int MAX_CARTES_JUGADOR = 3;
        const int MAX_CARTES_TOTAL = 48;

        const String CARTA_GIRADA = "ms-appx:///Assets/Images/back.png";

        bool joc_iniciat = false;
        List<Carta> cartesJugables = new List<Carta>();
        List<Carta> cartesJugades = new List<Carta>();
        List<Jugador> jugadors = new List<Jugador>();

        //triomf (tota la partida gira entorn aquest pal)
        int idxTriomf = 0;
        Carta triomf;
        triomfVista ptriomf;

        List<Carta> trobarGuanyador = new List<Carta>();

        zonaDeJoc zonaj;
        List<pantallaJugador> pJugador;
        public MainPage()
        {
            this.InitializeComponent();
            String nomCarta = "";
            String rutaCarta = "";
            Image im;

            zonaj = vZonaJoc;
            pJugador = new List<pantallaJugador>();
            int cont = 0;
            //generem la llista de cartes
            for (int i = 1; i < (MAX_CARTES_TOTAL/4) + 1; i++)
            {
                nomCarta = "b" + i;
                rutaCarta = "ms-appx:///Assets/Images/" + nomCarta + ".png";
                cartesJugables.Add(new Carta(cont,nomCarta, Pal.bastos,i, rutaCarta));
                cont++;

                nomCarta = "c" + i;
                rutaCarta = "ms-appx:///Assets/Images/" + nomCarta + ".png";
                cartesJugables.Add(new Carta(cont,nomCarta, Pal.copas, i, rutaCarta));
                cont++;

                nomCarta = "e" + i;
                rutaCarta = "ms-appx:///Assets/Images/" + nomCarta + ".png";
                cartesJugables.Add(new Carta(cont,nomCarta, Pal.espadas, i, rutaCarta));
                cont++;

                nomCarta = "o" + i;
                rutaCarta = "ms-appx:///Assets/Images/" + nomCarta + ".png";
                cartesJugables.Add(new Carta(cont,nomCarta, Pal.oros, i, rutaCarta));
                cont++;
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {



            zonaj.btnStartUC.Click += Button_Click;
            ptriomf = pTriomf;

            pJugador.Add(pJugador1);
            pJugador.Add(pJugador2);
            pJugador.Add(pJugador3);
            pJugador.Add(pJugador4);

            pJugador[0].txbJugadorUC.IsEnabledChanged += CargarPartida;
        }

        private void CargarPartida(object sender, DependencyPropertyChangedEventArgs e)
        {
            //carregar imatges per usuari 
            /* 
             *  List<Jugador> jugadors  (imagenes)
             * 
             * 0-2   --> Jugador 1
             * 3-5   --> Jugador 2
             * 6-8   --> Jugador 3
             * 9-11  --> Jugador 4
             */
            
            Random r = new Random();
            int idxCarta = 0; ;

            //seleccionem el triomf
            idxCarta = r.Next(cartesJugables.Count);

            for (int i = 0; i < pJugador.Count; i++)
            {
                jugadors.Add(new Jugador(pJugador[i].txbJugadorUC.Text, false));
                for (int j = 0; j < MAX_CARTES_JUGADOR; j++)
                {
                    idxCarta = r.Next(cartesJugables.Count);
                    //AFEGIM LES CARTES (MAXIM 3) A CADA USUARI
                    //debugWindows.Text += cartesJugables.Count.ToString() + "\n";
                    jugadors[i].Carta_jugador.Add(cartesJugables[idxCarta]);
                    cartesJugades.Add(cartesJugables[idxCarta]);
                    cartesJugables.Remove(cartesJugables[idxCarta]);
                }
            }

            int jugadorInici = r.Next(jugadors.Count);
            jugadors[jugadorInici].Torn = true;


            for (int i = 0; i < pJugador.Count; i++)
            {
                //provisional
                pJugador[i].txbJugadorUC.Text = "Jugador " + (i + 1);

                pJugador[i].img1UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[0].RutaCarta));
                pJugador[i].img1UC.Tapped += Im_Tapped;
                pJugador[i].img1UC.Tag = jugadors[i].Carta_jugador[0].Id;


                pJugador[i].img2UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[1].RutaCarta));
                pJugador[i].img2UC.Tapped += Im_Tapped;
                pJugador[i].img2UC.Tag = jugadors[i].Carta_jugador[1].Id;


                pJugador[i].img3UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[2].RutaCarta));
                pJugador[i].img3UC.Tapped += Im_Tapped;
                pJugador[i].img3UC.Tag = jugadors[i].Carta_jugador[2].Id;

                if (!jugadors[i].Torn)
                {
                    //tapem nomes les cartes exitents
                    pJugador[i].img1BackUC.Visibility = Visibility.Visible;
                    pJugador[i].img2BackUC.Visibility = Visibility.Visible;
                    pJugador[i].img3BackUC.Visibility = Visibility.Visible;
                    
                }

            }

            ptriomf.imgTriomfBackUC.Visibility = 0;
            triomf = cartesJugables[r.Next(cartesJugables.Count)];
            ptriomf.imgTriomfUC.Source = new BitmapImage(new Uri(triomf.RutaCarta));

            cartesJugades.Add(triomf);
            cartesJugables.Remove(cartesJugables[idxTriomf]);

        }

        int volta = 0;
        private void controladorTorn()
        {
            for (int i = 0; i < pJugador.Count; i++)
            {
                //provisional
                if (!jugadors[i].Torn)
                {
                    //No es el seu torn

                    //tapem nomes les cartes exitents

                    if(pJugador[i].img1UC.Source == null)
                    {
                        pJugador[i].img1BackUC.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        pJugador[i].img1BackUC.Visibility = Visibility.Visible;
                    }

                    if (pJugador[i].img2UC.Source == null)
                    {
                        pJugador[i].img2BackUC.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        pJugador[i].img2BackUC.Visibility = Visibility.Visible;
                    }

                    if (pJugador[i].img3UC.Source == null)
                    {
                        pJugador[i].img3BackUC.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        pJugador[i].img3BackUC.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    //torn

                    debugWindows.Text += "Torn: " + jugadors[i].Nom.ToString() + "  -Volta: " + volta + "  -Cartes restants: " + cartesJugables.Count +"\n";
                    pJugador[i].img1BackUC.Visibility = Visibility.Collapsed;
                    pJugador[i].img2BackUC.Visibility = Visibility.Collapsed;
                    pJugador[i].img3BackUC.Visibility = Visibility.Collapsed;
                }
            }
           
            volta++;
            if (volta == 4)
            {
                volta = 0;
                zonaj.imgCartaJugada1UC.Source = null;
                zonaj.imgCartaJugada2UC.Source = null;
                zonaj.imgCartaJugada3UC.Source = null;
                zonaj.imgCartaJugada4UC.Source = null;

                for (int i = 0; i < pJugador.Count; i++)
                {
                    if (pJugador[i].img1UC == null)
                    {
                        pJugador[i].img1UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[0].RutaCarta));
                    }
                    if (pJugador[i].img2UC == null)
                    {
                        pJugador[i].img2UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[1].RutaCarta));
                    }
                    if (pJugador[i].img3UC == null)
                    {
                        pJugador[i].img3UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[2].RutaCarta));
                    }
                }
                //Informació de victoria per al jugador (imprimir un cop amb el nom del guanyador)
                //debugWindows.Text += "Guanya ~NomJugador~ amb ~puntuació~" + "\n" + "Triomf: " + triomf.Nom_carta;

                debugWindows.Text += "TOTAL CARTES JUGADES: " + trobarGuanyador.Count.ToString() + "\n";

                for (int i = 0; i < trobarGuanyador.Count; i++)
                {
                    if (trobarGuanyador[i].Pal_carta.Equals(triomf.Pal_carta))
                    {
                        switch (trobarGuanyador[i].Num_carta)
                        {
                            case 1:
                                //11
                                jugadors[i].Puntuacio += 11;
                                break;
                            case 3:
                                //10
                                jugadors[i].Puntuacio += 10;
                                break;
                            case 12:
                                //4
                                jugadors[i].Puntuacio += 4;
                                break;
                            case 11:
                                //3
                                jugadors[i].Puntuacio += 3;
                                break;
                            case 10:
                                //2
                                jugadors[i].Puntuacio += 2;
                                break;
                        }

                        debugWindows.Text += "Guanyador jugador : " + i + "\n" + "Punts: " + jugadors[i].Puntuacio;
                        break;
                    }
                }
                trobarGuanyador.Clear();

                //GUANYADOR DE LA PARTIDA
                //if(cartesJugades.Count == 48)
                if (jugadors[0].Carta_jugador.Count == 0 && jugadors[1].Carta_jugador.Count == 0 && jugadors[2].Carta_jugador.Count == 0 && jugadors[3].Carta_jugador.Count == 0)
                {
                    jugadors.Sort();
                    debugWindows.Text += "FI PARTIDA";
                    debugWindows.Text += "Guanyador: " + jugadors[0].Nom;
                }
                
            }

            for (int i = 0; i < jugadors.Count; i++)
            {
                if(volta==3 && jugadors[i].Carta_jugador.Count == 0)
                {
                    //el jugador que es queda sense carta al final de joc es juga el triomf
                    jugadors[i].Carta_jugador.Add(triomf);
                    pJugador[i].img1UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[0].RutaCarta));
                    pJugador[i].img1UC.Tag = jugadors[i].Carta_jugador[0].Id;
                    break;
                }
            }

        }

        public void Im_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //funcio per controlar carta seleccionada del usuari
            //(primer comprobar si es el torn del usuari, si ho es la carta va al centre,
            //si no, no fa res (només té que deixar al usuari amb torn true))
            Image im = sender as Image;
            Random r = new Random();
            int idxRNG = 0;

            bool trobat = false;

            for (int i = 0; i < cartesJugades.Count; i++)
            {
                /**
                 * Atributs necesaris per calcular qui guanya i puntuacio 
                 */

                if (cartesJugades[i].Id.Equals(im.Tag) && volta == 0)
                {
                    zonaj.imgCartaJugada1UC.Source = new BitmapImage(new Uri(cartesJugades[i].RutaCarta));
                    debugWindows.Text += cartesJugades[i].Num_carta + "\n";
                    debugWindows.Text += cartesJugades[i].Pal_carta + "\n";
                    trobarGuanyador.Add(cartesJugades[i]);
                }
                if (cartesJugades[i].Id.Equals(im.Tag) && volta == 1)
                {
                    zonaj.imgCartaJugada2UC.Source = new BitmapImage(new Uri(cartesJugades[i].RutaCarta));
                    debugWindows.Text += cartesJugades[i].Num_carta + "\n";
                    debugWindows.Text += cartesJugades[i].Pal_carta + "\n";
                    trobarGuanyador.Add(cartesJugades[i]);
                }
                if (cartesJugades[i].Id.Equals(im.Tag) && volta == 2)
                {
                    zonaj.imgCartaJugada3UC.Source = new BitmapImage(new Uri(cartesJugades[i].RutaCarta));
                    debugWindows.Text += cartesJugades[i].Num_carta + "\n";
                    debugWindows.Text += cartesJugades[i].Pal_carta + "\n";
                    trobarGuanyador.Add(cartesJugades[i]);
                }
                if (cartesJugades[i].Id.Equals(im.Tag) && volta == 3)
                {
                    zonaj.imgCartaJugada4UC.Source = new BitmapImage(new Uri(cartesJugades[i].RutaCarta));
                    debugWindows.Text += cartesJugades[i].Num_carta + "\n";
                    debugWindows.Text += cartesJugades[i].Pal_carta + "\n";
                    trobarGuanyador.Add(cartesJugades[i]);
                }
            }

            for (int i = 0; i < jugadors.Count && !trobat; i++)
            {
                //comprobem el torn del jugador i el pasem al següent
                if (jugadors[i].Torn)
                {
                    //treure la carta del jugador y pasarali una altre

                    debugWindows.Text += "ImTag: " + im.Tag + "\n";

                    if (pJugador[i].img1UC.Tag.Equals(im.Tag))
                    {
                        pJugador[i].img1UC.Source = null;
                        if (cartesJugables.Count != 0)
                        {
                            idxRNG = r.Next(cartesJugables.Count);
                            jugadors[i].Carta_jugador.RemoveAt(0);
                            jugadors[i].Carta_jugador.Insert(0, cartesJugables[idxRNG]);
                            cartesJugades.Add(cartesJugables[idxRNG]);
                            cartesJugables.Remove(cartesJugables[idxRNG]);
                            pJugador[i].img1UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[0].RutaCarta));
                            pJugador[i].img1UC.Tag = jugadors[i].Carta_jugador[0].Id;
                            debugWindows.Text += "Carta pasada: IMG1 - " + pJugador[i].img1UC.Tag + "\n";
                        }
                    }
                    if (pJugador[i].img2UC.Tag.Equals(im.Tag))
                    { 
                        pJugador[i].img2UC.Source = null;
                        if(cartesJugables.Count != 0)
                        {
                            idxRNG = r.Next(cartesJugables.Count);
                            jugadors[i].Carta_jugador.RemoveAt(1);
                            jugadors[i].Carta_jugador.Insert(1, cartesJugables[idxRNG]);
                            cartesJugades.Add(cartesJugables[idxRNG]);
                            cartesJugables.Remove(cartesJugables[idxRNG]);
                            pJugador[i].img2UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[1].RutaCarta));
                            pJugador[i].img2UC.Tag = jugadors[i].Carta_jugador[1].Id;
                            debugWindows.Text += "Carta pasada: IMG2 - " + pJugador[i].img2UC.Tag + "\n" ;
                            debugWindows.Text += "Nova carta: " + jugadors[i].Carta_jugador[1].Id + "\n";
                            debugWindows.Text += "-------------------\n";
                        }
                    }
                    if (pJugador[i].img3UC.Tag.Equals(im.Tag))
                    {
                        pJugador[i].img3UC.Source = null;
                        if (cartesJugables.Count != 0)
                        {
                            idxRNG = r.Next(cartesJugables.Count);
                            jugadors[i].Carta_jugador.RemoveAt(2);
                            jugadors[i].Carta_jugador.Insert(2, cartesJugables[idxRNG]);
                            cartesJugades.Add(cartesJugables[idxRNG]);
                            cartesJugables.Remove(cartesJugables[idxRNG]);
                            pJugador[i].img3UC.Source = new BitmapImage(new Uri(jugadors[i].Carta_jugador[2].RutaCarta));
                            pJugador[i].img3UC.Tag = jugadors[i].Carta_jugador[2].Id;
                            debugWindows.Text += "Carta pasada: IMG3 - " + pJugador[i].img3UC.Tag + "\n";
                        }
                    }
                    

                    jugadors[i].Torn = false;
                    
                    if (i == 3)//3 hardcodejat => numero de jugadors
                    {
                        jugadors[0].Torn = true;
                    }
                    else
                    {
                        jugadors[i+1].Torn = true;
                        trobat = true;
                    }
                }
            }

            controladorTorn();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> noms = new List<string> { pJugador[0].txbJugadorUC.Text, pJugador[1].txbJugadorUC.Text, pJugador[2].txbJugadorUC.Text, pJugador[3].txbJugadorUC.Text };
            HashSet<string> nomsUnics = new HashSet<string>();
            bool unics = true;

            foreach (var n in noms)
            {
                if (!nomsUnics.Add(n))
                {
                    unics = false;
                }
            }
            
            
            if ((pJugador[0].txbJugadorUC.Text.Length >= 3 &&
                pJugador[1].txbJugadorUC.Text.Length >= 3 &&
                pJugador[2].txbJugadorUC.Text.Length >= 3 &&
                pJugador[3].txbJugadorUC.Text.Length >= 3) &&
                unics)
            {
                
                zonaj.btnStartUC.Visibility = (Visibility)1;
                joc_iniciat = true;
                
                pJugador[0].txbJugadorUC.IsEnabled = false;
                pJugador[1].txbJugadorUC.IsEnabled = false;
                pJugador[2].txbJugadorUC.IsEnabled = false;
                pJugador[3].txbJugadorUC.IsEnabled = false;
            }

            //borrar al finalitzar proves
            pJugador[0].txbJugadorUC.IsEnabled = false;
            zonaj.btnStartUC.Visibility = (Visibility)1;
        }
    }
}
