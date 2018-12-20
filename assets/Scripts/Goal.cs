using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public GameObject clearMessage;
    public LevelManager lm;

    [SerializeField]
    private float clearPopupTimer = 2;

    [SerializeField]
    private float popupDurationBeforeNextLevel = 2;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            StartCoroutine(DisplayClear());
        }
    }

    IEnumerator DisplayClear() {
        yield return new WaitForSeconds(clearPopupTimer);
        clearMessage.SetActive(true);
        yield return new WaitForSeconds(popupDurationBeforeNextLevel);
        lm.LoadMenuScene();
    }
}
