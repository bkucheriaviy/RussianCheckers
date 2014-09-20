using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CheckersCore.Exceptions;

namespace CheckersCore.ActionManagers
{
    public class MoveActionManager : BaseActionManager
    {
        public void Move(Point start, Point target)
        {
            ValidateMove(start, target);
            Gameboard.Cells[target.X, target.Y] = Gameboard.Cells[start.X, start.Y];
            Gameboard.Cells[start.X, start.Y] = null;
        }

        private void ValidateMove(Point start, Point end)
        {
            if (IsEmpty(start)) throw new InvalidMoveException("Nothing to move");
            if (!IsEmpty(end)) throw new InvalidMoveException("The target cell is already busy.");
            if (!IsDiagonalMove(start, end)) throw new InvalidMoveException("Piece can only move diagonally");
        }
        public IEnumerable<Point> GetNearbyPossibleMoves(Point position)
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
}
