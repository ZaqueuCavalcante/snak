namespace Game;

public class GameState
{
    public int Rows { get; }
    public int Columns { get; }
    public CellType[,] Grid { get; }

    public Direction Player { get; private set; }
    public int Score { get; private set; }
    public bool GameOver { get; private set; }

    private readonly Random _random = new();
    private readonly LinkedList<Position> _snakePositions = [];

	private readonly LinkedList<Direction> _commands = [];

	public GameState(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Grid = new CellType[rows, columns];
        Player = Direction.Right;

        AddSnake();
        AddFood();
    }

    private void AddSnake()
    {
        int middleRow = Rows / 2;
        for (int column = 1; column <= 3; column++)
        {
            Grid[middleRow, column] = CellType.Snake;
            _snakePositions.AddFirst(new Position(middleRow, column));
        }
    }

    private IEnumerable<Position> GetEmptyPositions()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (Grid[row, column] == CellType.Empty)
                {
                    yield return new(row, column);
                }
            }
        }
    }

    private void AddFood()
    {
        var emptyPositions = GetEmptyPositions().ToList();
        var total = emptyPositions.Count ;

        if (total == 0) return;

        var position = emptyPositions[_random.Next(total)];
        Grid[position.Row, position.Column] = CellType.Food;
    }

    public Position GetSnakeHead()
    {
        return _snakePositions.First!.Value;
    }

    public Position GetSnakeTail()
    {
        return _snakePositions.Last!.Value;
    }

    public IEnumerable<Position> GetSnake()
    {
        return _snakePositions;
    }

    private void AddSnakeHead(Position position)
    {
        _snakePositions.AddFirst(position);
        Grid[position.Row, position.Column] = CellType.Snake;
    }

    private void RemoveSnakeTail()
    {
        var tailPosition = GetSnakeTail();
        Grid[tailPosition.Row, tailPosition.Column] = CellType.Empty;
        _snakePositions.RemoveLast();
    }

    private Direction GetSnakeLastDirection()
    {
        if (_commands.Count == 0) return Player;

        return _commands.Last!.Value;
    }

	private bool CanChangeDirection(Direction newDirection)
	{
		if (_commands.Count == 2) return false;

        var lastDirection = GetSnakeLastDirection();
		return newDirection != lastDirection && newDirection != lastDirection.Opposite();
	}

	public void MoveTo(Direction direction)
    {
        if (CanChangeDirection(direction))
        {
            _commands.AddLast(direction);  
        }
    }

    public void MoveSnake()
    {
        if (_commands.Count > 0)
        {
            Player = _commands.First!.Value;
            _commands.RemoveFirst();
        }

        var newHeadPosition = GetSnakeHead().MoveTo(Player);
        var targetCell = WillHit(newHeadPosition);

        if (targetCell == CellType.Outside || targetCell == CellType.Snake)
        {
            GameOver = true;
            return;
        }

        if (targetCell == CellType.Empty)
        {
            RemoveSnakeTail();
            AddSnakeHead(newHeadPosition);
            return;
        }

        if (targetCell == CellType.Food)
        {
            AddSnakeHead(newHeadPosition);
            Score++;
            AddFood();
        }
    }

    private bool IsOutsideGrid(Position position)
    {
        return position.Row < 0 || position.Row >= Rows || position.Column < 0 || position.Column >= Columns;
    }

    private CellType WillHit(Position newHeadPosition)
    {
        if (IsOutsideGrid(newHeadPosition)) return CellType.Outside;

        if (newHeadPosition == GetSnakeTail()) return CellType.Empty;

        return Grid[newHeadPosition.Row, newHeadPosition.Column];
    }
}
