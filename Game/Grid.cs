namespace Game;

public class Grid
{
    private readonly int _rows;
    private readonly int _columns;
    private readonly CellType[,] _cells;
    private readonly Random _random = new();

	public Grid(int rows, int columns)
    {
        _rows = rows;
        _columns = columns;
        _cells = new CellType[rows, columns];
    }

    public CellType[,] Get() => _cells;

    public void AddFood()
    {
        var emptyPositions = GetEmptyPositions();

        var total = emptyPositions.Count;
        if (total == 0) return;

        var position = emptyPositions[_random.Next(total)];
        _cells[position.Row, position.Column] = CellType.Food;
    }

    private List<Position> GetEmptyPositions()
    {
        var positions = new List<Position>();
        for (int row = 0; row < _rows; row++)
        {
            for (int column = 0; column < _columns; column++)
            {
                if (_cells[row, column] == CellType.Empty)
                {
                    positions.Add(new(row, column));
                }
            }
        }
        return positions;
    }

    public bool IsOutside(Position position)
    {
        return position.Row < 0 || position.Row >= _rows || position.Column < 0 || position.Column >= _columns;
    }
}
