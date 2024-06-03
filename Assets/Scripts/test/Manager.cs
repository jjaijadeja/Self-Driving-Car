using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public List<GameObject> cars;
    public GameObject car;

    public GeneticAlgorithm best;
    public GeneticAlgorithm next_best;


    public int population = 10;
    public int generation = 0;

    private int cars_generated = 0;

    public float runtime;


    // Start is called before the first frame update
    void Start() {
        CreatePopulation();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        runtime += Time.deltaTime;
    }

    public List<GameObject> getCars() {
        return cars;
    }

    public void CreatePopulation() {
        cars = new List<GameObject>();

        for (int i = 0; i < population; i++) { 
            GameObject create_car = (Instantiate(car));
            cars.Add(create_car);
            create_car.GetComponent<CarController>().Activate();
        }

        Debug.Log("create " + cars.Count);

        generation++;
    }

    public void CreatePopulation(bool manipulation) {

        generation++;

        if (manipulation) {
            cars = new List<GameObject>();
            for (int i = 0; i < population; i++) {
                GeneticAlgorithm genetic = best.Crossover(next_best);
                GeneticAlgorithm mutated = genetic.Mutate();
                GameObject create_car = Instantiate(car);
                cars.Add(create_car);
                create_car.GetComponent<CarController>().Activate(mutated);
            }
        }
        cars_generated = 0;
        GameObject.Find("Camera").GetComponent<CameraManager>().FollowCar(cars[0]);
    }

    public void RestartGen() {
        cars.Clear();
        CreatePopulation();
    }

    public GeneticAlgorithm getBestGenetic() {
        return best;
    }
}
