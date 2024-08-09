namespace Game;

public class Snake
{
    public Direction HeadDirection { get; set; } = Direction.Right;
    public LinkedList<Position> CellsPositions { get; set; } = [];
	public LinkedList<Direction> Commands { get; set; } = [];


    public Position GetHead()
    {
        return CellsPositions.First!.Value;
    }

    public Position GetTail()
    {
        return CellsPositions.Last!.Value;
    }

    private Direction GetLastDirection()
    {
        if (Commands.Count == 0) return HeadDirection;

        return Commands.Last!.Value;
    }

	public bool CanChangeDirection(Direction newDirection)
	{
		if (Commands.Count == 2) return false;

        var lastDirection = GetLastDirection();
		return newDirection != lastDirection && newDirection != lastDirection.Opposite();
	}

	public void GoTo(Direction direction)
    {
        if (CanChangeDirection(direction))
        {
            Commands.AddLast(direction);  
        }
    }
}
