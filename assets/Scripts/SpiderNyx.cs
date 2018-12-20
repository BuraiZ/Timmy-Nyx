using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderNyx : Nyx {
    private Animator anim;

    protected virtual void Awake() {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update() {
        if (!gm.paused) {
            base.Update();
        }
    }

    protected override void HorizontalMove(float horizontal) {
        base.HorizontalMove(horizontal);

        if (horizontal == 0f) {
            anim.SetTrigger("idle");
        } else {
            anim.SetTrigger("walk");
        }
    }
}
