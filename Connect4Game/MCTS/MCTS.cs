using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Connect4Model;
using ProtoBuf;

namespace MCTS
{
    [ProtoContract]
    public class Node
    {
        //[ProtoMember(1)]
        public Node Parent = null;
        [ProtoMember(2)]
        public int TimesWon = 0;
        [ProtoMember(3)]
        public int TimesVisited = 0;
        [ProtoMember(4)]
        public int columnChosen;
        [ProtoMember(5)]
        public List<Node> Nodes { get; set; } = new List<Node>();
        
        public Connect4 GameState
        {
            get
            {
                var list = GetMoves(this);
                list.Reverse();
                return new Connect4(list);
            }
        }
        public List<int> GetMoves(Node node)
        {
            List<int> moveList = new List<int>();
            while (node.Parent != null)
            {
                moveList.Add(node.columnChosen);
                node = node.Parent;
            }

            return moveList;
        }
    }

    public interface IMCTSAI
    {
        void MakeMove(IConnect4 Game);
    }
    public class MCTSAI : IMCTSAI
    {
        private Node Root = new Node();
        private bool train;
        private PlayerColor AIColour;
        private GameStatusType GameWon
        {
            get => AIColour == PlayerColor.Black ? GameStatusType.BlackWin : GameStatusType.RedWin;
        }
        private double C = 0.5;
        private const int it = 100000000;
        Random rnd = new Random();
        public MCTSAI(bool _train = false, PlayerColor _aicolor = PlayerColor.Black)
        {
            train = _train;
            AIColour = _aicolor;

            Stream stream;

            if (train)
            {
                Root = new Node();
                for (int i = 0; i < it; i++)
                {
                    Selection(Root);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write("Generowanie modelu: "+ ((double)i/it)*100.0 + "%");
                }

                var file = File.Create("model.bin");
                    Serializer.Serialize(file, Root);
            }
            else
            {

                stream = new FileStream("model.bin", FileMode.OpenOrCreate, FileAccess.Read);
                if (stream.Length != 0)
                    Root = Serializer.Deserialize<Node>(stream);
                RestoreParents(Root, null);
                stream.Close();
                if (_aicolor == PlayerColor.Red)
                {
                    //switch win/lose
                    SwitchWinLose(Root);
                }
            }
        }

        private void SwitchWinLose(Node node)
        {
            node.TimesWon = node.TimesVisited - node.TimesWon;
            foreach(var child in node.Nodes)
                SwitchWinLose(child);
        }

        private void RestoreParents(Node root, Node parent)
        {
            root.Parent = parent;
            foreach(var node in root.Nodes)
                RestoreParents(node, root);
        }

        private bool AreBoardsEqual(FieldType[,] data1, FieldType[,] data2)
        {
            var equal =
                data1.Rank == data2.Rank &&
                Enumerable.Range(0, data1.Rank).All(dimension => data1.GetLength(dimension) == data2.GetLength(dimension)) &&
                data1.Cast<FieldType>().SequenceEqual(data2.Cast<FieldType>());
            return equal;
        }
        public void MakeMove(IConnect4 Game)
        {
            if (!AreBoardsEqual(Game.Board, Root.GameState.Board))
            {
                foreach (var node in Root.Nodes)
                {
                    if (AreBoardsEqual(node.GameState.Board,Game.Board))
                        Root = node;
                }
            }
            if(!AreBoardsEqual(Game.Board, Root.GameState.Board)) throw new Exception("I got lost :(");
            // use some time to expand tree
            Stopwatch s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(2))
            {
                Selection(Root);
            }
            s.Stop();
            var bestNode = Root.Nodes.Aggregate((i, j) => i.TimesWon > j.TimesWon ? i : j);
            Root = bestNode;
            Game.PutInColumn(bestNode.columnChosen, AIColour);
        }

        private void Selection(Node node)
        {
            if (node.Nodes.Count == 0)
            {
                Expansion(node);
                return;
            }

            Node bestChild = node.Nodes[0];
            double UCBBestChild = CalculateUCB(bestChild);
            foreach (Node child in node.Nodes)
            {
                double UCB = CalculateUCB(child);
                if (UCBBestChild < UCB)
                {
                    bestChild = child;
                    UCBBestChild = UCB;
                }
            }
            Selection(bestChild);
        }

