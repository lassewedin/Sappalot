using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

    public float width = 10f;
    Camera cam;

    public void Update() {
        if (cam == null) {
            cam = GetComponent<Camera>();
        }
        float requiredCameraDistance = GetRequiredDistance(cam.fieldOfView, cam.aspect, width);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -requiredCameraDistance);
    }

    //fovY in degrees
    public static float GetRequiredDistance(float fieldOfViewY, float aspect, float width) {
        float fovY = Mathf.Deg2Rad * fieldOfViewY;
        float fovX = 2f * Mathf.Atan(aspect * Mathf.Tan(fovY / 2f));
        float distanceFovX = (0.5f * width) / Mathf.Tan(0.5f * fovX);
        return distanceFovX;
    }
}
