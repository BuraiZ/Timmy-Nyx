using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : Triggerable {
    public GameObject spawnObject;

    // Use this for initialization
    void Start() {
    }

    protected override void SetInitialState() {
    }

    public override void TriggerEffect() {
        Instantiate(spawnObject, transform);
    }
}
