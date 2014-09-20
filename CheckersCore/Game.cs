using System.Drawing;
using CheckersCore.ActionManagers;
using CheckersCore.Exceptions;
using CheckersCore.Interfaces;

namespace CheckersCore
{
    public class Game
    {
        public IGameboard Gameboard { get; private set; }

        public AddRemoveActionManager AddRemoveAction
        {
            get
            {
                return (AddRemoveActionManager) Gameboard.FindActionManager(typeof (AddRemoveActionManager));
            }
        }

        public MoveActionManager MoveAction
        {
            get
            {
                return (MoveActionManager) Gameboard.FindActionManager(typeof (MoveActionManager));
            }
        }

        public AttackActionManager AttackAction
        {
            get
            {
                return (AttackActionManager) Gameboard.FindActionManager(typeof (AttackActionManager));
            }
        }

        public Game(int rows, int columns)
        {
            Gameboard = new Gameboard(rows, columns);
            
            Gameboard.RegisterActionManager(new AddRemoveActionManager());
            Gameboard.RegisterActionManager(new MoveActionManager());
            Gameboard.RegisterActionManager(new AttackActionManager());
        }

        public void AddPiece(int x, int y, string color)
        {
            AddRemoveAction.AddPiece(new Point(x, y), color);
        }

        public void Move(int x, int y, int targetX, int targetY)
        {
            if (AttackAction.IsAnyAliesMustAttack(new Point(x, y)))
                throw new InvalidMoveException("You shoud attack first");

            MoveAction.Move(new Point(x, y), new Point(targetX, targetY));
        }

        public void Attack(int x, int y, int targetX, int targetY)
        {
            AttackAction.Attack(new Point(x, y), new Point(targetX, targetY));
        }
    }
}
    