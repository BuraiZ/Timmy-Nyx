using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour {
    public NyxForm form;
    public Material normal;
    public Material possessable;

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.GetComponent<Nyx>()) {
            if (GetComponent<Renderer>()) {
                GetComponent<Renderer>().material = possessable;
            } else {
                GetComponentInChildren<Renderer>().material = possessable;
            }
        }
    }

    void OnTriggerExit(Collider coll) {
        if (coll.gameObject.GetComponent<Nyx>()) {
            if (GetComponent<Renderer>()) {
                GetComponent<Renderer>().material = normal;
            } else {
                GetComponentInChildren<Renderer>().material = normal;
            }
        }
    }
}
