using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuApp
{
    public class Coordenadas : IEquatable<Coordenadas>
    {
        public Coordenadas(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
        }
        public int Fila { get; private set; }
        public int Columna { get; private set; }

        public bool Equals(Coordenadas other)
        {
            return this.Fila.Equals(other.Fila) && this.Columna.Equals(other.Columna);
        }
    }
}
