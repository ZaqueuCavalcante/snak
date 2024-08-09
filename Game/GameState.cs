namespace Game;

public class GameState
{
    public int Rows { get; }
    public int Columns { get; }
    public Grid Grid { get; }


    public int Score { get; private set; }
    public bool GameOver { get; private set; }
    public Direction SnakeDirection { get; private set; }

	private readonly LinkedList<Direction> _commands = [];
    private readonly LinkedList<Position> _snakePositions = [];

	public GameState(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Grid = new Grid(rows, columns);
        SnakeDirection = Direction.Right;

        AddSnake();
        Grid.AddFood();
    }

    private void AddSnake()
    {
        int middleRow = Rows / 2;
        for (int column = 2; column >= 1; column--)
        {
            Grid.Get()[middleRow, column] = CellType.Snake;
            _snakePositions.AddLast(new Position(middleRow, column));
        }
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
        Grid.Get()[position.Row, position.Column] = CellType.Snake;
    }

    private void RemoveSnakeTail()
    {
        var tailPosition = GetSnakeTail();
        Grid.Get()[tailPosition.Row, tailPosition.Column] = CellType.Empty;
        _snakePositions.RemoveLast();
    }

    private Direction GetSnakeLastDirection()
    {
        if (_commands.Count == 0) return SnakeDirection;

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
            SnakeDirection = _commands.First!.Value;
            _commands.RemoveFirst();
        }

        var newHeadPosition = GetSnakeHead().MoveTo(SnakeDirection);
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
            Grid.AddFood();
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

        return Grid.Get()[newHeadPosition.Row, newHeadPosition.Column];
    }
}
