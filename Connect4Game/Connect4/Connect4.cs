using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Game
{
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
        Black = 2,
        Projection = 3
    };
    public enum PlayerColor
    {
        Red = 0,
        Black = 1
    }
    interface IConnect4
    {
        bool PutInColumn(int Column, PlayerColor playerColor);
        void RestartGame();
        GameStatusType GameStatus { get; }
        int Columns { get; }
        int Rows { get; }
        FieldType[,] Board { get; }
    }
        
    class Connect4 : IConnect4
    {
        const int rows = 6;
        const int columns = 7;
        public PlayerColor CurrentPlayer = PlayerColor.Red;
        public FieldType[,] Board { get; } = new FieldType[rows, columns];
        public GameStatusType GameStatus { get; protected set; } = GameStatusType.Initialised;
        public int Columns
        {
            get => columns;
        }
        public int Rows
        {
            get => rows;
        }

        private GameStatusType MapFieldToWin(FieldType fieldType)
        {
            if (fieldType == FieldType.Black)
                return GameStatusType.BlackWin;
            else
                return GameStatusType.RedWin;
        }

        private GameStatusType? CheckIfWin()
        {
            for (int i = 0; i < rows-3; i++)
            {
                for (int j = 0; j < columns-3; j++)
                {
                    var CurrentFieldType = Board[i, j];
                    if(CurrentFieldType == FieldType.Empty)
                        continue;
                    if (CurrentFieldType == Board[i, j + 1] && CurrentFieldType == Board[i, j + 2] &&
                        CurrentFieldType == Board[i, j + 3])
                        return MapFieldToWin(CurrentFieldType);
                    if (CurrentFieldType == Board[i + 1, j] && CurrentFieldType == Board[i + 2, j] &&
                        CurrentFieldType == Board[i + 3, j])
                        return MapFieldToWin(CurrentFieldType);
                    if (CurrentFieldType == Board[i + 1, j + 1] && CurrentFieldType == Board[i + 2, j + 2] &&
                        CurrentFieldType == Board[i + 3, j + 3])
                        return MapFieldToWin(CurrentFieldType);
                    if (i > 3)
                    {
                        if (CurrentFieldType == Board[i - 1, j + 1] && CurrentFieldType == Board[i - 2, j + 2] &&
                            CurrentFieldType == Board[i - 3, j + 3])
                            return MapFieldToWin(CurrentFieldType);
                    }
                }
            }
            return null;
        }

        public bool PutInColumn(int Column, PlayerColor playerColor)
        {
            if (playerColor != CurrentPlayer)
                return false;
            if (GameStatus == GameStatusType.RedWin || GameStatus == GameStatusType.BlackWin) // if game has finished one cannot put more
                return false;
            if (Board[Board.GetLength(0) - 1, Column] != FieldType.Empty) // if column is full, not possible
                return false;
            GameStatus = GameStatusType.Started;
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                if (Board[i, Column] == FieldType.Empty)
                {
                    if (playerColor == PlayerColor.Red)
                        Board[i, Column] = FieldType.Red;
                    else
                        Board[i, Column] = FieldType.Black;
                    break;
                }
            }

            var WinStatus = CheckIfWin();

            if (WinStatus != null)
            {
                GameStatus = (GameStatusType) WinStatus;
            }

            CurrentPlayer = CurrentPlayer == PlayerColor.Red ? PlayerColor.Black : PlayerColor.Red;

            return true;
        }

        public void RestartGame()
        {
            GameStatus = GameStatusType.Initialised;
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    Board[i, j] = FieldType.Empty;
                }
            }
        }
    }
}
