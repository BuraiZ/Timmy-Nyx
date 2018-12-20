using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiderling : MonoBehaviour {
    public LayerMask whatToIgnore;

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float walkDuration = 2f;     // seconds
    [SerializeField]
    private float idleDuration = 2f;     // seconds
    private bool _Flipped;

    private RaycastHit hit;
    private Animator anim;
    private Rigidbody rb;
    private Collider coll;

    private State state;
    private bool facingRight;
    private float startTime;

    // Use this for initialization
    void Start() {
        _Flipped = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        state = State.idle;
    }

    // Update is called once per frame
    void Update() {
        NextMove();
        CheckObstacles();
    }

    private void CheckObstacles() {
        // Check obstacles in front
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + coll.bounds.size.y / 2, transform.position.z + transform.forward.z * coll.bounds.size.z / 2);
        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.forward), out hit, 0.5f, ~whatToIgnore)) {
            state = State.idle;
            FlipAvatar(facingRight ? false : true);
        }

        // Check for platform edges
        pos = new Vector3(transform.position.x, transform.position.y + coll.bounds.size.y / 2, transform.position.z + transform.forward.z * (coll.bounds.size.z / 2 + 0.5f));
        if (!Physics.Raycast(pos, transform.TransformDirection(Vector3.down), out hit, 0.5f, ~whatToIgnore)) {
            state = State.idle;
            FlipAvatar(facingRight ? false : true);
        }
    }

    private void NextMove() {
        switch (state) {
            case State.idle:
                Idle();
                break;
            case State.walk:
                Walk();
                break;
        }
    }

    private void Idle() {
        if (!anim.IsInTransition(0)) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idle")) {
                anim.SetTrigger("idle");
                startTime = Time.time;
                rb.velocity = Vector3.zero;
            } else if (Time.time - startTime >= idleDuration) {
                startTime = 0;
                state = State.walk;
            }
        }
    }

    private void Walk() {
        if (!anim.IsInTransition(0)) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("walk")) {
                anim.SetTrigger("walk");
                startTime = Time.time;

                bool isHeadingRight;
                if (Random.Range(0.0f, 1.0f) < 0.5f) {
                    isHeadingRight = true;
                } else {
                    isHeadingRight = false;
                }
                FlipAvatar(isHeadingRight);
            } else if (Time.time - startTime >= walkDuration) {
                startTime = 0;
                state = State.idle;
            }
        }

        rb.velocity = transform.forward * speed;
    }

    private void FlipAvatar(bool isHeadingRight) {
        if (isHeadingRight && !facingRight) {
            facingRight = true;
            transform.Rotate(new Vector3(0, 180, 0));
        } else if (!isHeadingRight && facingRight) {
            facingRight = false;
            transform.Rotate(-new Vector3(0, 180, 0));
        }
    }
}
