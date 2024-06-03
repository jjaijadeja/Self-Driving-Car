using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour {
    public Color color = new Color(0, 1, 0, 0.5f);
    public float length = 50;
    public float final_width = 0.02f;
    public float initial_width = 0.02f;
    public float angle;

    private Vector3 position;
    private LineRenderer line;
    private float distance = 0;

    // Start is called before the first frame update
    void Start() {
        distance = length;
        position = new Vector3(0, 0, final_width);
        line = gameObject.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        line.startColor = color;
        line.endColor = color;
        line.startWidth = initial_width;
        line.endWidth = final_width;
        line.positionCount = 2;
    }

    // Update is called once per frame
    void Update() {

        Vector3 final_point = transform.position + transform.forward * length;

        RaycastHit collision;

        if (Physics.Raycast(transform.position, transform.forward, out collision, distance)) {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, collision.point);
            distance = collision.distance;
        }
        else {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, final_point);
            distance = length;
        }
    }

    public float getDistance() {
        return distance / length;
    }


}
