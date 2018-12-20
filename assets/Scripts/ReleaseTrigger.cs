using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ReleaseTrigger : Triggerable {
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        SetInitialState();
    }

    protected override void SetInitialState() {
        rb.isKinematic = true;
    }

    public override void TriggerEffect() {
        rb.isKinematic = false;
    }
}
