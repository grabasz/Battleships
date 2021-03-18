namespace Domain.Battleships
{
    public class Coordinate
    {
        public Coordinate(string row, string column)
        {
            Row = row;
            Column = column;
        }

        public string Row { get; set; }
        public string Column { get; set; }

        public int RowToIndex => Row.ToUpper()[0] % 32 - 1;
        public int ColumnToIndex => int.Parse(Column) - 1;
        public override bool Equals(object obj)
        {
            if ((obj is Coordinate coordinate))
            {
                return coordinate.Row == Row && coordinate.Column == Column;
            }

            return false;
        }
    }
}