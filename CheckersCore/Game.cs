﻿using System;
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

        private readonly Point[] _moves =
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
            ValidateAdding(new Point(x, y));
            Gameboard[x, y] = color;
        }

        public void Move(int x, int y, int targetX, int targetY)
        {
            ValidateMove(new Point(x, y), new Point(targetX, targetY));
            Gameboard[targetX, targetY] = Gameboard[x, y];
            RemovePiece(new Point(x, y));
        }

        public void Attack(int x, int y, int targetX, int targetY)
        {
            ValidateAttack(new Point(x, y), new Point(targetX, targetY));

            var attackEndpoint = DetermineAttackEndpoint(new Point(x, y), new Point(targetX, targetY));

            Gameboard[attackEndpoint.X, attackEndpoint.Y] = Gameboard[x, y];
            RemovePiece(new Point(x, y));
            RemovePiece(new Point(targetX, targetY));

        }

        public void RemovePiece(Point position)
        {
            if (!IsEmpty(position))
                Gameboard[position.X, position.Y] = null;
        }

        public void RemoveAllPieces()
        {
            for (int i = 0; i < Gameboard.GetLength(0); i++)
                for (int j = 0; j < Gameboard.GetLength(1); j++)
                    RemovePiece(new Point(i, j));
        }

        public bool IsValidMove(Point start, Point end)
        {
            try
            {
                ValidateMove(start, end);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ValidateAdding(Point position)
        {
            if (!IsBlack(position)) throw new InvalidAddException("The target cell can be only black");
            if (!IsEmpty(position)) throw new InvalidAddException("The target cell is already busy.");
        }

        private void ValidateMove(Point start, Point end)
        {
            if (IsEmpty(start)) throw new InvalidMoveException("Nothing to move");
            if (!IsEmpty(end)) throw new InvalidMoveException("The target cell is already busy.");
            if (!IsDiagonalMove(start, end)) throw new InvalidMoveException("Piece can only move diagonally");
            if (IsAnyAliesMustAttack(start)) throw new InvalidMoveException("One of the allies must to attack.");
        }

        private void ValidateAttack(Point aggressor, Point target)
        {
            if (!IsEnemies(aggressor, target)) throw new InvalidTargetException("The target is not the enemy");
            if (!IsVulnerable(aggressor, target)) throw new InvalidTargetException("The target is invulnerable.");
        }

        public bool IsAnyAliesMustAttack(Point position)
        {
            return GetAllAlies(position).Any(alies => GetNearbyVulnurableEnemies(alies).Any());
        }

        private IEnumerable<Point> GetAllAlies(Point position)
        {
            var alies = new List<Point>();
            for (int i = 0; i < Gameboard.GetLength(0); i++)
            {
                for (int j = 0; j < Gameboard.GetLength(1); j++)
                {
                    if (Gameboard[i, j] == Gameboard[position.X, position.Y] &&
                        !IsEmpty(position))
                    {
                        alies.Add(new Point(i, j));
                    }
                }
            }
            return alies;
        }

        private IEnumerable<Point> GetNearbyVulnurableEnemies(Point position)
        {
            return GetNearbyEnemies(position).Where(enemy => IsVulnerable(position, enemy));
        }

        private IEnumerable<Point> GetNearbyEnemies(Point position)
        {
            return GetNearbyPossibleMoves(position).Where(move => IsEnemies(position, move));
        }

        private bool IsEmpty(Point position)
        {
            return Gameboard[position.X, position.Y] == null;
        }

        private bool IsBlack(Point position)
        {
            return (position.X%2 == 0 && position.Y%2 == 0) ||
                   (position.X%2 != 0 && position.Y%2 != 0);
        }

        private bool IsDiagonalMove(Point start, Point end)
        {
            return _moves.Any(move => end == (new Point(move.X + start.X, move.Y + start.Y)));
        }

        private bool IsEnemies(Point start, Point target)
        {
            return !IsEmpty(target) && !IsEmpty(start) &&
                   Gameboard[start.X, start.Y] != Gameboard[target.X, target.Y];
        }

        private bool IsVulnerable(Point aggressor, Point target)
        {
            var attackEndoint = DetermineAttackEndpoint(aggressor, target);
            return IsEmpty(attackEndoint);
        }

        private Point DetermineAttackEndpoint(Point aggressor, Point target)
        {
            var vector = GetAttackVector(aggressor, target);

            return new Point(aggressor.X + 2*vector.X, aggressor.Y + 2*vector.Y);
        }

        private Point GetAttackVector(Point aggressor, Point target)
        {
            return _moves.Where(m => target == new Point(aggressor.X + m.X, aggressor.Y + m.Y)).First();
        }

        private IEnumerable<Point> GetNearbyPossibleMoves(Point position)
        {
            return _moves.Select(move => new Point(position.X + move.X, position.Y + move.Y))
                .Where(IsNotOutOfRange);
        }

        private bool IsNotOutOfRange(Point position)
        {
            return position.X >= 0 &&
                   position.Y >= 0 &&
                   position.X < Gameboard.GetLength(0) &&
                   position.Y < Gameboard.GetLength(1);
        }
    }

    public abstract class BaseRulesManager
    {
        
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

}
    