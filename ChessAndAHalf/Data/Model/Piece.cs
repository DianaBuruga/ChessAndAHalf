﻿using System.Collections.Generic;

namespace ChessAndAHalf.Data.Model
{
    public abstract class Piece
    {
        public bool IsFirstMove { get; set; }
        public List<Position> Captures {get; protected set;}
        public PlayerColor Color { get; set; }
        public abstract string GetImagePath();
        public abstract List<Position> GetLegalMoves(Board board, Square currentSquare);
        public abstract int Points { get; }

        public Piece(PlayerColor color)
        {
            Color = color;
        }
    }
}
