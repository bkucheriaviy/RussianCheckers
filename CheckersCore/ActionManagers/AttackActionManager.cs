using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckersCore.Exceptions;

namespace CheckersCore.ActionManagers
{
    public class AttackActionManager : BaseActionManager
    {
        private MoveActionManager MoveAction
        {
            get
            {
                return (MoveActionManager)Gameboard.FindActionManager(typeof(MoveActionManager));
            }
        }

        public void Attack(Point aggressor, Point target)
        {
            ValidateAttack(aggressor, target);

            var attackEndpoint = DetermineAttackEndpoint(aggressor, target);

            Gameboard.Cells[attackEndpoint.X, attackEndpoint.Y] = Gameboard.Cells[aggressor.X, aggressor.X];
            Gameboard.Cells[aggressor.X, aggressor.Y] = null;
            Gameboard.Cells[target.X, target.Y] = null;
        }

        private void ValidateAttack(Point aggressor, Point target)
        {
            if (!IsEnemies(aggressor, target)) throw new InvalidTargetException("The target is not the enemy");
            if (!IsVulnerable(aggressor, target)) throw new InvalidTargetException("The target is invulnerable.");
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
            return MoveAction.GetNearbyPossibleMoves(position).Where(move => IsEnemies(position, move));
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
