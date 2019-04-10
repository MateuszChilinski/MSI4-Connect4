using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect4Game
{
    public partial class Form1 : Form
    {
        const int rows = 6;
        const int columns = 7;
        const int margin = 10;

        PlayerColor UserPlayer = PlayerColor.Red;
        FieldType[,] Board = new FieldType[rows,columns]; // row, column
        GameStatusType GameStatus = GameStatusType.Initialised;
        enum GameStatusType
        {
            Initialised = 0,
            Started = 1,
            RedWin = 2,
            BlackWin = 3
        }
        enum FieldType
        {
            Empty = 0,
            Red = 1, // red always starts
            Black = 2
        };
        public enum PlayerColor
        {
            Red = 0,
            Black = 1
        }
        private void DrawConnect4(Graphics graphics)
        {
            int MaxWidth = Connect4Board.Size.Width;
            int MaxHeight = Connect4Board.Size.Height;

            
            
            float UsedWidthPerOne = (MaxWidth - 2 * margin)/(float)columns;
            float UsedHeightPerOne = (MaxHeight - 2 * margin)/(float)rows;

            for(int i = 0; i <= columns; i++)
            {
                graphics.DrawLine(Pens.Black, margin + UsedWidthPerOne * i, margin, margin + UsedWidthPerOne * i, MaxHeight - margin);
            }

            for (int i = 0; i <= rows; i++)
            {
                graphics.DrawLine(Pens.Black, margin, margin + UsedHeightPerOne * i, MaxWidth - margin, margin + UsedHeightPerOne * i);
            }
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    var field = Board[i, j];
                    if (field == FieldType.Empty)
                        continue;
                    if(field == FieldType.Red)
                    {
                        DrawCircle(graphics, (int)UsedWidthPerOne, (int)UsedHeightPerOne, margin, 2, 2, Brushes.Red);
                    }
                    else
                        DrawCircle(graphics, (int)UsedWidthPerOne, (int)UsedHeightPerOne, margin, 2, 2, Brushes.Black);
                }
            }

            
        }
        private void DrawCircle(Graphics graphics, int UsedWidthPerOne, int UsedHeightForOne, int margin, int row, int column, Brush brush)
        {
            graphics.FillEllipse(brush, new Rectangle(UsedWidthPerOne * row + (int)(margin * 1.5), UsedHeightForOne * column + (int)(margin * 1.5), 
                UsedWidthPerOne - margin, UsedHeightForOne - margin));

        }
        public bool PutInColumn(int Column, PlayerColor playerColor)
        {
            if(GameStatus == GameStatusType.RedWin || GameStatus == GameStatusType.BlackWin) // if game has finished one cannot put more
                return false;
            if (Board[Board.GetLength(0)-1, Column] != FieldType.Empty) // if column is full, not possible
                return false;

            for(int i = 0; i < Board.GetLength(0); i++)
            {
                if(Board[i,Column] == FieldType.Empty)
                {
                    if (playerColor == PlayerColor.Red)
                        Board[i, Column] = FieldType.Red;
                    else
                        Board[i, Column] = FieldType.Black;
                    break;
                }
            }

            return true;
        }
        private void RestartGame()
        {
            GameStatus = GameStatusType.Initialised;
            for(int i = 0; i < Board.GetLength(0); i++)
            {
                for(int j = 0; j < Board.GetLength(1); j++)
                {
                    var field = Board[i, j];
                    field = FieldType.Empty;
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
            Connect4Board.Paint += Connect4Board_Paint;
        }

        private void Connect4Board_Paint(object sender, PaintEventArgs e)
        {
            DrawConnect4(e.Graphics);
        }

        private void ChangeToColor()
        {
            if(GameStatus == GameStatusType.Started)
            {
                DialogResult dialogResult = MessageBox.Show("Gra została już rozpoczęta. Zmiana koloru spowoduje restart gry. Czy chcesz zmienić kolor?", "Gra już rozpoczęta!", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    RedRadioButton.Checked = !RedRadioButton.Checked;
                    BlackRadioButton.Checked = !BlackRadioButton.Checked;
                    return;
                }
            }
            if (RedRadioButton.Checked)
                UserPlayer = PlayerColor.Red;
            if (BlackRadioButton.Checked)
                UserPlayer = PlayerColor.Black;
            RestartGame();
        }

        private void RedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (UserPlayer == PlayerColor.Red && RedRadioButton.Checked)
                return;
            ChangeToColor();
        }

        private void BlackRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (UserPlayer == PlayerColor.Black && BlackRadioButton.Checked)
                return;
            ChangeToColor();
        }

        private void Connect4Board_Click(object sender, EventArgs e)
        {
            int X = MousePosition.X;
        }

        private Tuple?<int,int> GetAvailableInRow(int x)
        {
            int MaxWidth = Connect4Board.Width - 2 * margin;
            int UsedWidthPerOne = (int)((MaxWidth - 2 * margin) / (float)columns);
            int row = 0;
            if (x < margin)
                return null;
            for (int i = margin; i < MaxWidth; i+= UsedWidthPerOne)
            {
                if (x > i && x < i + UsedWidthPerOne)
                {
                    break;
                }
                row++;
            }

        }

        private void Connect4Board_MouseHover(object sender, EventArgs e)
        {
            
        }
    }
}
