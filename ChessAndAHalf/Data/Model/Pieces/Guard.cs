﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model.Pieces
{
    class Guard : Piece
    {
        public Guard(PlayerColor color) : base(color)
        {
        }

        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteGuard.png" : "../../Images/Pieces/blackGuard.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            List<Position> legalMoves = new List<Position>();
            //discutat mutari legale
            return legalMoves;
        }
    }
}