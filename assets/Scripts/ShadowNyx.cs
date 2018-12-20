using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShadowSurface
{
    Bottom,
    Back,
    Left,
    Right
}

public class ShadowNyx : Nyx {

    private Transform _Tr;
    private ShadowSurface CurrentSurface = ShadowSurface.Bottom;
    private ShadowSurface OldSurface = ShadowSurface.Bottom;
    private Vector3 VerticalDirection;
    private Vector3 HorizontalDirection;
    private Vector3 SurfaceDirection;
    //private Vector3 NewVerticalDirection;
    //private Vector3 NewHorizontalDirection;
    private Vector3 LastValidPosition;

    [SerializeField]
    private float Gravity = 9.8f;

    protected virtual void Awake()
    {
        base.Awake();
        _Tr = GetComponent<Transform>();
        HorizontalDirection = new Vector3(0,0,1);
        VerticalDirection = new Vector3(-1,0,0);
        SurfaceDirection = new Vector3(0,-1,0);
        LastValidPosition = _Tr.position;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!gm.paused) {
            var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
            var vertical = Input.GetAxis("Vertical") * MoveSpeed;
            Move2D(horizontal, vertical);
            _Rb.AddForce(SurfaceDirection * Gravity, ForceMode.Acceleration);

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
                UpdateDefaultMovementDirection();
            }

            if (Input.GetButtonDown("ShadowMode")) {
                //_Tr.position = new Vector3(-2, _Tr.position.y, _Tr.position.z);
                gameObject.SetActive(false);
                UpdateParentPosition();
                gm.ShadowMode();
            }

            StayInShadow();
        }
    }

    // Collision avec la surface ombragée
    protected void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "SMDirectionTrigger")
        {
            SMDirections directionScript = coll.GetComponent<SMDirections>();
            OldSurface = CurrentSurface;
            CurrentSurface = directionScript.getNewSurface(CurrentSurface);
            UpdateDirectMovementDirection();
        }
    }

    //Movement mode is different, Horizontal move shouldn't do anything.
    protected override void HorizontalMove(float horizontal)
    {

    }
    
    protected void Move2D(float horizontal, float vertical)
    {
        _Rb.velocity = horizontal * HorizontalDirection + vertical * VerticalDirection;
    }

    private void UpdateDirectMovementDirection()
    {
        switch (CurrentSurface)
        {
            case ShadowSurface.Bottom:
                HorizontalDirection = new Vector3(0,0,1);
                VerticalDirection = new Vector3(-1,0,0);
                SurfaceDirection = new Vector3(0, -1, 0);
                break;
            case ShadowSurface.Back:
                switch (OldSurface)
                {
                    case ShadowSurface.Right:
                        
                        if(VerticalDirection.x == -1)
                        {
                            HorizontalDirection = new Vector3(0, 1, 0);
                            VerticalDirection = new Vector3(0, 0, -1);
                        }
                        else
                        {
                            HorizontalDirection = new Vector3(0, 0, 1);
                            VerticalDirection = new Vector3(0, 1, 0);
                        }
                        break;
                    case ShadowSurface.Left:

                        if(VerticalDirection.x == -1)
                        {
                            HorizontalDirection = new Vector3(0, 1, 0);
                            VerticalDirection = new Vector3(0, 0, 1);
                        }
                        else
                        {
                            HorizontalDirection = new Vector3(0, 0, 1);
                            VerticalDirection = new Vector3(0, 1, 0);
                        }
                        break;
                    default:
                        HorizontalDirection = new Vector3(0, 0, 1);
                        VerticalDirection = new Vector3(0, 1, 0);
                        break;
                }
                SurfaceDirection = new Vector3(-1, 0, 0);
                break;
            case ShadowSurface.Left:
                if(OldSurface == ShadowSurface.Back && HorizontalDirection.z == 1)
                {
                    VerticalDirection = new Vector3(0, 1, 0);
                    HorizontalDirection = new Vector3(-1, 0, 0);
                }
                else
                {
                    VerticalDirection = new Vector3(-1, 0, 0);
                    HorizontalDirection = new Vector3(0, -1, 0);
                }
                SurfaceDirection = new Vector3(0, 0, -1);
                break;
            case ShadowSurface.Right:
                if (OldSurface == ShadowSurface.Back && HorizontalDirection.z == 1)
                {
                    VerticalDirection = new Vector3(0, 1, 0);
                    HorizontalDirection = new Vector3(1, 0, 0);
                }
                else
                {
                    VerticalDirection = new Vector3(-1, 0, 0);
                    HorizontalDirection = new Vector3(0, 1, 0);
                }
                SurfaceDirection = new Vector3(0, 0, 1);
                break;
        }
    }

    // We reset only on the backwall, from testing, it's what's most intuitive.
    private void UpdateDefaultMovementDirection()
    {
        switch (CurrentSurface)
        {
            case ShadowSurface.Back:
                HorizontalDirection = new Vector3(0, 0, 1);
                VerticalDirection = new Vector3(0, 1, 0);
                SurfaceDirection = new Vector3(-1, 0, 0);
                break;
        }
    }

    private void StayInShadow()
    {
        RaycastHit hit;
        bool isInShadow = false;
        if(Physics.Raycast(_Tr.position, SurfaceDirection, out hit))
        {
            if(hit.collider.tag == "InShadow")
            {
                LastValidPosition = _Tr.position;
                isInShadow = true;
            }
        }


        
        if(!isInShadow)
        {
            _Tr.position = LastValidPosition;
            _Rb.velocity = new Vector3(0,0,0);
        }
    }

    private void OnEnable() {
        LastValidPosition = _Tr.position;
        CurrentSurface = ShadowSurface.Bottom;
        OldSurface = ShadowSurface.Bottom;
        HorizontalDirection = new Vector3(0, 0, 1);
        VerticalDirection = new Vector3(-1, 0, 0);
        SurfaceDirection = new Vector3(0, -1, 0);
    }
}
