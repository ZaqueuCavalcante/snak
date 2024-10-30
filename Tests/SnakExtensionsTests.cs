namespace Tests;

public class SnakExtensionsTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.Angles))]
    public void Should_return_correct_value_for_angle((Position a, Position b, double angle) data)
    {
        // Arrange / Act
        var result = SnakExtensions.AbsoluteAngle(data.a, data.b);
        
        // Assert
        result.Should().Be(data.angle);
    }
}
