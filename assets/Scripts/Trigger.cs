using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
    public string target;
    public Triggerable[] triggerables;
    public int enterCount;
    public bool triggeredOnStay;
    private int count = 0;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == target) {
            count++;
            if (count >= enterCount) {
                foreach (Triggerable trigger in triggerables) {
                    trigger.TriggerEffect();
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!triggeredOnStay) return;

        if (other.tag == target) {
            count--;
        }
    }
}
