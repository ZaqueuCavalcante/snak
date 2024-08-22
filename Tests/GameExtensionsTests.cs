namespace Tests;

public class GameExtensionsTests
{
    [Test]
    public void Should_return_correct_image_from_cell_type()
    {
        // Arrange / Act / Assert
        CellType.Empty.ToImage().Should().Be(Images.Empty);
        CellType.Snake.ToImage().Should().Be(Images.Body);
        CellType.Food.ToImage().Should().Be(Images.Food);
    }

    [Test]
    public void Should_return_correct_rotation_from_direction()
    {
        // Arrange / Act / Assert
        Direction.Up.ToRotation().Should().Be(0);
        Direction.Right.ToRotation().Should().Be(90);
        Direction.Down.ToRotation().Should().Be(180);
        Direction.Left.ToRotation().Should().Be(270);
    }

    [Test]
    public void Should_return_smart_direction_on_0x0_cell()
    {
        // Arrange
        var position = new Position(0, 0);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Down);
    }
    [Test]
    public void Should_return_smart_direction_on_0x1_cell()
    {
        // Arrange
        var position = new Position(0, 1);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Left);
    }
    [Test]
    public void Should_return_smart_direction_on_1x0_cell()
    {
        // Arrange
        var position = new Position(1, 0);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Right || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_1x1_cell()
    {
        // Arrange
        var position = new Position(1, 1);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Right).Should().BeTrue();
    }

    [Test]
    public void Should_return_smart_direction_on_0x2_cell()
    {
        // Arrange
        var position = new Position(0, 2);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_0x3_cell()
    {
        // Arrange
        var position = new Position(0, 3);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Left);
    }
    [Test]
    public void Should_return_smart_direction_on_1x2_cell()
    {
        // Arrange
        var position = new Position(1, 0);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Right || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_1x3_cell()
    {
        // Arrange
        var position = new Position(1, 1);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Right).Should().BeTrue();
    }

    [Test]
    public void Should_return_smart_direction_on_0x4_cell()
    {
        // Arrange
        var position = new Position(0, 4);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_0x5_cell()
    {
        // Arrange
        var position = new Position(0, 5);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Left);
    }
    [Test]
    public void Should_return_smart_direction_on_1x4_cell()
    {
        // Arrange
        var position = new Position(1, 4);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Right || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_1x5_cell()
    {
        // Arrange
        var position = new Position(1, 5);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Up);
    }

    [Test]
    public void Should_return_smart_direction_on_2x0_cell()
    {
        // Arrange
        var position = new Position(2, 0);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Down);
    }
    [Test]
    public void Should_return_smart_direction_on_2x1_cell()
    {
        // Arrange
        var position = new Position(2, 1);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Up).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_3x0_cell()
    {
        // Arrange
        var position = new Position(3, 0);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Right || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_3x1_cell()
    {
        // Arrange
        var position = new Position(3, 1);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Right).Should().BeTrue();
    }

    [Test]
    public void Should_return_smart_direction_on_2x2_cell()
    {
        // Arrange
        var position = new Position(2, 2);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_2x3_cell()
    {
        // Arrange
        var position = new Position(2, 3);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Up).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_3x2_cell()
    {
        // Arrange
        var position = new Position(3, 2);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Right || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_3x3_cell()
    {
        // Arrange
        var position = new Position(3, 3);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Right).Should().BeTrue();
    }

    [Test]
    public void Should_return_smart_direction_on_2x4_cell()
    {
        // Arrange
        var position = new Position(2, 4);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_2x5_cell()
    {
        // Arrange
        var position = new Position(2, 5);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Up).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_3x4_cell()
    {
        // Arrange
        var position = new Position(3, 4);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Right || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_3x5_cell()
    {
        // Arrange
        var position = new Position(3, 5);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Up);
    }

    [Test]
    public void Should_return_smart_direction_on_4x0_cell()
    {
        // Arrange
        var position = new Position(4, 0);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Down);
    }
    [Test]
    public void Should_return_smart_direction_on_4x1_cell()
    {
        // Arrange
        var position = new Position(4, 1);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Left).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_5x0_cell()
    {
        // Arrange
        var position = new Position(5, 0);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Right);
    }
    [Test]
    public void Should_return_smart_direction_on_5x1_cell()
    {
        // Arrange
        var position = new Position(5, 1);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Right).Should().BeTrue();
    }

    [Test]
    public void Should_return_smart_direction_on_4x2_cell()
    {
        // Arrange
        var position = new Position(4, 2);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_4x3_cell()
    {
        // Arrange
        var position = new Position(4, 1);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Left).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_5x2_cell()
    {
        // Arrange
        var position = new Position(5, 2);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Right);
    }
    [Test]
    public void Should_return_smart_direction_on_5x3_cell()
    {
        // Arrange
        var position = new Position(5, 3);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Right).Should().BeTrue();
    }

    [Test]
    public void Should_return_smart_direction_on_4x4_cell()
    {
        // Arrange
        var position = new Position(4, 5);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Left || direction == Direction.Down).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_4x5_cell()
    {
        // Arrange
        var position = new Position(4, 5);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        (direction == Direction.Up || direction == Direction.Left).Should().BeTrue();
    }
    [Test]
    public void Should_return_smart_direction_on_5x4_cell()
    {
        // Arrange
        var position = new Position(5, 4);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Right);
    }
    [Test]
    public void Should_return_smart_direction_on_5x5_cell()
    {
        // Arrange
        var position = new Position(5, 5);
        // Act
        var direction = position.GetSmartDirection(6, 6);
        // Assert
        direction.Should().Be(Direction.Up);
    }
}
