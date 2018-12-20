using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    public Transform spawnPoint;
    public TimmyCourage timmyCourage;
    private float startTime = 0;

    private void OnTriggerEnter(Collider collider) {
        GameObject obj = collider.gameObject;

        if (obj.tag == "FallingBlock") {
            return;
        }

        if (obj.tag == "Player") {
            if ((Time.time - startTime) > 0.1f) {
                obj.transform.position = spawnPoint.position;
                timmyCourage.AddCourage(-30);
                startTime = Time.time;
            }
        } else {
            Destroy(obj);
        }
    }
}