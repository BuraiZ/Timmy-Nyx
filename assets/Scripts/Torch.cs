using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Item {
    public LayerMask whatIsObstacle;
    public string whatCanCounter;
    public ParticleSystem fireBurning;

    public AudioClip spiderwebClearClip;

    [SerializeField]
    private float range = 1f;

    public override void Use() {
        if (Physics.Raycast(transform.position, transform.parent.transform.TransformDirection(Vector3.forward), out hit, range, whatIsObstacle)) {
            if (hit.transform.tag == whatCanCounter) {
                audioCenter.PlaySFX(spiderwebClearClip);
                Destroy(hit.collider.gameObject);
                Destroy(this.gameObject);
            }
        }        
    }

    public override void BeforeUse() {
        if (Physics.Raycast(transform.position, transform.parent.transform.TransformDirection(Vector3.forward), out hit, range, whatIsObstacle)) {
            if (hit.transform.tag == whatCanCounter) {
                Vector3 targetBound = hit.transform.gameObject.GetComponent<BoxCollider>().bounds.size;
                Instantiate(fireBurning, new Vector3(hit.transform.position.x, hit.transform.position.y - targetBound.y / 2, hit.transform.position.z), Quaternion.identity, hit.transform);
            }
        }
    }
}
