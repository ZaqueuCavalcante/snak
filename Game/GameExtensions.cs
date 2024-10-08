using System.Windows.Media;

namespace Game;

public static class GameExtensions
{
    public static ImageSource ToImage(this CellType value)
    {
        return value switch
        {
            CellType.Empty => Images.Empty,
            CellType.Snake => Images.Body,
            CellType.Food => Images.Food,
            _ => Images.Empty,
        };
    }

	private static readonly Dictionary<Direction, int> DirectionToRotation = new()
	{
		{ Direction.Up, 0 },
		{ Direction.Right, 90 },
		{ Direction.Down, 180 },
		{ Direction.Left, 270 },
	};
    public static int ToRotation(this Direction value)
    {
        return DirectionToRotation[value];
    }

    public static List<Direction> GetSmartDirection(this Position position, int rows, int columns)
    {
        var row = position.Row;
        var column = position.Column;

        if (row % 2 == 0)
        {
            if (column % 2 == 0)
            {
                if (column == 0) return [Direction.Down];
                return [Direction.Left, Direction.Down];
            }

            if (row == 0) return [Direction.Left];
            return [Direction.Left, Direction.Up];
        }

        if (column % 2 == 0)
        {
            if (row == rows-1) return [Direction.Right];
            return [Direction.Right, Direction.Down];
        }

        if (column == columns-1) return [Direction.Up];
        return [Direction.Up, Direction.Right];
    }
}
