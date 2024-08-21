namespace Game;

public class GameState
{
    public int Rows { get; }
    public int Columns { get; }
    public Grid Grid { get; }
    public Snake Snake { get; }

    public bool IsRunning { get; set; }
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
            Grid.PutSnake(middleRow, column);
            Snake.Append(middleRow, column);
        }
    }

    private void MoveSnakeHead(Position position)
    {
        Grid.PutSnake(position);
        Snake.MoveTo(position);
    }

    private void RemoveSnakeTail()
    {
        Grid.PutEmpty(Snake.GetTail());
        Snake.DropTail();
    }

    public void MoveSnake()
    {
        Snake.ChangeDirection();

        var newHeadPosition = Snake.NextHeadPosition();
        var targetCell = WillHit(newHeadPosition);

        if (targetCell == CellType.Outside || targetCell == CellType.Snake)
        {
            GameOver = true;
            return;
        }

        if (targetCell == CellType.Empty)
        {
            RemoveSnakeTail();
            MoveSnakeHead(newHeadPosition);
            return;
        }

        if (targetCell == CellType.Food)
        {
            MoveSnakeHead(newHeadPosition);
            Score++;
            Grid.AddFood();
        }
    }

    private CellType WillHit(Position newHeadPosition)
    {
        if (Grid.IsOutside(newHeadPosition)) return CellType.Outside;

        if (newHeadPosition == Snake.GetTail()) return CellType.Empty;

        return Grid.GetCellAt(newHeadPosition);
    }
}
