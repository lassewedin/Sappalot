using UnityEngine;
using System.Collections;

public class AngleControls : MonoBehaviour {


    public float maxSpeedX = 1f;
    public float maxSpeedY = 1f;
    public float forceMagnitude = 10f;
    public float gunAngle = 20f;

    
    private new Rigidbody rigidbody;


	void Update () {
        if (rigidbody == null) {
            rigidbody = transform.GetComponent<Rigidbody>();
        }

        Vector3 leftForceVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (90f + gunAngle)) * forceMagnitude, Mathf.Sin(Mathf.Deg2Rad * (90f + gunAngle)) * forceMagnitude, 0f);
        Vector3 rightForceVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (90f - gunAngle)) * forceMagnitude, Mathf.Sin(Mathf.Deg2Rad * (90f - gunAngle)) * forceMagnitude, 0f);
        Vector3 centerForceVector = new Vector3(0f, forceMagnitude, 0f);

        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.RightArrow);

        if (left && !right) {
            rigidbody.AddForce(rightForceVector, ForceMode.Impulse);
            rigidbody.velocity = new Vector3(Mathf.Max(rigidbody.velocity.x, 0f), rigidbody.velocity.y, 0f);
        }
        else if (right && !left) {
            rigidbody.AddForce(leftForceVector, ForceMode.Impulse);
            rigidbody.velocity = new Vector3(Mathf.Min(rigidbody.velocity.x, 0f), rigidbody.velocity.y, 0f);
        }
        else if (left && right) {
            rigidbody.AddForce(centerForceVector, ForceMode.Impulse);
            //rigidbody.velocity = centerForceVector * forceMagnitude;
        }

        float speed = rigidbody.velocity.magnitude;
        rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -maxSpeedX, maxSpeedX), Mathf.Clamp(rigidbody.velocity.y, -maxSpeedY, maxSpeedY), 0f);
    }
}
