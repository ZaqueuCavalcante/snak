namespace Tests;

public class GameStateTests
{
    [Test]
    public void Should_create_correct_game_state()
    {
        // Arrange
        var rows = 3;
        var columns = 4;

        // Act
        var state = new GameState(rows, columns);

        // Assert
        state.Rows.Should().Be(rows);
        state.Columns.Should().Be(columns);
        state.Grid.Get()[1, 1].Should().Be(CellType.Snake);
        state.Grid.Get()[1, 2].Should().Be(CellType.Snake);
        state.SnakeDirection.Should().Be(Direction.Right);
    }
}
