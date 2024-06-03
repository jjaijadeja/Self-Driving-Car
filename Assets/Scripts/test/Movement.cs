using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private float theta = 0;
    public float speed = 0.2f;
    public float speed_rotation = 0.5f;
    private float vz;
    public float acceleration;
    private float increase = 5;


    // Start is called before the first frame update
    void Start() {
        vz = speed;
        
    }

    // Update is called once per frame
    void Update() {
        float time = Time.deltaTime;
        transform.position += transform.forward * (vz * time + 0.5f * acceleration * time * time);
        transform.Rotate(new Vector3(0, theta, 0));
    }

    public void updateCar(List<float> outputs) {

        if (outputs[0] * 2 > 1f) {
            theta = (outputs[0] * 2 - 1) * speed_rotation * Time.deltaTime;
        }
        else {
            theta = -(outputs[0] * 2) * speed_rotation * Time.deltaTime;
        
        }
        if (outputs[1] * 2 > 1f) {
            acceleration = (outputs[1] * 2 - 1) * increase;
        }
        else {
            acceleration = -outputs[1] * 2 * increase;
        }

    }
    public float getAcceleration() {
        return acceleration;
    }

    public float getTheta() {
        return theta;
    }
}
