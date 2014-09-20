using System;

namespace CheckersCore.Interfaces
{
    public interface IGameboard
    {
        object[,] Cells { get; }
        int RowsCount { get; }
        int ColumnsCount { get; }
        void RegisterActionManager(IActionManager manager);
        void UnregisterActionManager(Type type);
        IActionManager FindActionManager(Type type);
    }
}
