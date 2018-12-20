using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    public Transform spawnPoint;
    public TimmyCourage timmyCourage;
    private float startTime = 0;

    private void OnCollisionEnter(Collision collision) {
        GameObject obj = collision.gameObject;

        if (obj.tag == "Player") {
            if ((Time.time - startTime) > 0.1f) {
                obj.transform.position = spawnPoint.position;
                timmyCourage.AddCourage(-30);
                startTime = Time.time;
            }
        }
    }
}
