namespace Domain.Battleships
{
    public class Coordinate
    {
        public string Row { get; set; }
        public string Column { get; set; }

        public int RowToIndex => int.Parse(Row) - 1;
        public int ColumnToIndex => Column.ToUpper()[0] %32  - 1;

    }
}