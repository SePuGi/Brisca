using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brisca.Model
{
    internal class Jugador
    {
        String nom;
        bool torn;
        int puntuacio;
        //cartes que té (MAX 3)
        List<Carta> carta_jugador;

        public Jugador(string nom, bool torn)
        {
            this.nom = nom;
            this.torn = torn;
            this.puntuacio = 0;
            this.carta_jugador = new List<Carta>();
        }

        public String Nom { get => nom; set => nom = value; }
        public bool Torn { get => torn; set => torn = value; }
        public int Puntuacio { get => puntuacio; set => puntuacio = value; }
        public List<Carta> Carta_jugador { get => carta_jugador; set => carta_jugador = value; }

    }
}
