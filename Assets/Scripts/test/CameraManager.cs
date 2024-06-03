using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour {

    public GameObject best_car;
    public GameObject following;

    public Vector3 initial_position;

    public float camera_speed;
    public float rotation_speed;
    public float time = 1;
    public float initial_time;

    private float theta = 0;

    // Start is called before the first frame update
    void Start() {
        List<GameObject> cars = GameObject.Find("CarManager").GetComponent<Manager>().getCars();
        int rand = UnityEngine.Random.Range(0, cars.Count - 1);
        following = cars[rand];
        initial_position = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (following != null) {
            float time_passed = (Time.time - initial_time);
            float prop = time_passed / time;

            Vector3 current_pos;

            if (prop < 1) {
                current_pos = Vector3.Lerp(initial_position, following.transform.position, prop);
            }
            else {
                current_pos = following.transform.position;
            }

            transform.position = new Vector3(current_pos.x, current_pos.y + 11.21f, current_pos.z - 17.91f);
            transform.LookAt(current_pos);
            transform.Translate(Vector3.right * Time.deltaTime * theta * 5);
        }

        if (Input.GetKeyDown(KeyCode.Space)) { 

            List<GameObject> cars = GameObject.Find("CarManager").GetComponent<Manager>().getCars();
            int index = cars.IndexOf(following);
            if (index == cars.Count - 1) {
                index = 0;
            } else {
                index += 1;
            }

            FollowCar(cars[index]);
        }
    }

    public void FollowCar(GameObject follow) {
        initial_position = following.transform.position;
        initial_time = Time.time;
        following = follow;
    }

    public void UnfollowCar() {
        following = null;
    }

    public GameObject getFollowing() {
        return following;
    }
}
