using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeNyx : Nyx {

    protected virtual void Awake() {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update () {
        if (!gm.paused) {
            base.Update();
        }
    }
}
