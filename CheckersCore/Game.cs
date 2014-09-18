using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersCore
{
    public class Game
    {
        public Gameboard Gameboard { get; private set; }
        public AttackActionManager AttackActionManager { get; private set; }
        public MoveActionManager MoveActionManager { get; private set; }

        public Game(int rows, int columns)
        {
            Gameboard = new Gameboard(rows, columns);
            AttackActionManager = new AttackActionManager(Gameboard);
            MoveActionManager = new MoveActionManager(Gameboard);
        }



        public void Move(int x, int y, int targetX, int targetY)
        {
            ValidateMove(new Point(x, y), new Point(targetX, targetY));
            Gameboard.Cells[targetX, targetY] = Gameboard.Cells[x, y];
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
    }
    public class Gameboard
    {
        public object[,] Cells { get; private set; }
        public int RowsCount { get; private set; }
        public int ColumnsCount { get; private set; }

        private AddActionManager 
        public Gameboard(int rows, int columns)
        {
            RowsCount = rows;
            ColumnsCount = columns;
            Cells = new object[RowsCount, ColumnsCount];
        }
    }
    public abstract class BaseGameboardActionManager
    {
        public Gameboard Gameboard { get; private set; }

        protected readonly Point[] Moves =
        {
            new Point(1, 1),
            new Point(1, -1),
            new Point(-1, -1),
            new Point(-1, 1)
        };

        protected BaseGameboardActionManager(Gameboard gameboard)
        {
            Gameboard = gameboard;
        }

        protected bool IsEmpty(Point position)
        {
            return Gameboard.Cells[position.X, position.Y] == null;
        }
        
        private bool IsBlack(Point position)
        {
            return (position.X%2 == 0 && position.Y%2 == 0) ||
                   (position.X%2 != 0 && position.Y%2 != 0);
        }
    }

    public class AddActionManager : BaseGameboardActionManager
    {
        public AddActionManager(Gameboard gameboard) : base(gameboard)
        {
        }
        public void AddPiece(int x, int y, string color)
        {
            ValidateAdding(new Point(x, y));
            Gameboard.Cells[x, y] = color;
        }
        public void RemovePiece(Point position)
        {
            if (!IsEmpty(position))
                Gameboard.Cells[position.X, position.Y] = null;
        }

        public void RemoveAllPieces()
        {
            for (int i = 0; i < Gameboard.RowsCount; i++)
                for (int j = 0; j < Gameboard.ColumnsCount; j++)
                    RemovePiece(new Point(i, j));
        }
    }
    public class MoveActionManager : BaseGameboardActionManager
    {
        public MoveActionManager(Gameboard gameboard) : base(gameboard)
        {
        }
        protected IEnumerable<Point> GetNearbyPossibleMoves(Point position)
        {
            return Moves.Select(move => new Point(position.X + move.X, position.Y + move.Y))
                .Where(IsNotOutOfRange);
        }

        protected bool IsNotOutOfRange(Point position)
        {
            return position.X >= 0 &&
                   position.Y >= 0 &&
                   position.X < Gameboard.RowsCount &&
                   position.Y < Gameboard.ColumnsCount;
        }

        private bool IsDiagonalMove(Point start, Point end)
        {
            return Moves.Any(move => end == (new Point(move.X + start.X, move.Y + start.Y)));
        }
        
    }
    public class AttackActionManager : MoveActionManager
    {
        public AttackActionManager(Gameboard gameboard) : base(gameboard)
        {
        }

        public Point DetermineAttackEndpoint(Point aggressor, Point target)
        {
            var vector = GetAttackVector(aggressor, target);

            return new Point(aggressor.X + 2 * vector.X, aggressor.Y + 2 * vector.Y);
        }

        private Point GetAttackVector(Point aggressor, Point target)
        {
            return Moves.Where(m => target == new Point(aggressor.X + m.X, aggressor.Y + m.Y)).First();
        }

        private bool IsVulnerable(Point aggressor, Point target)
        {
            var attackEndoint = DetermineAttackEndpoint(aggressor, target);
            return IsEmpty(attackEndoint);
        }
        private bool IsEnemies(Point start, Point target)
        {
            return !IsEmpty(target) && !IsEmpty(start) &&
                   Gameboard.Cells[start.X, start.Y] != Gameboard.Cells[target.X, target.Y];
        }

        private IEnumerable<Point> GetNearbyVulnurableEnemies(Point position)
        {
            return GetNearbyEnemies(position).Where(enemy => IsVulnerable(position, enemy));
        }

        private IEnumerable<Point> GetNearbyEnemies(Point position)
        {
            return GetNearbyPossibleMoves(position).Where(move => IsEnemies(position, move));
        }

        private IEnumerable<Point> GetAllAlies(Point position)
        {
            var alies = new List<Point>();
            for (int i = 0; i < Gameboard.RowsCount; i++)
            {
                for (int j = 0; j < Gameboard.ColumnsCount; j++)
                {
                    if (Gameboard.Cells[i, j] == Gameboard.Cells[position.X, position.Y] &&
                        !IsEmpty(position))
                    {
                        alies.Add(new Point(i, j));
                    }
                }
            }
            return alies;
        }
        

        public bool IsAnyAliesMustAttack(Point position)
        {
            return GetAllAlies(position).Any(alies => GetNearbyVulnurableEnemies(alies).Any());
        }
    }
}
    