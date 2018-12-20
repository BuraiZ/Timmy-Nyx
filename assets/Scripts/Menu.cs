using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour {
    public EventSystem eventSystem;
    public GameObject pointer;
    public Button[] buttons;

    private Vector3 pointerOffset = new Vector3(-50, 0, 0);

    public void SetPointer(Transform selected) {
        if (pointer)
            pointer.transform.position = selected.position + pointerOffset;
    }

    private void OnEnable() {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(buttons[0].gameObject);
        if (pointer) pointer.transform.position = buttons[0].transform.position + pointerOffset;
    }
}
