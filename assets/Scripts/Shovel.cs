using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shovel : Item {
    public LayerMask whatIsObstacle;
    [SerializeField]
    private float range = 1f;

    public AudioClip diggingClip;

    public override void Use() {
        // dig in front        
        if (Physics.Raycast(transform.position, transform.parent.transform.TransformDirection(Vector3.forward), out hit, range, whatIsObstacle)) {
            Earth earth = hit.collider.gameObject.GetComponent<Earth>();
            if (earth) {
                audioCenter.PlaySFX(diggingClip);
                earth.Dug();
            }
        }

        //dig below        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, range, whatIsObstacle)) {
            Earth earth = hit.collider.gameObject.GetComponent<Earth>();
            if (earth) {
                audioCenter.PlaySFX(diggingClip);
                earth.Dug();
            }
        }
    }

    public override void BeforeUse() {
        return;
    }
}
