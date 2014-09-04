using System;
using CheckersCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CheckersTests
{
    [TestClass]
    public class CheckersTest
    {
        private Game _game;

        [TestInitialize]
        public void SetUp()
        {
            _game = new Game(8, 8);
        }

        [TestMethod]
        public void CanAddPieceOnBoard()
        {
            _game.RemoveAllPieces();
            _game.AddPiece(1, 1, "w");
            Assert.AreEqual(_game.Gameboard[1, 1], "w");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidAddException))]
        public void CantAddPieceOnWhiteCell()
        {
            _game.RemoveAllPieces();
            _game.AddPiece(1, 2, "w");
        }

        [TestMethod]
        public void CanMakeMove()
        {
            _game.RemoveAllPieces();
            _game.AddPiece(1, 1, "w");
            _game.Move(1, 1, 2, 2);
            Assert.AreEqual(_game.Gameboard[1, 1], null);
            Assert.AreEqual(_game.Gameboard[2, 2], "w");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidMoveException))]
        public void CantMoveEmptyCell()
        {
            _game.RemoveAllPieces();
            _game.Move(1, 1, 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidMoveException))]
        public void CantMoveOnBusyCell()
        {
            _game.RemoveAllPieces();
            _game.AddPiece(1, 1, "w");
            _game.AddPiece(2, 2, "w");
            _game.Move(1, 1, 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidMoveException))]
        public void CantMoveNotDiagonally()
        {
            _game.RemoveAllPieces();
            _game.AddPiece(1, 1, "w");
            _game.Move(1, 1, 1, 3);
        }

        [TestMethod]
        public void CanAttack()
        {
            _game.RemoveAllPieces();
            _game.AddPiece(1, 1, "w");
            _game.AddPiece(2, 2, "b");
            _game.Attack(1, 1, 2, 2);

            Assert.AreEqual(null, _game.Gameboard[1, 1]);
            Assert.AreEqual(null, _game.Gameboard[2, 2]);
            Assert.AreEqual("w", _game.Gameboard[3, 3]);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidMoveException))]
        public void CantMoveWhenYouMustToAttack()
        {
            _game.RemoveAllPieces();
            _game.AddPiece(1, 1, "w");
            _game.AddPiece(1, 3, "w");
            _game.AddPiece(2, 2, "b");
            _game.Move(1, 3, 2, 4);
        }
    }
}
