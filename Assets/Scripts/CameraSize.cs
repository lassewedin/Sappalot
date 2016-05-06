using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

    public Transform[] followBikes;

    public float width = 10f;

    Camera cam;
    //public Bounds boundingRect;
    //public Bounds viewBoundingRect;
    //Vector3 centerPosition;

    public void Update() {
        if (cam == null) {
            cam = GetComponent<Camera>();
        }

        //UpdateBoundingRect(followBikes);
        //UpdateViewBoundingRect();

        //centerPosition = viewBoundingRect.center;

        float requiredCameraDistance = GetRequiredDistance(cam.fieldOfView, cam.aspect, width);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -requiredCameraDistance);
    }

    //fovY in degrees
    public static float GetRequiredDistance(float fieldOfViewY, float aspect, float width) {
        float fovY = Mathf.Deg2Rad * fieldOfViewY;
        float fovX = 2f * Mathf.Atan(aspect * Mathf.Tan(fovY / 2f));
        float distanceFovX = (0.5f * width) / Mathf.Tan(0.5f * fovX);
        //float distanceFovY = (0.5f * viewedRect.size.y) / Mathf.Tan(0.5f * fovY);

        //float requiredCameraDistance = Mathf.Max(distanceFovX, distanceFovY);
        return distanceFovX; // requiredCameraDistance;
    }

    // private Vector3[] GetPositions(Transform[] transforms) {
    //     Vector3[] positions = new Vector3[transforms.Length];
    //     for (int i = 0; i < transforms.Length; i++) {
    //          positions[i] = transforms[i].position;
    //      }
    //     return positions;
    // }

    //private void UpdateBoundingRect(Transform[] transforms) {
    //    float minX = float.PositiveInfinity;
    //    float maxX = float.NegativeInfinity;
    //    float minY = float.PositiveInfinity;
    //    float maxY = float.NegativeInfinity;
    //    foreach (var transform in transforms) {
    //        minX = Mathf.Min(minX, transform.position.x);
    //        maxX = Mathf.Max(maxX, transform.position.x);
    //        minY = Mathf.Min(minY, transform.position.y);
    //        maxY = Mathf.Max(maxY, transform.position.y);
    //    }
    //
    //    boundingRect.SetMinMax(new Vector3(minX, minY), new Vector3(maxX, maxY));
    //}

    //private void UpdateViewBoundingRect() {
    //    viewBoundingRect.SetMinMax(new Vector3(boundingRect.min.x - leftMargin, boundingRect.min.y - bottomMargin), new Vector3(boundingRect.max.x + rightMargin, boundingRect.max.y + topMargin));
    //}

    //private void OnDrawGizmos() {
    //    Vector3 leftBottom;
    //    Vector3 leftTop;
    //    Vector3 rightBottom;
    //    Vector3 rightTop;

    //    leftBottom = new Vector3(boundingRect.min.x, boundingRect.min.y, 0f);
    //    leftTop = new Vector3(boundingRect.min.x, boundingRect.max.y, 0f);
    //    rightBottom = new Vector3(boundingRect.max.x, boundingRect.min.y, 0f);
    //     rightTop = new Vector3(boundingRect.max.x, boundingRect.max.y, 0f);

    //   Gizmos.color = Color.grey;
    //    Gizmos.DrawLine(leftBottom, rightBottom);
    //    Gizmos.DrawLine(leftTop, rightTop);
    //    Gizmos.DrawLine(leftBottom, leftTop);
    //    Gizmos.DrawLine(rightBottom, rightTop);


    //    leftBottom = new Vector3(viewBoundingRect.min.x, viewBoundingRect.min.y, 0f);
    //    leftTop = new Vector3(viewBoundingRect.min.x, viewBoundingRect.max.y, 0f);
    //    rightBottom = new Vector3(viewBoundingRect.max.x, viewBoundingRect.min.y, 0f);
    //    rightTop = new Vector3(viewBoundingRect.max.x, viewBoundingRect.max.y, 0f);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(leftBottom, rightBottom);
    //    Gizmos.DrawLine(leftTop, rightTop);
    //    Gizmos.DrawLine(leftBottom, leftTop);
    //    Gizmos.DrawLine(rightBottom, rightTop);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(centerPosition, new Vector3(0.1f, 0.1f, 0.1f));
    // }


}
