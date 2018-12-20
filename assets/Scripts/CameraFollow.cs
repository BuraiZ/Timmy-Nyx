using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public GameObject target;       //Public variable to store a reference to the player game object
    public Transform limitPoint;
    [SerializeField]
    private bool fixedCamera = true;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start() {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        if (fixedCamera) transform.position = new Vector3(transform.position.x, target.transform.position.y + 1, target.transform.position.z);
        offset = transform.position - target.transform.position;
    }

    void Update() {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        float posY = target.transform.position.y;
        float posZ = target.transform.position.z;

        if (target.transform.position.y <= limitPoint.position.y) {
            posY = limitPoint.position.y;
        }
        if (target.transform.position.z >= limitPoint.position.z) {
            posZ = limitPoint.position.z;
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, posY, posZ) + offset, Time.deltaTime);
        //transform.position = target.transform.position + offset;
    }

    public void SwitchFocusTo(GameObject newTarget) {
        target = newTarget;
    }

    private void UpdateBackground() {

    }
}