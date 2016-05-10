using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public bool invertedControls = false;
    public KeyCode leftKey;
    public KeyCode rightKey;

    public float maxSpeedX = 1f;
    public float maxSpeedY = 1f;
    public float forceMagnitude = 10f;
    public float forceAngle = 20f;
    public float gunAngle = 20f;
    public float gunScatterAngle = 5f;
    public float waitTimeBeforeFireWapon0 = 0f;
    public float waitTimeBeforeFireWapon1 = 1f;
    public Transform hands;

    private new Rigidbody rigidbody;
    private float timeUntilFireWeapon0;
    private float timeUntilFireWeapon1;
  

	public float hoverForce = 20f;

    public Weapon[] weapons;

    void Update () {

    }

	void FixedUpdate() {
		if (rigidbody == null) {
			rigidbody = transform.GetComponent<Rigidbody>();
		}

		Vector3 leftForceVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (90f + forceAngle)) * forceMagnitude, Mathf.Sin(Mathf.Deg2Rad * (90f + forceAngle)) * forceMagnitude, 0f);
		Vector3 rightForceVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (90f - forceAngle)) * forceMagnitude, Mathf.Sin(Mathf.Deg2Rad * (90f - forceAngle)) * forceMagnitude, 0f);
		Vector3 centerForceVector = new Vector3(0f, forceMagnitude, 0f);

		bool left = invertedControls ? Input.GetKey(rightKey) : Input.GetKey(leftKey);
		bool right = invertedControls ? Input.GetKey(leftKey) : Input.GetKey(rightKey);

		if (left && !right) {
			hands.transform.rotation = Quaternion.Euler(0f, 0f, -gunAngle + Random.Range(-gunScatterAngle * 0.5f, gunScatterAngle * 0.5f));
			rigidbody.AddForce(rightForceVector, ForceMode.Impulse);
			Attack();
		}
		else if (right && !left) {
			rigidbody.AddForce(leftForceVector, ForceMode.Impulse);
			hands.transform.rotation = Quaternion.Euler(0f, 0f, gunAngle + Random.Range(-gunScatterAngle * 0.5f, gunScatterAngle * 0.5f));
			Attack();
		}
		else if (left && right) {
			rigidbody.AddForce(centerForceVector, ForceMode.Impulse);
			hands.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-gunScatterAngle * 0.5f, gunScatterAngle * 0.5f));
			Attack();
		}
		else if (!left && !right) {
			timeUntilFireWeapon0 = waitTimeBeforeFireWapon0;
			timeUntilFireWeapon1 = waitTimeBeforeFireWapon1;
			hands.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}

    private void Attack() {
		bool leftShift = Input.GetKey(KeyCode.LeftShift);

        if (timeUntilFireWeapon0 <= 0f) {
			if (!leftShift)
				weapons[0].Attack();
			else
				weapons[2].Attack();
        }
        else {
            timeUntilFireWeapon0 -= Time.deltaTime;
        }

        if (timeUntilFireWeapon1 <= 0f) {
			if (!leftShift)
				weapons[1].Attack();
			else
				weapons[3].Attack();
        }
        else {
            timeUntilFireWeapon1 -= Time.deltaTime;
        }
    }
		
	void OnTriggerStay(Collider collider) {
		if (rigidbody == null) {
			rigidbody = transform.GetComponent<Rigidbody>();
		}

		if (collider.tag == "HoverZone") {
			rigidbody.AddForce(Vector3.up * hoverForce, ForceMode.Acceleration);
		}
		  
	}




}
