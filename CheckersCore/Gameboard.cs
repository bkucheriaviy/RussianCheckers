using System;
using System.Collections.Generic;
using System.Linq;
using CheckersCore.Interfaces;

namespace CheckersCore
{
    public class Gameboard : IGameboard
    {
        public object[,] Cells { get; private set; }
        public int RowsCount { get; private set; }
        public int ColumnsCount { get; private set; }

        private readonly HashSet<IActionManager> _actionManagers = new HashSet<IActionManager>();

        public Gameboard(int rows, int columns)
        {
            RowsCount = rows;
            ColumnsCount = columns;
            Cells = new object[RowsCount, ColumnsCount];
        }

        public void RegisterActionManager(IActionManager manager)
        {
            manager.Gameboard = this;
            _actionManagers.Add(manager);
        }

        public void UnregisterActionManager(Type type)
        {
            
            _actionManagers.RemoveWhere(m => m.GetType() == type);
        }

        public IActionManager FindActionManager(Type type)
        {
            var manager =_actionManagers.FirstOrDefault(m => m.GetType() == type);
            if (manager == null) 
                throw new Exception("There is no such registered managers");
            return manager;
        }
    }
}
