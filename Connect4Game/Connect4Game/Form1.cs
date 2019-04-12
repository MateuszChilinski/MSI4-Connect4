using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCTS;
using Connect4Model;

namespace Connect4Game
{
    public partial class Form1 : Form
    {

        private Tuple<int, int> CurrentHover = null;
        const int margin = 10;
        PlayerColor UserPlayer = PlayerColor.Red;
        private void DrawConnect4(Graphics graphics)
        {
            int MaxWidth = Connect4Board.Size.Width;
            int MaxHeight = Connect4Board.Size.Height;
            
            float UsedWidthPerOne = (MaxWidth - 2 * margin)/(float)Game.Columns;
            float UsedHeightPerOne = (MaxHeight - 2 * margin)/(float)Game.Rows;

            for(int i = 0; i <= Game.Columns; i++)
            {
                graphics.DrawLine(Pens.Black, margin + UsedWidthPerOne * i, margin, margin + UsedWidthPerOne * i, MaxHeight - margin);
            }

            for (int i = 0; i <= Game.Rows; i++)
            {
                graphics.DrawLine(Pens.Black, margin, margin + UsedHeightPerOne * i, MaxWidth - margin, margin + UsedHeightPerOne * i);
            }
            for (int i = 0; i < Game.Rows; i++)
            {
                for (int j = 0; j < Game.Columns; j++)
                {
                    var field = Game.Board[i, j];
                    if (field == FieldType.Empty)
                        continue;
                    if(field == FieldType.Red)
                    {
                        DrawCircle(graphics, (int)UsedWidthPerOne, (int)UsedHeightPerOne, margin, i, j, Brushes.Red);
                    }
                    else
                        DrawCircle(graphics, (int)UsedWidthPerOne, (int)UsedHeightPerOne, margin, i, j, Brushes.Black);
                }
            }
            if(CurrentHover != null && Game.GameStatus != GameStatusType.BlackWin && Game.GameStatus != GameStatusType.RedWin)
                DrawCircle(graphics, (int)UsedWidthPerOne, (int)UsedHeightPerOne, margin, CurrentHover.Item1, CurrentHover.Item2, Brushes.Orange);


        }
        private void DrawCircle(Graphics graphics, int UsedWidthPerOne, int UsedHeightForOne, int margin, int row, int column, Brush brush)
        {
            graphics.FillEllipse(brush, new Rectangle(UsedWidthPerOne * column + (int)(margin * 1.5), UsedHeightForOne * (Game.Rows - row-1) + (int)(margin * 1.8), 
                UsedWidthPerOne - margin, UsedHeightForOne - margin));

        }
        IConnect4 Game;
        IMCTSAI AI;
        public Form1()
        {
            InitializeComponent();
            RestartButton.Click += RestartButton_Click;
            Connect4Board.Paint += Connect4Board_Paint;
            Game = new Connect4();
            AI = new MCTSAI();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            Game.RestartGame();
            Connect4Board.Refresh();
            AI = new MCTSAI();
        }

        private void Connect4Board_Paint(object sender, PaintEventArgs e)
        {
            DrawConnect4(e.Graphics);
        }

        private void ChangeToColor()
        {
            if(Game.GameStatus == GameStatusType.Started)
            {
                DialogResult dialogResult = MessageBox.Show("Gra została już rozpoczęta. Zmiana koloru spowoduje restart gry. Czy chcesz zmienić kolor?", "Gra już rozpoczęta!", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    if (BlackRadioButton.Checked)
                        RedRadioButton.Checked = !RedRadioButton.Checked;
                    else
                        BlackRadioButton.Checked = !BlackRadioButton.Checked;
                    return;
                }
            }
            if (RedRadioButton.Checked)
                UserPlayer = PlayerColor.Red;
            if (BlackRadioButton.Checked)
                UserPlayer = PlayerColor.Black;
            Game.RestartGame();
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

        private void Connect4Board_Click(object sender, MouseEventArgs e)
        {
            var available = GetAvailableInRow(e.Location.X);
            if (available != null && Game.GameStatus != GameStatusType.BlackWin && Game.GameStatus != GameStatusType.RedWin)
            {
                Game.PutInColumn(available.Item2, UserPlayer);
                if (Game.GameStatus != GameStatusType.Started && Game.GameStatus != GameStatusType.Initialised)
                {
                    MessageBox.Show("Koniec gry. Wygrały " + (Game.GameStatus == GameStatusType.BlackWin ? "czarne" : "czerwone") + ".");
                }
                else
                {
                    AI.MakeMove(Game);
                    if (Game.GameStatus != GameStatusType.Started && Game.GameStatus != GameStatusType.Initialised)
                    {
                        MessageBox.Show("Koniec gry. Wygrały " + (Game.GameStatus == GameStatusType.BlackWin ? "czarne" : "czerwone") + ".");
                    }
                }
                Connect4Board.Refresh();
            }
        }

        private Tuple<int,int> GetAvailableInRow(int x)
        {
            int MaxWidth = Connect4Board.Width - 2 * margin;
            int UsedWidthPerOne = (int)((MaxWidth - 2 * margin) / (float)Game.Columns);
            int column = 0;
            if (x < margin)
                return null;
            for (int i = margin; i < MaxWidth; i+= UsedWidthPerOne)
            {
                if (x > i && x < i + UsedWidthPerOne)
                {
                    break;
                }
                column++;
            }

            if (column > Game.Board.GetLength(1)-1)
                return null;
            for (int i = 0; i < Game.Board.GetLength(1)-1; i++)
            {
                if (Game.Board[i, column] == FieldType.Empty)
                {
                    return new Tuple<int, int>(i, column);
                }
            }

            return null;
        }

        private void Connect4BoardOnMouseMove(object sender, MouseEventArgs e)
        {
            if(Game.GameStatus != GameStatusType.BlackWin && Game.GameStatus != GameStatusType.RedWin)
                CurrentHover = GetAvailableInRow(e.Location.X);
            Connect4Board.Refresh();
        }
    }
}
