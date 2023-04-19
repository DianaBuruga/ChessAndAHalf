namespace ChessAndAHalf.Data.Model
{
    public class Square
    {
        public Position Position { get; set; }
        public Piece Occupant { get; set; }
        public bool IsHighlighted { get; set; }
        public bool IsCaptured { get; set; }

        public Square(int row, int column, Piece occupant = null)
        {
            Position = new Position(row, column);
            Occupant = occupant;
            IsHighlighted = false;
            IsCaptured = false;
        }

        public int GetRow()
        {
            return Position.Row;
        }

        public int GetColumn()
        {
            return Position.Column;
        }
    }
}
