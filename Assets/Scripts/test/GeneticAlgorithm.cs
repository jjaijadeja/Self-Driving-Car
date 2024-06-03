using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneticAlgorithm {

    [SerializeField] private List<float[][]> individual;

    private float mutation_rate = 0.2f;
    private float crossover_rate = 0.65f;

    public GeneticAlgorithm(List<float[][]> weights) {
        this.individual = weights;
    }

    public List<float[][]> getIndividual() {
        return individual;
    }

    public GeneticAlgorithm Mutate() {
        List<float[][]> new_individual = new List<float[][]>();
        for (int i = 0; i < individual.Count; i++) {
            float[][] weights_layer = individual[i];
            for (int j = 0; j < weights_layer.Length; j++) {
                for (int k = 0; k < weights_layer[j].Length; k++) {
                    float rand = UnityEngine.Random.Range(0f, 1f);
                    if (rand < mutation_rate) {
                        weights_layer[j][k] = UnityEngine.Random.Range(-1f, 1f);
                    }
                }
            }

            new_individual.Add(weights_layer);
        }

        return new GeneticAlgorithm(new_individual);
    }

    public GeneticAlgorithm Crossover(GeneticAlgorithm parent) {

        List<float[][]> child = new List<float[][]>();

        for (int i = 0; i < individual.Count; i++) {
            float[][] parent_layer = parent.getIndividual()[i];
            float[][] layer = individual[i];

            for (int j = 0; j < parent_layer.Length; j++) {
                for (int k = 0; k < parent_layer[j].Length; k++) {
                    float rand = UnityEngine.Random.Range(0f, 1f);

                    if (rand < crossover_rate)
                    {
                        layer[j][k] = parent_layer[j][k];
                    }
                    else {
                        layer[j][k] = layer[j][k];
                    }
                }
            }
            child.Add(layer);

        }
        return new GeneticAlgorithm(child);
    }
}

