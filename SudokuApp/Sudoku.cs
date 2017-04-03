using System;
using System.Collections.Generic;
using System.Data;

namespace SudokuApp
{
    public class Sudoku
    {
        private static int[,] sudokuOriginal = 
        {
            {7,9,2,3,5,1,8,4,6},
            {4,6,8,9,2,7,5,1,3},
            {1,3,5,6,8,4,7,9,2},
            {6,2,1,5,7,9,4,3,8},
            {5,8,3,2,4,6,1,7,9},
            {9,7,4,8,1,3,2,6,5},
            {8,1,6,4,9,2,3,5,7},
            {3,5,7,1,6,8,9,2,4},
            {2,4,9,7,3,5,6,8,1}
        };

        public int[,] GridOriginal { get; private set; }
        private int[,] grid;

        private const int Cuadrantes = 9;
        private Random numeroRandom;
        private int numerosPorCuadrante;
        private int filaMinima;
        private int filaMaxima;
        private int columnaMinima;
        private int columnaMaxima;
        
        public Sudoku()
        {
            numeroRandom = new Random();
        }
        public int[,] NuevoJuego(Nivel nivel)
        {
            if (nivel == Nivel.Medio)
                numerosPorCuadrante = 4;
            else if (nivel == Nivel.Avanzado)
                numerosPorCuadrante = 3;
            
            MezclarGrid(); //Mezcla el grid original para obtener nuevos grids a partir de ese
            GenerarJuego();
            return grid;
        }
        public List<Coordenadas> ValidarSudoku()
        {
            List<Coordenadas> celdasInvalidas = new List<Coordenadas>();
            for (int fila = 0; fila < 9; fila++)
            {
                for(int columna = 0; columna < 9; columna++)
                {
                    VerificarReglas(grid[fila, columna], fila, columna, celdasInvalidas);
                }
            }
            return celdasInvalidas;
        }
        private void VerificarReglas(int numero, int fila, int columna, List<Coordenadas> celdasInvalidas)
        {
            int cuadrante = ObtenerCuadrantePorCelda(fila, columna);

            ExisteEnCuadrante(numero, cuadrante, celdasInvalidas);
            ExisteEnFila(numero, fila, celdasInvalidas);
            ExisteEnColumna(numero, columna, celdasInvalidas);
        }
        private void VerificarRepetidos(List<Coordenadas> lista, List<Coordenadas> listaCoordenadas)
        {
            if (listaCoordenadas.Count > 1)
            {
                foreach (Coordenadas coordenadas in listaCoordenadas)
                {
                    if (!lista.Contains(coordenadas))
                        lista.Add(coordenadas);
                }
            }
        }
        private int ObtenerCuadrantePorCelda(int fila, int columna)
        {
            if ((fila >= 0 && fila <= 2) && (columna >= 0 && columna <= 2))
                return 0;
            if ((fila >= 0 && fila <= 2) && (columna >= 3 && columna <= 5))
                return 1;
            if ((fila >= 0 && fila <= 2) && (columna >= 6 && columna <= 8))
                return 2;
            if ((fila >= 3 && fila <= 5) && (columna >= 0 && columna <= 2))
                return 3;
            if ((fila >= 3 && fila <= 5) && (columna >= 3 && columna <= 5))
                return 4;
            if ((fila >= 3 && fila <= 5) && (columna >= 6 && columna <= 8))
                return 5;
            if ((fila >= 6 && fila <= 8) && (columna >= 0 && columna <= 2))
                return 6;
            if ((fila >= 6 && fila <= 8) && (columna >= 3 && columna <= 5))
                return 7;
            if ((fila >= 6 && fila <= 8) && (columna >= 6 && columna <= 8))
                return 8;
            return -1;
        }
        private void GenerarJuego()
        {
            int fila, columna;
            bool repetir = true;

            grid = new int[9, 9];
            GridOriginal = new int[9, 9];
            for (int cuadrante = 0; cuadrante < Cuadrantes; cuadrante++)
            {
                for (int cantidadNumeros = 0; cantidadNumeros < numerosPorCuadrante; cantidadNumeros++)
                {
                    do{
                        ObtenerFilaColumnaPorCuadrante(cuadrante);
                        fila = numeroRandom.Next(filaMinima, filaMaxima);
                        columna = numeroRandom.Next(columnaMinima, columnaMaxima);

                        // Verifico que las coordenadas no sean repetidas y que el numero cumpla con las reglas del juego
                        if (!CeldaTieneDato(fila, columna))
                        {
                            grid[fila, columna] = sudokuOriginal[fila, columna];
                            GridOriginal[fila, columna] = sudokuOriginal[fila, columna];
                            repetir = false;
                        }
                    }while (repetir);
                    repetir = true;
                }
            }
        }
        private void ObtenerFilaColumnaPorCuadrante(int cuadrante)
        {
            switch (cuadrante)
            {
                case 0:
                    filaMaxima = 3;
                    columnaMaxima = 3;
                    filaMinima = 0;
                    columnaMinima = 0;
                    break;
                case 1:
                    filaMaxima = 3;
                    columnaMaxima = 6;
                    filaMinima = 0;
                    columnaMinima = 3;
                    break;
                case 2:
                    filaMaxima = 3;
                    columnaMaxima = 9;
                    filaMinima = 0;
                    columnaMinima = 6;
                    break;
                case 3:
                    filaMaxima = 6;
                    columnaMaxima = 3;
                    filaMinima = 3;
                    columnaMinima = 0;
                    break;
                case 4:
                    filaMaxima = 6;
                    columnaMaxima = 6;
                    filaMinima = 3;
                    columnaMinima = 3;
                    break;
                case 5:
                    filaMaxima = 6;
                    columnaMaxima = 9;
                    filaMinima = 3;
                    columnaMinima = 6;
                    break;
                case 6:
                    filaMaxima = 9;
                    columnaMaxima = 3;
                    filaMinima = 6;
                    columnaMinima = 0;
                    break;
                case 7:
                    filaMaxima = 9;
                    columnaMaxima = 6;
                    filaMinima = 6;
                    columnaMinima = 3;
                    break;
                case 8:
                    filaMaxima = 9;
                    columnaMaxima = 9;
                    filaMinima = 6;
                    columnaMinima = 6;
                    break;
            }
        }
        private void MezclarGrid()
        {
            int cantidadVeces = numeroRandom.Next(1, 1000);

            for (int i = 0; i < cantidadVeces; i++)
            {
                switch (numeroRandom.Next(5))
                {
                    case 0:
                        ConvertirColumnasEnFilas();
                        break;
                    case 1:
                        IntercambiarFilas();
                        break;
                    case 2:
                        IntercambiarColumnas();
                        break;
                    case 3:
                        IntercambiarGruposFilas();
                        break;
                    case 4:
                        IntercambiarGruposColumnas();
                        break;
                }
            }
            //Imprimo el sudoku generado en consola
            for(int fila = 0; fila < 9; fila++)
            {
                for(int columna = 0; columna < 9; columna++)
                {
                    if (columna == 8)
                        Console.WriteLine(sudokuOriginal[fila, columna]);
                    else
                        Console.Write(sudokuOriginal[fila, columna] + " ");

                    if (fila == 8 && columna == 8)
                        Console.WriteLine("\n\n");
                }
            }
        }
        private void IntercambiarFilas()
        {
            //Agarra un grupo de 3 filas y luego selecciona dos filas aleatorias de ese grupo y las intercambia
            int grupo = numeroRandom.Next(3);

            int fila1 = numeroRandom.Next(3);
            int fila2;
            do
                fila2 = numeroRandom.Next(3);
            while (fila1 == fila2);

            for (int i = 0; i < 9; i++)
            {
                int temp = sudokuOriginal[grupo * 3 + fila2, i];
                sudokuOriginal[grupo * 3 + fila2, i] = sudokuOriginal[grupo * 3 + fila1, i];
                sudokuOriginal[grupo * 3 + fila1, i] = temp;
            }
        }
        private void IntercambiarColumnas()
        {
            ConvertirColumnasEnFilas();
            IntercambiarFilas();
            ConvertirColumnasEnFilas();
        }
        private void ConvertirColumnasEnFilas()
        {
            // Convierte cada columna de sudokuOriginal en una fila en la matriz temp
            int[,] temp = new int[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    temp[i, j] = sudokuOriginal[j, i];
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sudokuOriginal[i, j] = temp[i, j];
                }
            }
        }
        private void IntercambiarGruposFilas()
        {
            //Agarra dos grupos de 3 filas y los intercambia
            int grupo1 = numeroRandom.Next(3);
            int grupo2;

            do
                grupo2 = numeroRandom.Next(3);
            while (grupo1 == grupo2);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int temp = sudokuOriginal[grupo2 * 3 + i, j];
                    sudokuOriginal[grupo2 * 3 + i, j] = sudokuOriginal[grupo1 * 3 + i, j];
                    sudokuOriginal[grupo1 * 3 + i, j] = temp;
                }
            }
        }
        private void IntercambiarGruposColumnas()
        {
            ConvertirColumnasEnFilas();
            IntercambiarGruposFilas();
            ConvertirColumnasEnFilas();
        }
        private bool CeldaTieneDato(int fila, int columna)
        {
            return grid[fila, columna] != 0;
        }
        private bool EsNumeroCero(int numero)
        {
            return numero == 0;
        }
        private void ExisteEnCuadrante(int numero, int cuadrante, List<Coordenadas> celdasInvalidas)
        {
            //List<Coordenadas> lista = new List<Coordenadas>();
            int contador = 0;
            bool existenCeldasInvalidas = false;

            ObtenerFilaColumnaPorCuadrante(cuadrante);
            for (int fila = filaMinima; fila < filaMaxima; fila++)
            {
                for (int columna = columnaMinima; columna < columnaMaxima; columna++)
                {
                    if (grid[fila, columna] == numero)
                    {
                        contador++;
                        Coordenadas coordenadas = new Coordenadas(fila, columna);
                        if (!celdasInvalidas.Contains(coordenadas))
                        {
                            existenCeldasInvalidas = true;
                            celdasInvalidas.Add(coordenadas);
                        }
                    }
                }
            }
            if (contador == 1 && existenCeldasInvalidas)
                celdasInvalidas.RemoveAt(celdasInvalidas.Count - 1);
        }
        private void ExisteEnFila(int numero, int fila, List<Coordenadas> celdasInvalidas)
        {
            //List<Coordenadas> lista = new List<Coordenadas>();
            int contador = 0;
            bool existenCeldasInvalidas = false;

            for (int columna = 0; columna < 9; columna++)
            {
                if (grid[fila, columna] == numero)
                {
                    contador++;
                    Coordenadas coordenadas = new Coordenadas(fila, columna);
                    if (!celdasInvalidas.Contains(coordenadas))
                    {
                        existenCeldasInvalidas = true;
                        celdasInvalidas.Add(coordenadas);
                    }

                    //contador++;
                    //Coordenadas coordenadas = new Coordenadas(fila, columna);
                    //lista.Add(coordenadas);
                }
            }
            if (contador == 1 && existenCeldasInvalidas)
                celdasInvalidas.RemoveAt(celdasInvalidas.Count - 1);
        }
        private void ExisteEnColumna(int numero, int columna, List<Coordenadas> celdasInvalidas)
        {
            //List<Coordenadas> lista = new List<Coordenadas>();
            int contador = 0;
            bool existenCeldasInvalidas = false;

            for (int fila = 0; fila < 9; fila++)
            {
                if (grid[fila, columna] == numero)
                {
                    contador++;
                    Coordenadas coordenadas = new Coordenadas(fila, columna);
                    if (!celdasInvalidas.Contains(coordenadas))
                    {
                        existenCeldasInvalidas = true;
                        celdasInvalidas.Add(coordenadas);
                    }

                    //contador++;
                    //Coordenadas coordenadas = new Coordenadas(fila, columna);
                    //lista.Add(coordenadas);
                }
            }
            if (contador == 1 && existenCeldasInvalidas)
                celdasInvalidas.RemoveAt(celdasInvalidas.Count - 1);
        }
    }
}
