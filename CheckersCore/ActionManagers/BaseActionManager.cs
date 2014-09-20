using System.Drawing;
using CheckersCore.Interfaces;

namespace CheckersCore.ActionManagers
{
    public abstract class BaseActionManager : IActionManager
    {
        public IGameboard Gameboard { get; set; }

        protected readonly Point[] Moves =
        {
            new Point(1, 1),
            new Point(1, -1),
            new Point(-1, -1),
            new Point(-1, 1)
        };

        protected bool IsEmpty(Point position)
        {
            return Gameboard.Cells[position.X, position.Y] == null;
        }

        protected bool IsBlack(Point position)
        {
            return (position.X % 2 == 0 && position.Y % 2 == 0) ||
                   (position.X % 2 != 0 && position.Y % 2 != 0);
        }
    }
}
