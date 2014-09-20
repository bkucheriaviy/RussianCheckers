using System.Drawing;
using CheckersCore.Exceptions;

namespace CheckersCore.ActionManagers
{
    public class AddRemoveActionManager : BaseActionManager
    {
        public void AddPiece(Point target, string color)
        {
            ValidateAdding(target);
            Gameboard.Cells[target.X, target.Y] = color;
        }

        public void ValidateAdding(Point position)
        {
            if (!IsBlack(position)) throw new InvalidAddException("The target cell can be only black");
            if (!IsEmpty(position)) throw new InvalidAddException("The target cell is already busy.");
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
}
