namespace Tests;

public class NeuralNetworkTests
{
    [Test]
    public void Should_calculate_correct_direction_when_all_weights_are_zero()
    {
        // Arrange
        var neuralNetwork = new NeuralNetwork([], []);
        double[] inputs = [0.50, -0.36, 0.89];

        // Act
        var direction = neuralNetwork.Calculate(inputs);

        // Assert
        direction.Should().Be(Direction.Right);
    }

    [Test]
    public void Should_calculate_correct_direction_when_all_weights_are_max()
    {
        // Arrange
        double[][] intermediateNeurons =
        [
            [1000, 1000, 1000],
            [1000, 1000, 1000],
            [1000, 1000, 1000],
            [1000, 1000, 1000],
            [1000, 1000, 1000],
        ];

        double[][] outputNeurons =
        [
            [1000, 1000, 1000, 1000, 1000],
            [1000, 1000, 1000, 1000, 1000],
            [1000, 1000, 1000, 1000, 1000],
            [1000, 1000, 1000, 1000, 1000],
        ];

        var neuralNetwork = new NeuralNetwork(intermediateNeurons, outputNeurons);
        double[] inputs = [0.50, -0.10, 0.20];

        // Act
        var direction = neuralNetwork.Calculate(inputs);

        // Assert
        direction.Should().Be(Direction.Right);
    }
}
