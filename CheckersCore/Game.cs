using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersCore
{
    public class Game
    {
        public string[,] Gameboard { get; private set; }

        private readonly Point[] _diagonalMoves =
        {
            new Point(1, 1),
            new Point(1, -1),
            new Point(-1, -1),
            new Point(-1, 1)
        };

        public Game(int width, int height)
        {
            Gameboard = new string[width, height];
        }

        public void AddPiece(int x, int y, string color)
        {
            ValidateAdding(x, y);
            Gameboard[x, y] = color;
        }

        public void Move(int x, int y, int targetX, int targetY)
        {
            ValidateMove(new Point(x, y), new Point(targetX, targetY));
            Gameboard[targetX, targetY] = Gameboard[x, y];
            RemovePiece(x, y);
        }

        public void Attack(int x, int y, int targetX, int targetY)
        {
            ValidateAttack(new Point(x, y), new Point(targetX, targetY));

            var endPoint = GetAttackEndpoint(new Point(x, y), new Point(targetX, targetY));

            Gameboard[endPoint.X, endPoint.Y] = Gameboard[x, y];
            RemovePiece(targetX, targetY);
            RemovePiece(x, y);
        }

        public void RemovePiece(int x, int y)
        {
            if (!IsEmptyCell(x, y))
                Gameboard[x, y] = null;
        }

        public void RemoveAllPieces()
        {
            for (int i = 0; i < Gameboard.GetLength(0); i++)
                for (int j = 0; j < Gameboard.GetLength(1); j++)
                    RemovePiece(i, j);
        }

        private void ValidateAdding(int x, int y)
        {
            if (!IsBlackCell(x, y)) throw new InvalidAddException("The target cell can be only black");
            if (!IsEmptyCell(x, y)) throw new InvalidAddException("The target cell is already busy.");
        }

        private void ValidateMove(Point start, Point end)
        {
            if (IsEmptyCell(start.X, start.Y)) throw new InvalidMoveException("Nothing to move");
            if (!IsEmptyCell(end.X, end.Y)) throw new InvalidMoveException("The target cell is already busy.");
            if (!IsDiagonalMove(start, end)) throw new InvalidMoveException("Piece can only move diagonally");
            if (MustAttack(Gameboard[start.X, start.Y]))
                throw new InvalidMoveException("One of the pieces must to attack.");
        }

        private void ValidateAttack(Point aggressor, Point target)
        {
            if (!IsEnemies(aggressor, target)) throw new InvalidTargetException("Target point is not the enemy");

            var endPoint = GetAttackEndpoint(aggressor, target);

            if (!IsEmptyCell(endPoint.X, endPoint.Y))
                throw new InvalidTargetException("Pice can't be attacked. Endpoint is not free");
        }

        private bool MustAttack(string pieceColor)
        {
            for (int i = 0; i < Gameboard.GetLength(0); i++)
            {
                for (int j = 0; j < Gameboard.GetLength(1); j++)
                {
                    if (Gameboard[i, j] == pieceColor && Gameboard[i, j] != null)
                    {
                        if (GetNearbyVulnurableEnemies(new Point(i, j)).Any()) return true;
                    }
                }
            }
            return false;
        }

        private bool IsEmptyCell(int x, int y)
        {
            return Gameboard[x, y] == null;
        }

        private bool IsBlackCell(int x, int y)
        {
            return (x%2 == 0 && y%2 == 0) || (x%2 != 0 && y%2 != 0);
        }

        private bool IsDiagonalMove(Point start, Point end)
        {
            return _diagonalMoves.Any(move => end == (new Point(move.X + start.X, move.Y + start.Y)));
        }

        private bool IsEnemies(Point start, Point target)
        {
            return !IsEmptyCell(target.X, target.Y) && !IsEmptyCell(start.X, start.Y) &&
                   Gameboard[start.X, start.Y] != Gameboard[target.X, target.Y];
        }

        private bool IsNotOutOfRange(Point point)
        {
            return point.X >= 0 &&
                   point.Y >= 0 &&
                   point.X < Gameboard.GetLength(0) &&
                   point.Y < Gameboard.GetLength(1);
        }

        private IEnumerable<Point> GetNearbyVulnurableEnemies(Point start)
        {
            return
                GetNearbyEnemies(start)
                    .Select(enemy => GetAttackEndpoint(start, enemy))
                    .Where(endPoint => IsEmptyCell(endPoint.X, endPoint.Y));
        }

        private IEnumerable<Point> GetNearbyEnemies(Point start)
        {
            return GetNearbyPossibleMoves(start)
                .Where(move => IsEnemies(start, move));
        }

        private IEnumerable<Point> GetNearbyPossibleMoves(Point start)
        {
            return _diagonalMoves.Select(move => new Point(start.X + move.X, start.Y + move.Y))
                .Where(IsNotOutOfRange);
        }

        private Point GetAttackEndpoint(Point aggressor, Point target)
        {
            var vector = GetAttackVector(aggressor, target).First();

            return new Point(aggressor.X + 2*vector.X, aggressor.Y + 2*vector.Y);
        }

        private IEnumerable<Point> GetAttackVector(Point start, Point target)
        {
            return _diagonalMoves.Where(m => target == new Point(start.X + m.X, start.Y + m.Y));
        }
    }

    public class InvalidAddException : Exception
    {
        public InvalidAddException(string message) : base(message)
        {

        }
    }

    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message) : base(message)
        {
            
        }
    }

    public class InvalidTargetException : Exception
    {
        public InvalidTargetException(string message) : base(message)
        {
            
        }
    }

    public class Cheker:IChecker
    {
        public CheckerColor Color { get; private set; }

        public Gameboard Gameboard { get; private set; }

        public Point Position { get; private set; }

        public void Attack(Point target)
        {
            throw new NotImplementedException();
        }

        public void Move(Point target)
        {
            throw new NotImplementedException();
        }
    }

    public interface IChecker
    {
         CheckerColor Color { get; }

        IGameboard Gameboard { get; }

        Point Position { get; }

        void Attack(Point target,IRules rules);

        void Move(Point target,IRule);
    }

    public interface IGameboard
    {
    }

    public enum CheckerColor
    {
        White,
        Black
    }
}
    