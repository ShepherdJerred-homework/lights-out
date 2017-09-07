using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lights_out {
    public partial class MainForm : Form {
        private const int GridOffsetInPixels = 25;
        private const int GridLengthInPixels = 200;
        private const int GridSize = 3;
        private const int CellLength = GridLengthInPixels / GridSize;
        private bool[,] Grid;
        private Random Random;

        public MainForm() {
            InitializeComponent();
            InitializeGrid();
            Random = new Random();
        }

        private void InitializeGrid() {
            Grid = new bool[GridSize, GridSize];

            for (int currentRow = 0; currentRow < GridSize; currentRow++) {
                for (int currentColumn = 0; currentColumn < GridSize; currentColumn++) {
                    Grid[currentRow, currentColumn] = true;
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
        }

        private void MainForm_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            for (int currentRow = 0; currentRow < GridSize; currentRow++) {
                for (int currentColumn = 0; currentColumn < GridSize; currentColumn++) {
                    Brush brush;
                    Pen pen;
                    if (Grid[currentRow, currentColumn]) {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    }
                    else {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    int x = currentColumn * CellLength + GridOffsetInPixels;
                    int y = currentRow * CellLength + GridOffsetInPixels;

                    g.DrawRectangle(pen, x, y, CellLength, CellLength);
                    g.FillRectangle(brush, x + 1, y + 1, CellLength - 1, CellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            // Make sure click was inside the grid
            if (e.X < GridOffsetInPixels || e.X > CellLength * GridSize + GridOffsetInPixels ||
                e.Y < GridOffsetInPixels || e.Y > CellLength * GridSize + GridOffsetInPixels)
                return;
            // Find row, col of mouse press
            int r = (e.Y - GridOffsetInPixels) / CellLength;
            int c = (e.X - GridOffsetInPixels) / CellLength;
            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
            for (int j = c - 1; j <= c + 1; j++)
                if (i >= 0 && i < GridSize && j >= 0 && j < GridSize)
                    Grid[i, j] = !Grid[i, j];
            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (PlayerWon()) {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool PlayerWon() {
            for (int currentRow = 0; currentRow < GridSize; currentRow++) {
                for (int currentColumn = 0; currentColumn < GridSize; currentColumn++) {
                    if (Grid[currentRow, currentColumn] == false) {
                        return false;
                    }
                }
            }
            return true;
        }

        private void newGameButton_Click(object sender, EventArgs e) {
            newGame();
        }

        private void exitButton_Click(object sender, EventArgs e) {
            exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            newGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void exit() {
            Close();
        }

        private void newGame() {
            for (int currentRow = 0; currentRow < GridSize; currentRow++) {
                for (int currentColumn = 0; currentColumn < GridSize; currentColumn++) {
                    Grid[currentRow, currentColumn] = Random.Next(2) == 1;
                }
            }
            this.Invalidate();
        }
    }
}