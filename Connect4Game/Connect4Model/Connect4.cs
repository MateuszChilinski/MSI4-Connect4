using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Connect4Model
{
    public enum GameStatusType
    {
        Initialised = 0,
        Started = 1,
        RedWin = 2,
        BlackWin = 3
    }
    public enum FieldType
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
    public interface IConnect4
    {
        bool PutInColumn(int Column, PlayerColor playerColor);
        void RestartGame();
        GameStatusType GameStatus { get; }
        int Columns { get; }
        int Rows { get; }
        FieldType[,] Board { get; }
        PlayerColor CurrentPlayer { get; }
    }
    [ProtoContract]

    public class Connect4 : IConnect4//, ISerializable
    {
        [ProtoMember(1)]
        static int rows = 6;
        [ProtoMember(2)]
        static int columns = 7;
        [ProtoMember(3)]
        public PlayerColor CurrentPlayer { get; protected set; } = PlayerColor.Red;
        [ProtoMember(4)]
        public FieldType[,] Board { get; } = new FieldType[rows, columns];
        [ProtoMember(5)]
        public GameStatusType GameStatus { get; protected set; } = GameStatusType.Initialised;
        public int Columns
        {
            get => columns;
        }
        public int Rows
        {
            get => rows;
        }

        public Connect4(Connect4 oldGame)
        {
            CurrentPlayer = oldGame.CurrentPlayer;
            GameStatus = oldGame.GameStatus;
            Array.Copy(oldGame.Board, Board, Rows*Columns);
        }

        public Connect4()
        {

        }

        public Connect4(List<int> moves)
        {
            foreach (var move in moves)
            {
                PutInColumn(move, CurrentPlayer);
            }
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
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var CurrentFieldType = Board[i, j];
                    if(CurrentFieldType == FieldType.Empty)
                        continue;
                    if (j < columns-3 && CurrentFieldType == Board[i, j + 1] && CurrentFieldType == Board[i, j + 2] &&
                        CurrentFieldType == Board[i, j + 3])
                        return MapFieldToWin(CurrentFieldType);
                    if (i < rows - 3 && CurrentFieldType == Board[i + 1, j] && CurrentFieldType == Board[i + 2, j] &&
                        CurrentFieldType == Board[i + 3, j])
                        return MapFieldToWin(CurrentFieldType);
                    if (j < columns - 3 && i < rows - 3 && CurrentFieldType == Board[i + 1, j + 1] && CurrentFieldType == Board[i + 2, j + 2] &&
                        CurrentFieldType == Board[i + 3, j + 3])
                        return MapFieldToWin(CurrentFieldType);
                    if (i > 2)
                    {
                        if (j < columns - 3 && CurrentFieldType == Board[i - 1, j + 1] && CurrentFieldType == Board[i - 2, j + 2] &&
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
            CurrentPlayer = PlayerColor.Red;
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
