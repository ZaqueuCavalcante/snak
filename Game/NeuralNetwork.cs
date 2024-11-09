using System.Numerics;

namespace Game;

public class NeuralNetwork
{
    // Os valores dos pesos variam de -1000 a 1000
    public double[][] IntermediateNeurons { get; }
    public double[][] OutputNeurons { get; }

    public double[] IntermediateOutput { get; }
    public double[] FinalOutput { get; }

    public NeuralNetwork(double[][] intermediateNeurons, double[][] outputNeurons)
    {
        IntermediateNeurons = intermediateNeurons;
        OutputNeurons = outputNeurons;
        IntermediateOutput = new double[IntermediateNeurons.GetUpperBound(0)+1];
        FinalOutput = new double[OutputNeurons.GetUpperBound(0)+1];;
    }

    public Direction Calculate(double[] inputs)
    {
        var intermediateMaxValue = inputs.Length * 1000d;
        for (int neuron = 0; neuron <= IntermediateNeurons.GetUpperBound(0); neuron++)
        {
            var value = SnakExtensions.Multiply(inputs, IntermediateNeurons[neuron]);
            IntermediateOutput[neuron] = value / intermediateMaxValue;
        }

        var outputMaxValue = IntermediateOutput.Length * 1000d;
        for (int neuron = 0; neuron <= OutputNeurons.GetUpperBound(0); neuron++)
        {
            var value = SnakExtensions.Multiply(IntermediateOutput, OutputNeurons[neuron]);
            FinalOutput[neuron] = value / outputMaxValue;
        }

        int maxIndex = FinalOutput.Length > 0 ? FinalOutput.ToList().IndexOf(FinalOutput.Max()) : 0;

        return maxIndex switch
        {
            0 => Direction.Right,
            1 => Direction.Down,
            2 => Direction.Left,
            _ => Direction.Up,
        };
    }
}
