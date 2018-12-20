using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timmy : PlayerControler {
    public Transform holdPos;
    private Vector3 holdingRotPos = new Vector3(-69f, 280f, -78f);
    private GameObject nearbyItem;
    private Transform wallCheck;
    private bool usingItem = false;

    // Pour le glider 
    public bool _Gliding { get; set; }
    bool _CanCancelGlider { get; set; }
    bool _Carrying { get; set; }

    // pour courir
    public bool _Running { get; set; }
    public bool _HasStamina { get; set; }

    public GameObject glider;

    [SerializeField]
    float GliderDrag = 10.0f;

    [SerializeField]
    float GliderCancelDelay = 0.2f;

    [SerializeField]
    float GliderMoveSpeed = 8.0f;

    [SerializeField]
    float RunningMoveSpeed = 10.0f;

    [SerializeField]
    float KnockbackSpeed = 5.0f;

    [SerializeField]
    float KnockbackAngle = 45.0f;

    [SerializeField]
    LayerMask WhatIsItem;

    [SerializeField]
    LayerMask WhatIsWall;

    protected override void Awake() {
        base.Awake();
        _Anim = GetComponent<Animator>();
        _Gliding = false;
        _CanCancelGlider = false;
        _Running = false;
        _HasStamina = true;
        InstantiateGlider();
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();
        wallCheck = transform.Find("WallCheck");
    }

    // Update is called once per frame
    protected override void Update() {
        if (isSelected && !usingItem && !gm.paused) {
            base.Update();
            CheckWalled();
            CheckJump();

            CheckGlider();
            CheckRun();
            CheckPickup();
            CheckUseItem();
        }
    }

    protected override void HorizontalMove(float horizontal) {
        if (_Gliding) {
            horizontal = Input.GetAxis("Horizontal") * GliderMoveSpeed;
        } else if (_Running) {
            horizontal = Input.GetAxis("Horizontal") * RunningMoveSpeed;
        }
        base.HorizontalMove(horizontal);
        if (_Anim) _Anim.SetFloat("MoveSpeed", Mathf.Abs(horizontal));
    }

    // Gère le saut du personnage, ainsi que son animation de saut
    void CheckJump() {
        if (_Grounded) {
            if (Input.GetButtonDown("Jump")) {
                _Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                _Grounded = false;
                if (_Anim) {
                    _Anim.SetBool("Grounded", false);
                    _Anim.SetBool("Jump", true);
                }
            }
        }
    }

    // Collision avec le sol
    void OnCollisionEnter(Collision coll) {
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) == 0)
            return;

        if (coll.gameObject.tag == "Enemy") {
            Vector3 spiderToTimmy = coll.transform.position - _Rb.position;
            Vector3 direction = spiderToTimmy / spiderToTimmy.magnitude;
            float zDirection = direction.z / Mathf.Abs(direction.z);
            _Rb.velocity = new Vector3(_Rb.velocity.x, Mathf.Sin(KnockbackAngle * 2 * Mathf.PI / 360) * KnockbackSpeed, zDirection * Mathf.Cos(KnockbackAngle * 2 * Mathf.PI / 360) * KnockbackSpeed);
        }

        // Évite une collision avec le plafond
        if (coll.relativeVelocity.y > 0) {
            _Grounded = true;
            ResetGlider();
            if (_Anim) _Anim.SetBool("Grounded", _Grounded);
        }
    }

    // Gère l'action de glider du personnage 
    void CheckGlider() {
        if (_HasStamina)
        {
            if (!_Grounded && !_Gliding)
            {
                if (Input.GetButtonDown("Glider"))
                {
                    _Gliding = true;
                    _Rb.drag = GliderDrag;
                    StartCoroutine(CanCancelGliderAfter());
                    glider.SetActive(true);
                }
            }


            if (_CanCancelGlider && _Gliding)
            {
                if (Input.GetButtonDown("Glider"))
                {
                    ResetGlider();
                }
            }
        }
        else
        {
            StopCoroutine(CanCancelGliderAfter());
            ResetGlider();
        }
    }

    void ResetGlider()
    {
        _Gliding = false;
        _CanCancelGlider = false;
        _Rb.drag = -1;
        glider.SetActive(false);
    }

    void CheckRun() {
        if (_HasStamina)
        {
            if (_Grounded && !_Gliding)
            {
                if (Input.GetButton("Run"))
                {
                    _Running = true;
                }
                else
                {
                    _Running = false;
                }
            }
        }
        else
        {
            _Running = false;
        }
    }

    void CheckPickup() {
        if (Input.GetButtonDown("Pickup") && nearbyItem != null) {
            if (_Grounded && !_Carrying) {
                _Carrying = true;
                nearbyItem.GetComponent<Collider>().isTrigger = true;
                nearbyItem.GetComponent<Rigidbody>().isKinematic = true;
                nearbyItem.transform.parent = holdPos;
                nearbyItem.transform.position = holdPos.position;
                //nearbyItem.transform.eulerAngles = transform.eulerAngles + holdingRotPos;
                nearbyItem.GetComponent<Item>().Pickup(_Flipped);
            } else {
                _Carrying = false;
                nearbyItem.transform.parent = null;
                nearbyItem.GetComponent<Collider>().isTrigger = false;
                nearbyItem.GetComponent<Rigidbody>().isKinematic = false;
                nearbyItem.GetComponent<Item>().Drop();
            }
        }
    }

    void CheckUseItem()
    {
        if (_Anim.GetCurrentAnimatorStateInfo(1).IsName("Pickup") || _Anim.GetCurrentAnimatorStateInfo(1).IsName("Wave"))
            return;

        if (Input.GetButtonDown("UseItem") && _Carrying)
        {
            Item item = holdPos.GetComponentInChildren<Item>();
            switch (item.GetType().ToString()) {
                case "Shovel":
                    StartCoroutine(AnimatingUseItem());
                    break;
                case "Torch":
                    StartCoroutine(AnimatingUseItem());
                    break;
            }
        }
    }

    private void CheckWalled() {
        Collider[] colls = Physics.OverlapBox(wallCheck.position, new Vector3(0.2f, 0.5f, 0.05f), Quaternion.identity, WhatIsWall);
        _Walled = colls.Length != 0;
    }

    public void StopHorizontalMovement()
    {
        _Rb.velocity = new Vector3(0.0f, _Rb.velocity.y, 0.0f);
        if (_Anim) _Anim.SetFloat("MoveSpeed", 0.0f);
    }

    // Gère le délai avant de pouvoir annuler le gliding 
    IEnumerator CanCancelGliderAfter() {
        yield return new WaitForSeconds(GliderCancelDelay);
        _CanCancelGlider = true;
    }

    void OnTriggerStay(Collider coll) {
        if ((WhatIsItem & (1 << coll.gameObject.layer)) == 0)
            return;

        nearbyItem = coll.gameObject;
    }

    void OnTriggerExit(Collider coll) {
        if ((WhatIsItem & (1 << coll.gameObject.layer)) == 0)
            return;
        
        nearbyItem = null;
    }

    void InstantiateGlider()
    {
        glider = Instantiate(glider, new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z), Quaternion.identity, transform);
        glider.transform.Rotate(new Vector3(90f, 0f, 0f));
        glider.transform.localScale = new Vector3(0.5f, 0.5f, 0.2f);
        glider.SetActive(false);
    }

    private IEnumerator AnimatingUseItem() {
        usingItem = true;
        _Rb.velocity = Vector3.zero;

        Item item = holdPos.GetComponentInChildren<Item>();
        switch (item.GetType().ToString()) {
            case "Shovel":
                _Anim.SetBool("Pickup", true);
                item.BeforeUse();
                break;
            case "Torch":
                _Anim.SetBool("Wave", true);
                item.BeforeUse();
                break;
        }

        do {
            yield return null;
        } while (_Anim.GetCurrentAnimatorStateInfo(1).IsName("Pickup") || _Anim.GetCurrentAnimatorStateInfo(1).IsName("Wave"));

        item.Use();

        usingItem = false;

        yield break;
    }
}
