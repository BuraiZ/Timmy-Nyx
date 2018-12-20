using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
    protected bool _isHeld = false;

    [SerializeField]
    private Vector3 holdingRotPos = new Vector3(-69f, 280f, -78f);

    protected RaycastHit hit;

    public AudioCenter audioCenter;

    public void Pickup(bool flipped) {
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;

        Transform transform = GetComponent<Transform>();
        if(flipped)
            transform.eulerAngles = new Vector3(-holdingRotPos.x, holdingRotPos.y, holdingRotPos.z);
        else
            transform.eulerAngles = holdingRotPos;
        _isHeld = true;
    }

    public void Drop() {
        transform.parent = null;
        GetComponent<Collider>().isTrigger = false;
        GetComponent<Rigidbody>().isKinematic = false;
        _isHeld = false;
    }
    public abstract void BeforeUse();
    public abstract void Use();
}
