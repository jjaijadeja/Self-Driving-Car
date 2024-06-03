using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class CarController : MonoBehaviour {

    private GeneticAlgorithm genetic;
    private NeuralNetwork NN;

    public string file_name;

    private Vector3 initial_point;

    public float distance;
    public float trackTime;

    private bool initialised = false;


    public float fitness; // overall fitness score
    public float distance_score = 1.3f;
    public float speed_score = 0.7f;

    private float distance_travelled; // how far the car travelled
    private float average_speed; // average speed of car during life

    private Vector3 lastPos; // last position of car

    public void Activate() {
        NN = new NeuralNetwork();
        genetic = new GeneticAlgorithm(NN.getWeights());
        initial_point = transform.position;
        lastPos = transform.position;
        initialised = true;
    }

    public void Activate(GeneticAlgorithm genetic)
    {
        NN = new NeuralNetwork(genetic);
        this.genetic = genetic;
        initial_point = transform.position;
        lastPos = transform.position;
        initialised = true;
    }

    private void FixedUpdate()
    {
        if (initialised) { 
            trackTime += Time.deltaTime;
            average_speed = distance_travelled / trackTime;
        }


    }

    // Update is called once per frame
    void Update() {
        if (initialised) {
            // get inputs
            float[] inputs = GetComponent<Receivers>().getDistance();

            // feed forward to create network
            NN.FeedForward(inputs);

            List<float> outputs = NN.getOutputs();
            GetComponent<Movement>().updateCar(outputs);
            distance = Vector3.Distance(transform.position, initial_point);
            distance_travelled += Vector3.Distance(transform.position, lastPos);
            lastPos = transform.position;
        }

    }

    public GeneticAlgorithm getGenetic() {
        return genetic;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("wall2"))
        {
            WriteData();
            onDeath();
        }
    }

    public void onDeath() {

        Manager manager = GameObject.Find("CarManager").GetComponent<Manager>();

        List<GameObject> cars = manager.getCars();



        if (cars.Count == 2) {
            manager.best = cars[0].GetComponent<CarController>().getGenetic();
            manager.next_best = cars[1].GetComponent<CarController>().getGenetic();
        }

        if (cars.Count == 1) {
            if (!manager.best.Equals(cars[0].GetComponent<CarController>().getGenetic())) {
                GeneticAlgorithm temp = manager.next_best;
                manager.next_best = manager.best;
                manager.best = temp;
            }

            if (cars[0].GetComponent<CarController>().getDistance() > 80) {
                SaveToJson(manager.getBestGenetic().getIndividual());
            }

            cars.Remove(gameObject);
            manager.CreatePopulation(true);
            Destroy(gameObject);
        }
        else {
            int rand = UnityEngine.Random.Range(0, (int)cars.Count);
            if (cars[rand] == gameObject) {
                onDeath();
            }
            else {
                if (gameObject == GameObject.Find("Camera").GetComponent<CameraManager>().getFollowing()) {
                    GameObject.Find("Camera").GetComponent<CameraManager>().FollowCar(cars[rand]);
                }

                cars.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public float getDistance() {
        return distance;
    }

    public void SaveToJson(List<float[][]> individual) {
        string data = JsonUtility.ToJson(individual);

        Debug.Log(data);

        string file_path = Application.persistentDataPath + "/CarNeuralNetwork.json";

        Debug.Log(file_path);
        System.IO.File.WriteAllText(file_path, data);
    }

    public void LoadFromJson() {
        string file_path = Application.persistentDataPath + "/CarNeuralNetwork.json";
        string data = System.IO.File.ReadAllText(file_path);

        genetic = JsonUtility.FromJson<GeneticAlgorithm>(data);
    }

    public void WriteData() {
        // path of file

        string file_path = Application.dataPath + "/data.txt";

        // create file if it doesn't exist
        if (!File.Exists(file_path)) {
            File.WriteAllText(file_path, "");
        }

        // write to file
        // generation
        // hidden layers
        // number of neurons
        // total distance travelled
        // total time (survived)
        // fitness


        Manager manager = GameObject.Find("CarManager").GetComponent<Manager>();

        string generations = manager.generation.ToString();
        string hl = NN.getNumHiddenLayer().ToString();
        string neurons = NN.getNumHiddenLayerSize().ToString();

        string content = generations + "//" + hl + "//" + neurons + "//" + distance_travelled.ToString() + "//" + trackTime.ToString() + "//" + average_speed.ToString() + "\n";
        File.AppendAllText(file_path, content);


    }
}
