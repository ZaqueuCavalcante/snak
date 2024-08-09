namespace Game;

public class GameState
{
    public int Rows { get; }
    public int Columns { get; }
    public Grid Grid { get; }
    public Snake Snake { get; }

    public int Score { get; private set; }
    public bool GameOver { get; private set; }

	public GameState(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Grid = new Grid(rows, columns);
        Snake = new Snake();

        AddSnake();
        Grid.AddFood();
    }

    private void AddSnake()
    {
        int middleRow = Rows / 2;
        for (int column = 2; column >= 1; column--)
        {
            Grid.Get()[middleRow, column] = CellType.Snake;
            Snake.CellsPositions.AddLast(new Position(middleRow, column));
        }
    }

    private void AddSnakeHead(Position position)
    {
        Grid.Get()[position.Row, position.Column] = CellType.Snake;
        Snake.CellsPositions.AddFirst(position);
    }

    private void RemoveSnakeTail()
    {
        var tailPosition = Snake.GetTail();
        Grid.Get()[tailPosition.Row, tailPosition.Column] = CellType.Empty;
        Snake.CellsPositions.RemoveLast();
    }




    public void MoveSnake()
    {
        if (Snake.Commands.Count > 0)
        {
            Snake.HeadDirection = Snake.Commands.First!.Value;
            Snake.Commands.RemoveFirst();
        }

        var newHeadPosition = Snake.GetHead().MoveTo(Snake.HeadDirection);
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

    private CellType WillHit(Position newHeadPosition)
    {
        if (Grid.IsOutside(newHeadPosition)) return CellType.Outside;

        if (newHeadPosition == Snake.GetTail()) return CellType.Empty;

        return Grid.Get()[newHeadPosition.Row, newHeadPosition.Column];
    }
}
