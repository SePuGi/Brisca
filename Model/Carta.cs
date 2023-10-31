using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Brisca.Model
{
    internal class Carta
    {
        int id;
        String nom_carta;
        Pal pal_carta;
        int num_carta;
        String rutaCarta;

        public Carta(int id, string nom_carta, Pal pal_carta, int num_carta, String imCarta)
        {
            this.id = id;
            this.nom_carta = nom_carta;
            this.pal_carta = pal_carta;
            this.num_carta = num_carta;
            this.rutaCarta = imCarta;
        }
        public int Id { get => id; set => id = value; }
        public String Nom_carta { get => nom_carta; set => nom_carta = value; }
        public Pal Pal_carta { get => pal_carta; set => pal_carta = value; }
        public int Num_carta { get => num_carta; set => num_carta = value; }
        public String RutaCarta { get => rutaCarta; }

    }
}
