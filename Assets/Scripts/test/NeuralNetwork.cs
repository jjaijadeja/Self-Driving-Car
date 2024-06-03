using System.Collections.Generic;
using UnityEngine;

// remove monobehaviour as unity aid is not required
public class NeuralNetwork {

    // neural network key values
    public int input_layer_size = 5;
    public int output_layer_size = 2;

    public int hidden_layers = 1;
    public int hidden_layer_size = 5;

    public float max_initial_value = 1f;

    private const float EULER = 2.71828f;
    private List<List<float>> neurons;
    private List<float[][]> weights;

    private int total_layers = 0;


    public NeuralNetwork() {
        // total number of layers in NN
        total_layers = hidden_layers + 2;

        // initialise neurons and weights constructs
        neurons = new List<List<float>>();
        weights = new List<float[][]>();

        // fill neuron and weights values
        for (int i = 0; i < total_layers; i++) {

            float[][] current_layer_weights;

            //create new layer
            List<float> layer = new List<float>();

            int layer_size = getLayerSize(i);

            if (i != hidden_layers + 1) {  // check layer is not OUTPUT
                current_layer_weights = new float[layer_size][];
                int next_layer = getLayerSize(i + 1);
                for (int j = 0; j < layer_size; j++) { // current layer
                    current_layer_weights[j] = new float[next_layer];
                    for (int k = 0; k < next_layer; k++) { // next layer
                        current_layer_weights[j][k] = generateRandomValue();
                    }
                }
                weights.Add(current_layer_weights);
            }

            for (int j = 0; j < layer_size;  j++) {
                layer.Add(0);
            }
            neurons.Add(layer);
        }
    }

    public NeuralNetwork(GeneticAlgorithm genetic) {

        List<float[][]> genetic_weights = genetic.getIndividual();


        // total number of layers in NN
        total_layers = hidden_layers + 2;

        // initialise neurons and weights constructs
        neurons = new List<List<float>>();
        weights = new List<float[][]>();

        // fill neuron and weights values
        for (int i = 0; i < total_layers; i++)
        {
            float[][] current_layer_weights;
            float[][] genetic_weights_layer;

            //create new layer
            List<float> layer = new List<float>();

            int layer_size = getLayerSize(i);

            if (i != hidden_layers + 1)
            {  // check layer is not OUTPUT
                genetic_weights_layer = genetic_weights[i];
                current_layer_weights = new float[layer_size][];
                int next_layer = getLayerSize(i + 1);
                for (int j = 0; j < layer_size; j++)
                { // current layer
                    current_layer_weights[j] = new float[next_layer];
                    for (int k = 0; k < next_layer; k++)
                    { // next layer
                        current_layer_weights[j][k] = genetic_weights_layer[j][k];
                    }
                }
                weights.Add(current_layer_weights);
            }

            for (int j = 0; j < layer_size; j++)
            {
                layer.Add(0);
            }
            neurons.Add(layer);
        }
    }

    public void FeedForward(float[]input_layer_size) {

        // updating the network

        // set layer values in INPUT
        List<float> input_layer = neurons[0];
        for (int i = 0; i < input_layer_size.Length; i++) {
            input_layer[i] = input_layer_size[i];
        }

        // update neurons from layers INPUT to OUTPUT

        for (int layer = 0; layer < neurons.Count - 1; layer++) {
            float[][] weights_layer = weights[layer];
            int next_layer = layer + 1;
            List<float> neurons_layer = neurons[layer];
            List<float> next_neurons_layer = neurons[next_layer];

            for (int i = 0; i < next_neurons_layer.Count; i++) { // next layer
                float total = 0;
                for (int j = 0; j < neurons_layer.Count; j++) {
                    // multiplication for feed forward
                    total += weights_layer[j][i] * neurons_layer[j];
                }

                next_neurons_layer[i] = Sigmoid(total);
            }
        }
    }

    public List<float> getOutputs() {
        return neurons[neurons.Count - 1];
    }

    public float Sigmoid(float s) {
        return 1 / (float)(1 + Mathf.Pow(EULER, -s));
    }

    public float generateRandomValue() {
        return UnityEngine.Random.Range(-max_initial_value, max_initial_value);
    }

    public int getLayerSize(int i) {
        int size = 0;

        if (i == 0) // check if layer is INPUT
        {
            size = input_layer_size;
        }
        else if (i == hidden_layers + 1) // check if layer is OUTPUT
        {
            size = output_layer_size;
        }
        else {
            size = hidden_layer_size; // check if layer is HIDDEN
        }


        return size;
    }

    public List<List<float>> getNeurons() {
        return neurons;
    }

    public List<float[][]> getWeights() {
        return weights;
    }

    public int getNumHiddenLayer() {
        return hidden_layers;
    }

    public int getNumHiddenLayerSize() {
        return hidden_layer_size;
    }

    
}
