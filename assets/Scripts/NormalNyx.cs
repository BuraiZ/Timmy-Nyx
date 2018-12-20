using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalNyx : Nyx {
    public Transform holdPos;
    private Vector3 holdingRotPos = new Vector3(-69f, 280f, -78f);
    private GameObject nearbyItem;
    bool _Carrying { get; set; }
    private bool usingItem = false;

    [SerializeField]
    LayerMask WhatIsItem;

    protected virtual void Awake() {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update () {
        if (!gm.paused && !usingItem) {
            base.Update();
            CheckPickup();
            CheckUseItem();
        }
    }

    protected override void OnTriggerStay(Collider coll) {
        base.OnTriggerStay(coll);

        if ((WhatIsItem & (1 << coll.gameObject.layer)) == 0)
            return;

        nearbyItem = coll.gameObject;
    }

    void OnTriggerExit(Collider coll) {
        if ((WhatIsItem & (1 << coll.gameObject.layer)) == 0)
            return;

        nearbyItem = null;
    }

    protected override void HorizontalMove(float horizontal) {
        base.HorizontalMove(horizontal);
        if (_Anim) _Anim.SetFloat("MoveSpeed", Mathf.Abs(horizontal));
    }

    void CheckPickup() {
        if (Input.GetButtonDown("Pickup")) {
            if (_Grounded && !_Carrying) {
                _Carrying = true;
                nearbyItem.transform.parent = holdPos;
                nearbyItem.transform.position = holdPos.position;
                //nearbyItem.transform.eulerAngles = transform.eulerAngles + holdingRotPos;
                nearbyItem.GetComponent<Item>().Pickup(_Flipped);
            } else {
                _Carrying = false;
                nearbyItem.GetComponent<Item>().Drop();
            }
        }
    }

    void CheckUseItem() {
        if (_Anim.GetCurrentAnimatorStateInfo(1).IsName("Pickup") || _Anim.GetCurrentAnimatorStateInfo(1).IsName("Wave"))
            return;

        if (Input.GetButtonDown("UseItem") && _Carrying) {
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

    private void OnEnable() {
        usingItem = false;
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
