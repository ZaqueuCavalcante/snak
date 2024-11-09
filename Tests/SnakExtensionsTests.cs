namespace Tests;

public class SnakExtensionsTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.Directions))]
    public void Should_return_correct_value_for_absolute_direction((Direction direction, double value) data)
    {
        // Arrange / Act
        var result = data.direction.Absolute();
        
        // Assert
        result.Should().Be(data.value);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.DeltaRows))]
    public void Should_return_correct_value_for_absolute_delta_row((Position a, Position b, double value) data)
    {
        // Arrange / Act
        var result = SnakExtensions.AbsoluteDeltaRow(data.a, data.b, 10);
        
        // Assert
        result.Should().Be(data.value);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.DeltaColumns))]
    public void Should_return_correct_value_for_absolute_delta_column((Position a, Position b, double value) data)
    {
        // Arrange / Act
        var result = SnakExtensions.AbsoluteDeltaColumn(data.a, data.b, 10);
        
        // Assert
        result.Should().Be(data.value);
    }
}
