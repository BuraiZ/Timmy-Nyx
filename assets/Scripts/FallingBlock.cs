using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour {
    [SerializeField]
    private float timeBeforeFall = 1;
    [SerializeField]
    private float restoreTime = 5;
    private Vector3 initPos;
    private float startTime = -1;
    private bool triggered = false;

    private Rigidbody rb;
    private Collider coll;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        initPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && !triggered) {
            StartCoroutine(TriggerFall());
            StartCoroutine(TriggerRestore());
        }
    }

    IEnumerator TriggerFall() {
        yield return new WaitForSeconds(timeBeforeFall);
        rb.isKinematic = false;
    }

    IEnumerator TriggerRestore() {
        triggered = true;
        yield return new WaitForSeconds(restoreTime);
        rb.isKinematic = true;
        transform.position = initPos;
        triggered = false;
    }
}
