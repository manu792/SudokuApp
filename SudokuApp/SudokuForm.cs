using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuApp
{
    public partial class SudokuForm : Form
    {
        private const int filas = 9;
        private Sudoku sudoku;
        private int[,] grid;
        private List<Coordenadas> celdasInvalidas;

        public SudokuForm()
        {
            celdasInvalidas = new List<Coordenadas>();
            sudoku = new Sudoku();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerarGrid();
        }

        private void GenerarGrid()
        {
            for (int i = 0; i < filas; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Height = 45;

                sudokuGrid.Rows.Add(row);
            }
            sudokuGrid.Rows[2].DividerHeight = 2;
            sudokuGrid.Rows[5].DividerHeight = 2;

            sudokuGrid.ClearSelection();
        }

        private void dataGridView1_EditingControlShowing(object sender, System.Windows.Forms.DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.KeyPress += new KeyPressEventHandler(EditingControl_KeyPress);
            }
        }

        private void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 48) // revisar que el numero ingresado NO sea 0
                e.Handled = true;
            else
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (sudokuGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && sudokuGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != string.Empty)
                grid[e.RowIndex, e.ColumnIndex] = Int32.Parse(sudokuGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());


            if (celdasInvalidas.Count > 0)
                LimpiarCeldasInvalidas();

            if (EstaLlena())
            {
                celdasInvalidas = sudoku.ValidarSudoku();
                MarcarCeldasInvalidas();  // Marco de rojo las celdas invalidas que tienen numeros repetidos

                if (celdasInvalidas.Count == 0)
                {
                    MessageBox.Show("Bien hecho!");
                    sudokuGrid.Enabled = false;
                }
            }
        }
        private void MarcarCeldasInvalidas()
        {
            foreach (Coordenadas coordenadas in celdasInvalidas)
            {
                sudokuGrid.Rows[coordenadas.Fila].Cells[coordenadas.Columna].Style.BackColor = Color.Red;
            }
        }
        private void LimpiarCeldasInvalidas()
        {
            foreach (Coordenadas coordenadas in celdasInvalidas)
            {
                sudokuGrid.Rows[coordenadas.Fila].Cells[coordenadas.Columna].Style.BackColor = Color.AliceBlue;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            celdasInvalidas.Clear();

            if (comboBox1.SelectedIndex >= 0)
            {
                Nivel nivel = (Nivel)comboBox1.SelectedIndex;
                grid = sudoku.NuevoJuego(nivel);
                ImprimirGrid(grid);

                button3.Enabled = true;
                sudokuGrid.Enabled = true;
            }
        }

        private void ImprimirGrid(int[,] grid)
        {
            for (int fila = 0; fila < sudokuGrid.Rows.Count; fila++)
            {
                for (int columna = 0; columna < sudokuGrid.Columns.Count; columna++)
                {
                    if (grid[fila, columna] != 0)
                    {
                        sudokuGrid.Rows[fila].Cells[columna].Value = grid[fila, columna];
                        sudokuGrid.Rows[fila].Cells[columna].ReadOnly = true;
                        sudokuGrid.Rows[fila].Cells[columna].Style.BackColor = Color.LightBlue;
                    }
                    else
                    {
                        sudokuGrid.Rows[fila].Cells[columna].Value = string.Empty;
                        sudokuGrid.Rows[fila].Cells[columna].ReadOnly = false;
                        sudokuGrid.Rows[fila].Cells[columna].Style.BackColor = Color.AliceBlue;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sudoku.GridOriginal != null)
            {
                celdasInvalidas.Clear();
                ImprimirGrid(sudoku.GridOriginal);

                sudokuGrid.Enabled = true;
            }
            
        }

        private bool EstaLlena()
        {
            foreach (DataGridViewRow fila in sudokuGrid.Rows)
            {
                foreach (DataGridViewCell celda in fila.Cells)
                {
                    if (celda.Value == null || celda.Value.ToString().Equals(string.Empty))
                        return false;
                }
            }
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (EstaLlena())
            {
                if (celdasInvalidas.Count == 0)
                    MessageBox.Show("Bien hecho!");
                else
                    MessageBox.Show("Fallaste. Intenta un nuevo juego o haz click en Reiniciar para reiniciar este mismo juego.");

                sudokuGrid.Enabled = false;
            }
            else
            {
                MessageBox.Show("Aun hay celdas sin completar");
            }
        }
    }
}