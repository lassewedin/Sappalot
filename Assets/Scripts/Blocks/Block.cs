using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public enum Type {
		unbreakable,
		rock,
		iceTemp,
		fireTemp,
		dynamicTemperature,
	
	}
	public Type type = Type.rock;

    public GameObject blockParticle;

    public int maxHp = 8;

    public float startTremperature = 0f;
	public float temperatureReached = 1f;
	public float temperatureMax = 2f;
	public float temperatureToNormalSpeed = 0.2f;

	public GameObject fire;
	public GameObject ice;

    private int hp;
	private float temperature = 0f;
	private Material material;

    private void Awake() {
        hp = maxHp;
        temperature = startTremperature;
    }

	public void Hit(int damage, float temperature) {
		if (type == Type.unbreakable) {
            return;
        }

		if (type == Type.rock) {
			hp -= damage;
			if (hp <= 0) {
                Destroy();
            }
		}

		if (type == Type.dynamicTemperature) {
			if (this.temperature < -temperatureReached && temperature > 0f) {
                Destroy();
            } else if (this.temperature > temperatureReached && temperature < 0f) {
                Destroy();
            }
			this.temperature += temperature;
			this.temperature = Mathf.Clamp(this.temperature, -temperatureMax, temperatureMax);
		}
    }

    private void Destroy() {
        
        GameObject breakParticleInstance = Instantiate(blockParticle, transform.position + new Vector3(0f, 0f, -5f), Quaternion.identity) as GameObject; //y - 2.5 since 90 degrees turned around x
        Destroy(gameObject);
    } 

	public void FixedUpdate() {
		if (type == Type.dynamicTemperature) {
			if (temperature != 0f) {
				float speedChange = temperatureMax * temperatureToNormalSpeed;
				if (temperature > 0f) {
					temperature -= Time.fixedDeltaTime * speedChange;
					if (temperature < 0f) {
						temperature = 0f;
					}
				} else if (temperature < 0f) {
					temperature += Time.fixedDeltaTime * speedChange;
					if (temperature > 0f) {
						temperature = 0f;
					}
				}
			}
			if (temperature > temperatureReached) {
				fire.SetActive(true);
				ice.SetActive(false);
			} else if (temperature < -temperatureReached) {
				fire.SetActive(false);
				ice.SetActive(true);
			} else {
				fire.SetActive(false);
				ice.SetActive(false);
			}

		}
	}

	public void Update() {
		if (type == Type.dynamicTemperature) {
			if (material == null) {
				material = GetComponent<MeshRenderer>().material;
			}
			float warmColor = Mathf.Max(temperature / temperatureReached, 0f);
			float coldColor = Mathf.Max(-temperature / temperatureReached, 0f);


			float red = 0.5f + warmColor / 2f;
			float green = 0.5f;
			float blue = 0.5f + coldColor / 2f;
			material.color = new Color(red, green, blue, 1f);
		}
	}

}
