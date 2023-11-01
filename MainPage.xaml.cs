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

        List<Carta> cartesJugables = new List<Carta>();
        List<Carta> cartesJugades = new List<Carta>();
        List<Jugador> jugadors = new List<Jugador>();

        //triomf (tota la partida gira entorn aquest pal)
        int idxTriomf = 0;
        Carta triomf;
        triomfVista ptriomf;

        List<(Carta, Jugador)> trobarGuanyador = new List<(Carta, Jugador)>();
        List<(Carta, Jugador)> mateixPal = new List<(Carta, Jugador)>();
        List<(Carta, Jugador)> diferentPal = new List<(Carta, Jugador)>();

        zonaDeJoc zonaj;
        List<pantallaJugador> pJugador;

        int fiPartida = 0;
        int idxComparacio = 0;

        bool guanyadorTrobat = false;
        int cartesRepartides = 12;

        /*VARIABLES DE PROVA*/
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
                    jugadors[i].Carta_jugador.Add(cartesJugables[idxCarta]);
                    cartesJugades.Add(cartesJugables[idxCarta]);
                    cartesJugables.Remove(cartesJugables[idxCarta]);
                }
            }

            int jugadorInici = r.Next(jugadors.Count);
            jugadors[jugadorInici].Torn = true;


            for (int i = 0; i < pJugador.Count; i++)
            {

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
            cartesJugables.Remove(triomf);

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
                    pJugador[i].img1BackUC.Visibility = Visibility.Collapsed;
                    pJugador[i].img2BackUC.Visibility = Visibility.Collapsed;
                    pJugador[i].img3BackUC.Visibility = Visibility.Collapsed;
                }
            }
           
            volta++;
            if (volta == 4)
            {
                volta = 0;//Reiniciem el contador de voltes

                //borrem les cartes jugades anteriorment
                zonaj.imgCartaJugada1UC.Source = null;
                zonaj.imgCartaJugada2UC.Source = null;
                zonaj.imgCartaJugada3UC.Source = null;
                zonaj.imgCartaJugada4UC.Source = null;

                for (int i = 0; i < pJugador.Count; i++)
                {
                    //repartim les cartes
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

                for (int i = 0; i < trobarGuanyador.Count; i++)
                {
                    //només entra si el pal d'alguna carta es el triomf
                    if (trobarGuanyador[i].Item1.Pal_carta.Equals(triomf.Pal_carta))
                    {
                        mateixPal.Add(trobarGuanyador[i]);
                    }
                    else
                    {
                         diferentPal.Add(trobarGuanyador[i]);
                    }
                }

                if (mateixPal.Count == 0)
                {
                    //totes les cartes comparteixen pal amb el triomf
                    /* 1   -- 11
                    * 3   -- 10
                    * 12  --  4
                    * 11  --  3
                    * 10  --  2
                    * .   --  1
                    */
                    diferentPal = diferentPal.OrderBy(pair => pair.Item1.Num_carta).ToList();
                    if(diferentPal[0].Item1.Num_carta == 1 || diferentPal[0].Item1.Num_carta == 3)
                    {
                        idxComparacio = 0;
                    }
                    else if (diferentPal[0].Item1.Num_carta == 2 && diferentPal[1].Item1.Num_carta == 3)
                    {
                        idxComparacio = 1;
                    }
                    else
                    {
                        idxComparacio = diferentPal.Count - 1;
                    }
                    switch (diferentPal[idxComparacio].Item1.Num_carta)
                    {
                        case 1:
                            sumarPunts(11, diferentPal);
                            break;
                        case 3:
                            sumarPunts(10, diferentPal);
                            break;
                        case 12:
                            sumarPunts(4, diferentPal);
                            break;
                        case 11:
                            sumarPunts(3, diferentPal);
                            break;
                        case 10:
                            sumarPunts(2, diferentPal);
                            break;
                        case 9:
                        case 8:
                        case 7:
                        case 6:
                        case 5:
                        case 4:
                        case 2:
                            sumarPunts(1, diferentPal);
                            break;
                        default:
                            sumarPunts(0, diferentPal);
                            break;
                    }
                }
                else
                {
                    mateixPal = mateixPal.OrderBy(pair => pair.Item1.Num_carta).ToList();
                    if (mateixPal[0].Item1.Num_carta == 1 || mateixPal[0].Item1.Num_carta == 3)
                    {
                        idxComparacio = 0;
                    }
                    else if (mateixPal[0].Item1.Num_carta == 2 && mateixPal[1].Item1.Num_carta == 3)
                    {
                        idxComparacio = 1;
                    }
                    else
                    {
                        idxComparacio = mateixPal.Count - 1;
                    }
                    switch (mateixPal[idxComparacio].Item1.Num_carta)
                    {
                        case 1:
                            sumarPunts(11, mateixPal);
                            break;
                        case 3:
                            sumarPunts(10, mateixPal);
                            break;
                        case 12:
                            sumarPunts(4, mateixPal);
                            break;
                        case 11:
                            sumarPunts(3, mateixPal);
                            break;
                        case 10:
                            sumarPunts(2, mateixPal);
                            break;
                        case 9:
                        case 8:
                        case 7:
                        case 6:
                        case 5:
                        case 4:
                        case 2:
                            sumarPunts(1, mateixPal);
                            break;
                        default:
                            sumarPunts(0, mateixPal);
                            break;
                    }
                }

                idxComparacio = 0;

                diferentPal.Clear();
                mateixPal.Clear();
                trobarGuanyador.Clear();

                //GUANYADOR DE LA PARTIDA
                for (int i = 0; i < jugadors.Count; i++) {
                    if (jugadors[i].Torn && zonaj.imgCartaJugada4UC.Source == null && cartesRepartides == 48 && fiPartida == 2)
                    {
                        jugadors = jugadors.OrderByDescending(pair => pair.Puntuacio).ToList();
                        debugWindows.Text = "FI PARTIDA \n";
                        debugWindows.Text += "Guanyador: " + jugadors[0].Nom + " amb " + jugadors[0].Puntuacio + " punts";
                    }else if (jugadors[i].Torn && zonaj.imgCartaJugada4UC.Source == null && cartesRepartides == 48)
                    {
                        fiPartida++;
                    }
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

        private void sumarPunts(int punts, List<(Carta,Jugador)> l)
        {
            //trobar el jugador que a guanyat
            int guanya = 0;
            for (int i = 0; i < jugadors.Count; i++)
            {
                if (jugadors[i].Nom.Equals(l[idxComparacio].Item2.Nom))
                {
                    guanya = i; 
                    break;
                }
            }
            debugWindows.Text = "Jugador " + jugadors[guanya].Nom + " guanya " + punts +" punts\n";
            jugadors[guanya].Puntuacio += punts;
            pJugador[guanya].txbPuntuacioUC.Text = jugadors[guanya].Puntuacio + "";
        }


        public void Im_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image im = sender as Image;
            Random r = new Random();
            int idxRNG = 0;

            bool trobat = false;

            for (int i = 0; i < cartesJugades.Count; i++)
            {
                if (cartesJugades[i].Id.Equals(im.Tag) && volta == 0)
                {
                    zonaj.imgCartaJugada1UC.Source = new BitmapImage(new Uri(cartesJugades[i].RutaCarta));

                    for(int j = 0; j < jugadors.Count; j++)
                    {
                        if (jugadors[j].Torn)
                        {
                            trobarGuanyador.Add((cartesJugades[i], jugadors[j]));
                        }
                    }
                }
                if (cartesJugades[i].Id.Equals(im.Tag) && volta == 1)
                {
                    zonaj.imgCartaJugada2UC.Source = new BitmapImage(new Uri(cartesJugades[i].RutaCarta));
                    for (int j = 0; j < jugadors.Count; j++)
                    {
                        if (jugadors[j].Torn)
                        {
                            trobarGuanyador.Add((cartesJugades[i], jugadors[j]));
                        }
                    }
                }
                if (cartesJugades[i].Id.Equals(im.Tag) && volta == 2)
                {
                    zonaj.imgCartaJugada3UC.Source = new BitmapImage(new Uri(cartesJugades[i].RutaCarta));
                    for (int j = 0; j < jugadors.Count; j++)
                    {
                        if (jugadors[j].Torn)
                        {
                            trobarGuanyador.Add((cartesJugades[i], jugadors[j]));
                        }
                    }
                }
                if (cartesJugades[i].Id.Equals(im.Tag) && volta == 3)
                {
                    zonaj.imgCartaJugada4UC.Source = new BitmapImage(new Uri(cartesJugades[i].RutaCarta));
                    for (int j = 0; j < jugadors.Count; j++)
                    {
                        if (jugadors[j].Torn)
                        {
                            trobarGuanyador.Add((cartesJugades[i], jugadors[j]));
                        }
                    }
                }
            }

            for (int i = 0; i < jugadors.Count && !trobat; i++)
            {
                //comprobem el torn del jugador i el pasem al següent
                if (jugadors[i].Torn)
                {

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
                            cartesRepartides++;
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
                            cartesRepartides++;
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
                            cartesRepartides++;
                        }
                    }
                    

                    jugadors[i].Torn = false;
                    
                    if (i == 3)//3 hardcodejat => numero de jugadors
                    {
                        jugadors[0].Torn = true;
                        if (cartesRepartides == 47 && pJugador[0].img1UC.Source == null)
                        {
                            pJugador[0].img1UC.Source = new BitmapImage(new Uri(triomf.RutaCarta));
                            pJugador[0].img1UC.Tag = triomf.Id;
                            ptriomf.imgTriomfUC.Visibility = Visibility.Collapsed;
                            cartesRepartides++;
                        }else
                        if (cartesRepartides == 47 && pJugador[0].img2UC.Source == null)
                        {
                            pJugador[0].img2UC.Source = new BitmapImage(new Uri(triomf.RutaCarta));
                            pJugador[0].img2UC.Tag = triomf.Id;
                            ptriomf.imgTriomfUC.Visibility = Visibility.Collapsed;
                            cartesRepartides++;
                        }else
                        if (cartesRepartides == 47 && pJugador[0].img3UC.Source == null)
                        {
                            pJugador[0].img3UC.Source = new BitmapImage(new Uri(triomf.RutaCarta));
                            pJugador[0].img3UC.Tag = triomf.Id;
                            ptriomf.imgTriomfUC.Visibility = Visibility.Collapsed;
                            cartesRepartides++;
                        }
                    }
                    else
                    {
                        jugadors[i+1].Torn = true;
                        trobat = true;
                        if (cartesRepartides == 47 && pJugador[i+1].img1UC.Source == null)
                        {
                            pJugador[i+1].img1UC.Source = new BitmapImage(new Uri(triomf.RutaCarta));
                            pJugador[i+1].img1UC.Tag = triomf.Id;
                            ptriomf.imgTriomfUC.Visibility = Visibility.Collapsed;
                            cartesRepartides++;
                        }
                        else
                        if (cartesRepartides == 47 && pJugador[i+1].img2UC.Source == null)
                        {
                            pJugador[i+1].img2UC.Source = new BitmapImage(new Uri(triomf.RutaCarta));
                            pJugador[i+1].img2UC.Tag = triomf.Id;
                            ptriomf.imgTriomfUC.Visibility = Visibility.Collapsed;
                            cartesRepartides++;
                        }
                        else
                        if (cartesRepartides == 47 && pJugador[i+1].img3UC.Source == null)
                        {
                            pJugador[i+1].img3UC.Source = new BitmapImage(new Uri(triomf.RutaCarta));
                            pJugador[i+1].img3UC.Tag = triomf.Id;
                            ptriomf.imgTriomfUC.Visibility = Visibility.Collapsed;
                            cartesRepartides++;
                        }
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
                
                pJugador[0].txbJugadorUC.IsEnabled = false;
                pJugador[1].txbJugadorUC.IsEnabled = false;
                pJugador[2].txbJugadorUC.IsEnabled = false;
                pJugador[3].txbJugadorUC.IsEnabled = false;
            }
        }
    }
}
