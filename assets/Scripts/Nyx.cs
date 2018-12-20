using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nyx : PlayerControler {
    [SerializeField]
    LayerMask WhatIsPossessable;

    public AudioCenter audioCenter;
    public AudioClip courageDropClip;
    public AudioClip courageSodaClip;
    public AudioClip invalidClip;

    protected GameManager gm;
    public bool _inShadow = false;

    protected override void Awake() {
        base.Awake();
        _Rb = GetComponentInParent<Rigidbody>();
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();
        _Anim = GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();
	}

    // Update is called once per frame
    protected override void Update () {
        if (isSelected) {
            base.Update();

            if (Input.GetButtonDown("Unpossession") || Input.GetAxis("Unpossession") > 0) {
                Unpossession();
            }
            
            if (Input.GetButtonDown("ShadowMode"))
            {
                if (_inShadow)
                {
                    gameObject.SetActive(false);
                    UpdateParentPosition();
                    gm.ShadowMode();
                }
                else
                {
                    audioCenter.PlaySFX(invalidClip);
                }
            }
        }
	}

    public void Unpossession()
    {
        gameObject.SetActive(false);
        UpdateParentPosition();
        gm.ChangeNyxFormTo(0);
    }

    protected virtual void OnTriggerStay(Collider coll) {
        if ((WhatIsPossessable & (1 << coll.gameObject.layer)) == 0)
            return;
        
        if (Input.GetButtonDown("Possession")) {
            gameObject.SetActive(false);

            UpdateParentPosition();
            gm.ChangeNyxFormTo(coll.gameObject.GetComponent<Possessable>().form);
        }
    }

    public void IsActive(bool isActive) {
        // get parent position
        transform.position = transform.parent.position;
        gameObject.SetActive(isActive);
    }

    protected override void HorizontalMove(float horizontal) {
        base.HorizontalMove(horizontal);
    }
    
    protected void OnCollisionEnter(Collision coll) {
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) != 0)
        {
            // Évite une collision avec le plafond
            if (coll.relativeVelocity.y >= 0)
            {
                _Grounded = true;
                if (_Anim) _Anim.SetBool("Grounded", _Grounded);
            }
        }

        if (coll.gameObject.tag == "CourageDrop") {
            audioCenter.PlaySFX(courageDropClip);
            TimmyCourage timmyCourage = (TimmyCourage)FindObjectOfType(typeof(TimmyCourage));
            timmyCourage.AddCourage(10);
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "CourageSoda") {
            audioCenter.PlaySFX(courageSodaClip);
            TimmyCourage timmyCourage = (TimmyCourage)FindObjectOfType(typeof(TimmyCourage));
            timmyCourage.IncreaseMaxCourage(20);
            Destroy(coll.gameObject);
        }
    }

    protected void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "InShadow")
        {
            _inShadow = true;
        }
    }

    protected void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag == "InShadow")
        {
            _inShadow = false;
        }
    }

    public void UpdateParentPosition() {
        // save child position to parent
        transform.parent.position = transform.position;
    }

    private void OnDisable() {
        _inShadow = false;
    }
}
