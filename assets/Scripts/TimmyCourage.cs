using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimmyCourage : MonoBehaviour {

    [SerializeField]
    public float maxCourage = 100;

    [SerializeField]
    public float InvincibilityTime = 1.0f;

    public float InvincibilityLeft = 0f;
    private float courage;
    public Slider courageSlider;
    PlayerControler _controler;

    public AudioCenter audioCenter;
    public AudioClip courageDropClip;
    public AudioClip courageSodaClip;
    public AudioClip damageClip;

    void Awake()
    {
        _controler = GetComponent<PlayerControler>();
        courage = maxCourage;
        RectTransform courageSliderTransform = courageSlider.GetComponent<RectTransform>();
        courageSliderTransform.sizeDelta = new Vector2(maxCourage * 2, courageSliderTransform.sizeDelta.y);
        courageSliderTransform.anchoredPosition = new Vector2(maxCourage + 15.0f, courageSliderTransform.anchoredPosition.y);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Enemy" && InvincibilityLeft <= 0) {
            audioCenter.PlaySFX(damageClip);
            InvincibilityLeft = InvincibilityTime;
            courage -= 10;
        }

        if (coll.gameObject.tag == "CourageDrop")
        {
            audioCenter.PlaySFX(courageDropClip);
            AddCourage(10);
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.tag == "CourageSoda") {
            audioCenter.PlaySFX(courageSodaClip);
            IncreaseMaxCourage(20);
            Destroy(coll.gameObject);
        }
    }

    //Collision avec un ennemi
    void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "Enemy" && InvincibilityLeft <= 0) {
            audioCenter.PlaySFX(damageClip);
            InvincibilityLeft = InvincibilityTime;
            courage -= 10;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        InvincibilityLeft = InvincibilityLeft <= 0 ? InvincibilityLeft : InvincibilityLeft - Time.deltaTime;
        if(courage <= 0)
        {
            FindObjectOfType<GameManager>().TriggerLoseCondition();
        }
        courageSlider.value = courage;
	}

    public void AddCourage(int amount)
    {
        courage += amount;
        if (courage > maxCourage)
            courage = maxCourage;
    }

    public void IncreaseMaxCourage(int amount)
    {
        maxCourage += amount;
        courage = maxCourage;
        courageSlider.maxValue = maxCourage;
        RectTransform courageSliderTransform = courageSlider.GetComponent<RectTransform>();
        courageSliderTransform.sizeDelta = new Vector2(maxCourage * 2, courageSliderTransform.sizeDelta.y);
        courageSliderTransform.anchoredPosition = new Vector2(maxCourage + 15.0f, courageSliderTransform.anchoredPosition.y);
    }
}
