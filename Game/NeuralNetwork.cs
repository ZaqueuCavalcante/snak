namespace Game;

public class NeuralNetwork
{
    // Os valores dos pesos variam de -1000 a 1000
    public double[][] IntermediateNeurons { get; }
    public double[][] OutputNeurons { get; }

    public int Score { get; set; }

    public double[] _intermediateOutput { get; }
    public double[] _finalOutput { get; }

    public NeuralNetwork(double[][] intermediateNeurons, double[][] outputNeurons)
    {
        IntermediateNeurons = intermediateNeurons;
        OutputNeurons = outputNeurons;
        _intermediateOutput = new double[IntermediateNeurons.GetUpperBound(0)+1];
        _finalOutput = new double[OutputNeurons.GetUpperBound(0)+1];;
    }

    public Direction[] Calculate(double[] inputs)
    {
        var intermediateMaxValue = inputs.Length * 1000d;
        for (int neuron = 0; neuron <= IntermediateNeurons.GetUpperBound(0); neuron++)
        {
            var value = SnakExtensions.Multiply(inputs, IntermediateNeurons[neuron]);
            _intermediateOutput[neuron] = value / intermediateMaxValue;
        }

        var outputMaxValue = _intermediateOutput.Length * 1000d;
        for (int neuron = 0; neuron <= OutputNeurons.GetUpperBound(0); neuron++)
        {
            var value = SnakExtensions.Multiply(_intermediateOutput, OutputNeurons[neuron]);
            _finalOutput[neuron] = value / outputMaxValue;
        }

        var outputs = new List<NNOutput>
        {
            new() { Direction = Direction.Right, Value = Math.Abs(_finalOutput[0]) },
            new() { Direction = Direction.Down, Value = Math.Abs(_finalOutput[1]) },
            new() { Direction = Direction.Left, Value = Math.Abs(_finalOutput[2]) },
            new() { Direction = Direction.Up, Value = Math.Abs(_finalOutput[3]) },
        };

        return outputs.OrderByDescending(x => x.Value).Select(x => x.Direction).ToArray();
    }

    public static NeuralNetwork NewRandom()
    {
        var random = new Random();
        double Next()
        {
            return random.NextDouble() * 2000 - 1000;
        }

        double[][] intermediateNeurons =
        [
            [Next(), Next(), Next(), Next()],
            [Next(), Next(), Next(), Next()],
            [Next(), Next(), Next(), Next()],
            [Next(), Next(), Next(), Next()],
            [Next(), Next(), Next(), Next()],
        ];
        double[][] outputNeurons =
        [
            [Next(), Next(), Next(), Next(), Next()],
            [Next(), Next(), Next(), Next(), Next()],
            [Next(), Next(), Next(), Next(), Next()],
            [Next(), Next(), Next(), Next(), Next()],
        ];

        return new NeuralNetwork(intermediateNeurons, outputNeurons);
    }
}

public class NNOutput
{
    public double Value { get; set; }
    public Direction Direction { get; set; }
}
