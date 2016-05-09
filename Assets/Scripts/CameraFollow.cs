using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{

    public GameObject follow;

    Camera cam;

    public void Update() {
        if (cam == null) {
            cam = GetComponent<Camera>();
        }
        cam.transform.position = new Vector3(cam.transform.position.x, follow.transform.position.y, cam.transform.position.z);
    }
}
