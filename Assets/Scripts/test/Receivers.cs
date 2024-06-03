using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receivers : MonoBehaviour {

    public Color color = new Color(255, 255, 0, 0.5f);
    public float distance = 20;
    public static int num_receivers = 5;
    public static int spread = 120;
    public float height = 0;

    private static int interval;
    private GameObject[] receivers;


    // Start is called before the first frame update
    void Start() {
        interval = spread / (num_receivers - 1);
        receivers = new GameObject[num_receivers];

        for (int i = 0; i < num_receivers; i++) {
            float angle = interval * i - spread / 2;
            GameObject temp = new GameObject();
            Receiver receiver = temp.AddComponent<Receiver>();
            receiver.final_width = 0.02f;
            receiver.color = color;
            receiver.length = distance;
            receiver.transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
            receiver.angle = angle;

            receivers[i] = temp;
            receivers[i].transform.Rotate(new Vector3(0, angle, 0));
            temp.transform.SetParent(transform);
        }
        
    }

    // Update is called once per frame
    void Update() {

        foreach (GameObject receiver in receivers) {
            receiver.transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        }
        
    }


    public float[] getDistance() {
        float[] inputs = new float[receivers.Length];

        for (int i = 0; i < receivers.Length; i++) {
            inputs[i] = receivers[i].GetComponent<Receiver>().getDistance();
        }

        return inputs;
    }
}
