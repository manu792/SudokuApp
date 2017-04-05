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
        private int[,] gridUsuario;

        private const int Cuadrantes = 9;
        private const int Filas = 9;
        private const int Columnas = 9;
        private Random numeroRandom;
        private int numerosPorCuadrante;
        private int filaMinimaCuadrante;
        private int columnaMinimaCuadrante;
        
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
            return gridUsuario;
        }
        public List<Coordenadas> ValidarSudoku()
        {
            List<Coordenadas> celdasInvalidas = new List<Coordenadas>();
            for (int fila = 0; fila < Filas; fila++)
            {
                for (int columna = 0; columna < Columnas; columna++)
                {
                    VerificarReglas(gridUsuario[fila, columna], fila, columna, celdasInvalidas);
                }
            }
            return celdasInvalidas;
        }
        private void VerificarReglas(int numero, int fila, int columna, List<Coordenadas> celdasInvalidas)
        {
            ObtenerCuadrantePorCelda(fila, columna);

            ExisteEnCuadrante(numero, celdasInvalidas);
            ExisteEnFila(numero, fila, celdasInvalidas);
            ExisteEnColumna(numero, columna, celdasInvalidas);
        }
        private int ObtenerCuadrantePorCelda(int fila, int columna)
        {
            //Se usa cuadrante basado en indice 0
            int anchoCuadrante = 3;

            filaMinimaCuadrante = fila / anchoCuadrante * 3;
            columnaMinimaCuadrante = columna / anchoCuadrante * 3;
            return (columnaMinimaCuadrante + filaMinimaCuadrante * anchoCuadrante) / 3;
        }
        private void GenerarJuego()
        {
            int fila, columna;
            bool repetir = true;

            gridUsuario = new int[Filas, Columnas];
            GridOriginal = new int[Filas, Columnas];
            for (int cuadrante = 0; cuadrante < Cuadrantes; cuadrante++)
            {
                for (int cantidadNumeros = 0; cantidadNumeros < numerosPorCuadrante; cantidadNumeros++)
                {
                    do{
                        fila = numeroRandom.Next(0, Filas);
                        columna = numeroRandom.Next(0, Columnas);
                        if (ObtenerCuadrantePorCelda(fila, columna) == cuadrante)
                        {
                            // Verifico que las coordenadas no sean repetidas
                            if (!CeldaTieneDato(fila, columna))
                            {
                                gridUsuario[fila, columna] = sudokuOriginal[fila, columna];
                                GridOriginal[fila, columna] = sudokuOriginal[fila, columna];
                                repetir = false;
                            }
                        }
                    }while (repetir);
                    repetir = true;
                }
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
            for(int fila = 0; fila < Filas; fila++)
            {
                for(int columna = 0; columna < Columnas; columna++)
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
            int grupos = Filas / 3;

            int grupo = numeroRandom.Next(grupos);

            int fila1 = numeroRandom.Next(3);
            int fila2;
            do
                fila2 = numeroRandom.Next(3);
            while (fila1 == fila2);

            for (int i = 0; i < Filas; i++)
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
            int[,] temp = new int[Filas, Columnas];

            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    temp[i, j] = sudokuOriginal[j, i];
                }
            }
            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    sudokuOriginal[i, j] = temp[i, j];
                }
            }
        }
        private void IntercambiarGruposFilas()
        {
            //Agarra dos grupos de 3 filas y los intercambia
            int grupos = Filas / 3;
            int grupo1 = numeroRandom.Next(grupos);
            int grupo2;

            do
                grupo2 = numeroRandom.Next(grupos);
            while (grupo1 == grupo2);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < Filas; j++)
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
            return gridUsuario[fila, columna] != 0;
        }
        private void ExisteEnCuadrante(int numero, List<Coordenadas> celdasInvalidas)
        {
            int contador = 0;
            bool existenCeldasInvalidas = false;

            //ObtenerFilaColumnaPorCuadrante(cuadrante);
            for (int fila = filaMinimaCuadrante; fila < filaMinimaCuadrante + 3; fila++)
            {
                for (int columna = columnaMinimaCuadrante; columna < columnaMinimaCuadrante + 3; columna++)
                {
                    if (gridUsuario[fila, columna] == numero)
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
            int contador = 0;
            bool existenCeldasInvalidas = false;

            for (int columna = 0; columna < Columnas; columna++)
            {
                if (gridUsuario[fila, columna] == numero)
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
            if (contador == 1 && existenCeldasInvalidas)
                celdasInvalidas.RemoveAt(celdasInvalidas.Count - 1);
        }
        private void ExisteEnColumna(int numero, int columna, List<Coordenadas> celdasInvalidas)
        {
            int contador = 0;
            bool existenCeldasInvalidas = false;

            for (int fila = 0; fila < Filas; fila++)
            {
                if (gridUsuario[fila, columna] == numero)
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
            if (contador == 1 && existenCeldasInvalidas)
                celdasInvalidas.RemoveAt(celdasInvalidas.Count - 1);
        }
    }
}