        private void Expansion(Node node)
        {
            if (node.GameState.GameStatus != GameStatusType.BlackWin &&
                node.GameState.GameStatus != GameStatusType.RedWin)
            {
                List<Node> winners = new List<Node>();

                var newNodes = GetAvailableMoves(node.GameState);
                foreach (int column in newNodes)
                {
                    Connect4 updatedGame = new Connect4((Connect4)node.GameState);
                    updatedGame.PutInColumn(column, updatedGame.CurrentPlayer);
                    Node newNode = new Node();
                    newNode.columnChosen = column;
                    newNode.Parent = node;
                    node.Nodes.Add(newNode);
                    if(updatedGame.GameStatus == GameWon)
                        winners.Add(node);
                }

                if (winners.Count > 0)
                {
                    Node winnerNode = winners[0];
                    Simulation(winnerNode);
                    return;
                }

                var losing = GameWon == GameStatusType.BlackWin ? GameStatusType.RedWin : GameWon;
                foreach (var newNode in node.Nodes)
                {
                    if (newNode.GameState.GameStatus == losing)
                        winners.Add(node);
                }


                if (winners.Count > 0)
                {
                    Node winnerNode = winners[0];
                    Simulation(winnerNode);
                    return;
                }

                Node randomNode = node.Nodes[rnd.Next(0, node.Nodes.Count)];
                Simulation(randomNode);
            }
            else
            {
                IConnect4 gameToSimulate = new Connect4((Connect4)node.GameState);
                Backpropagation(node, gameToSimulate.GameStatus == GameWon);
            }
        }

        private List<int> GetAvailableMoves(IConnect4 Game)
        {
            List<int> AvailableMoves = new List<int>();
            var Board = Game.Board;
            for (int i = 0; i < Board.GetLength(1); i++)
            {
                if (Board[Game.Rows-1, i] == FieldType.Empty)
                    AvailableMoves.Add(i);
            }

            return AvailableMoves;
        }

        private void MakeSmartMove(IConnect4 Game)
        {
            List<int> availableMoves = GetAvailableMoves(Game);
            foreach (var move in availableMoves)
            {
                var newGame = new Connect4((Connect4)Game);
                newGame.PutInColumn(move, Game.CurrentPlayer);
                if (newGame.GameStatus == GameStatusType.BlackWin || newGame.GameStatus == GameStatusType.RedWin)
                {
                    Game.PutInColumn(move, Game.CurrentPlayer);
                    return;
                }
            }
            MakeRandomMove(Game);
        }
        private void Simulation(Node node)
        {
            IConnect4 gameToSimulate = new Connect4((Connect4)node.GameState);
            while (gameToSimulate.GameStatus != GameStatusType.BlackWin &&
                   gameToSimulate.GameStatus != GameStatusType.RedWin && GetAvailableMoves(gameToSimulate).Count > 0)
            {
                MakeSmartMove(gameToSimulate);
            }

            bool won = gameToSimulate.GameStatus == GameWon;
            Backpropagation(node, won);
        }
        private void MakeSimulationMove(IConnect4 Game, int column)
        {
            Game.PutInColumn(column, Game.CurrentPlayer);
        }
        private void MakeRandomMove(IConnect4 Game)
        {
            List<int> availableMoves = GetAvailableMoves(Game);
            var randomColumn = availableMoves[rnd.Next(0, availableMoves.Count)];
            Game.PutInColumn(randomColumn, Game.CurrentPlayer);
        }

        private void Backpropagation(Node node, bool won)
        {
            Node currentNode = node;
            while (currentNode != null)
            {
                currentNode.TimesVisited++;
                if (won)
                    currentNode.TimesWon++;
                currentNode = currentNode.Parent;
            }
        }

        private double CalculateUCB(Node v)
        {
            if (v.TimesVisited == 0)
                return double.PositiveInfinity;
            return (double)v.TimesWon/v.TimesVisited + C * Math.Sqrt(Math.Log(v.Parent.TimesVisited) / v.TimesVisited);
        }
    }
}
