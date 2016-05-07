using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public int maxHp = 1;

    private int hp;

    private void Start() {
        hp = maxHp;
    }

    public void Hit(int damage) {
        hp -= damage;
        if (hp <= 0) {
            Destroy(gameObject);
        }
    }
}
