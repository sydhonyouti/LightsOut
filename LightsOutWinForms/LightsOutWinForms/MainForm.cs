using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOutWinForms
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25;      //Distance from upper-left side of window
        private const int gridLength = 200;     //Size in pixels of grid
        private LightsOutGame game;             //Game logic

        public MainForm()
        {
            InitializeComponent();
            game = new LightsOutGame();
        }

        private void newGameButton(object sender, EventArgs e)
        {
            game.NewGame();

            // Redraw Grid
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int cellLength = gridLength / game.GridSize;

            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    // Get proper pen and brush for
                    // on/off grid section
                    Brush brush;
                    Pen pen;

                    if (game.IsOn(r,c))
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;  // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;  // Off
                    }

                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * cellLength + GridOffset;
                    int y = r * cellLength + GridOffset;

                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, cellLength, cellLength);
                    g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
                }
            }
        }

        private void exitButton(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGameButton(sender, e);
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            int cellLength = gridLength / game.GridSize;

            //Make sure click was inside the grid
            if (e.X < GridOffset || e.X > cellLength * game.GridSize + GridOffset ||
                e.Y < GridOffset || e.Y > cellLength * game.GridSize + GridOffset)
                return;

            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / cellLength;
            int c = (e.X - GridOffset) / cellLength;

            game.FlipLight(r, c);

            // Redraw grid
            this.Invalidate();
            
            //Check to see if puzzle has been solved
            if (game.IsGameOver())
            {
                // Display winnder dialog box just inside window
                MessageBox.Show(this, "Congradulations! You've won!", "Lights Out!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (e.Button == MouseButtons.Right)
            {
                game.Cheat();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        }

        public void UncheckOtherToolStripMenuItems(ToolStripMenuItem selectedMenuItem)
        {
            selectedMenuItem.Checked = true;

            // Select the other MenuItens from the ParentMenu(OwnerItens) and unchecked this,
            // The current Linq Expression verify if the item is a real ToolStripMenuItem
            // and if the item is a another ToolStripMenuItem to uncheck this.
            foreach (var ltoolStripMenuItem in (from object
                                                    item in selectedMenuItem.Owner.Items
                                                let ltoolStripMenuItem = item as ToolStripMenuItem
                                                where ltoolStripMenuItem != null
                                                where !item.Equals(selectedMenuItem)
                                                select ltoolStripMenuItem))
                (ltoolStripMenuItem).Checked = false;

            // I used help to figure out how to individually click on one item at a time
            //https://stackoverflow.com/questions/13603654/check-only-one-toolstripmenuitem

        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);

            //Change the grid
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        }

        // 4x4 Grid Paint
        private void x4ToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int cellLength = gridLength / 4;

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    // Get proper pen and brush for
                    // on/off grid section
                    Brush brush;
                    Pen pen;

                    if (game.IsOn(r, c))
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;  // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;  // Off
                    }

                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * cellLength + GridOffset;
                    int y = r * cellLength + GridOffset;

                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, cellLength, cellLength);
                    g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
                }
            }
        }
    }
}
