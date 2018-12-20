using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour {
    private bool weaken = false;
    public bool refillable;
    public float refillTimer;
    public Material normal;
    public Material cracked;

    public void Dug() {
        if (weaken) {
            DestroyEarth();
        } else {
            weaken = true;
            GetComponent<Renderer>().material = cracked;
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag != "Enemy") return;

        if (weaken && System.Math.Abs(collision.transform.position.z -transform.position.z) < 0.2f) {
            DestroyEarth();
        }
    }

    private void Refill() {
        FindObjectOfType<GameManager>().StartActivateCoroutine(this.gameObject, refillTimer);
    }

    private void DestroyEarth() {
        if (refillable) {
            Refill();
            weaken = false;
            GetComponent<Renderer>().material = normal;
            this.gameObject.SetActive(false);
        } else {
            Destroy(this.gameObject);
        }
    }
}
