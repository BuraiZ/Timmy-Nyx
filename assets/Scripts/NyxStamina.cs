using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NyxStamina : MonoBehaviour {

    [SerializeField]
    public int maxStamina = 60;

    private float stamina;
    public Slider staminaSlider;
    
    private GameManager gm;

    void Awake() {
        gm = FindObjectOfType<GameManager>();
        staminaSlider.transform.parent.gameObject.SetActive(false);
        RectTransform staminaSliderTransform = staminaSlider.GetComponent<RectTransform>();
        //staminaSliderTransform.sizeDelta = new Vector2(maxStamina*2, staminaSliderTransform.sizeDelta.y);
        //staminaSliderTransform.anchoredPosition = new Vector2(maxStamina + 15.0f, staminaSliderTransform.anchoredPosition.y);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (gm.IsPlayingTimmy()) return;

        if(gm.GetNyxSelected() != NyxForm.shadow)
            stamina -= 2.0f*Time.deltaTime;

        if (stamina <= 0) {
            stamina = 0;
            gm.CharacterChange();
        }
        staminaSlider.value = stamina;
    }

    void OnEnable() {
        stamina = maxStamina;
        staminaSlider.maxValue = stamina;
        staminaSlider.transform.parent.gameObject.SetActive(true);
    }

    void OnDisable() {
        staminaSlider.transform.parent.gameObject.SetActive(false);
    }
}
