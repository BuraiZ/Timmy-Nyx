using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimmyStamina : MonoBehaviour
{
    [SerializeField]
    public float maxStamina = 100f;

    [SerializeField]
    public float StaminaRecoveryDelay = 2.0f;

    [SerializeField]
    public float StaminaRecoveryRate = 5.0f;

    [SerializeField]
    public float StaminaGlidingDecrease = 20.0f;

    [SerializeField]
    public float StaminaRunningDecrease = 10.0f;

    public float stamina;

    private float StaminaRecoveryTimer = 0.0f;

    public Slider staminaSlider;
    Timmy _controler;

    void Awake()
    {
        _controler = GetComponent<Timmy>();
        stamina = maxStamina;
    }

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        /*Debug.Log("Gliding status: " + _controler._Gliding);
        Debug.Log("Running status: " + _controler._Running);
        Debug.Log("Has stamina: " + _controler._HasStamina);*/

        checkStamina();

        if (_controler._Gliding)
        {
            stamina = Mathf.Clamp(stamina - (StaminaGlidingDecrease * Time.deltaTime), 0.0f, maxStamina);
            StaminaRecoveryTimer = 0.0f;
        }
        else if (_controler._Running)
        {
            stamina = Mathf.Clamp(stamina - (StaminaRunningDecrease * Time.deltaTime), 0.0f, maxStamina);
            StaminaRecoveryTimer = 0.0f;
        }
        else
        {
            recoverStamina();
        }


        staminaSlider.value = stamina;
    }

    void checkStamina()
    {
        if (stamina <= 0)
        {
            _controler._HasStamina = false;
        }
        else
        {
            _controler._HasStamina = true;
        }
    }

    // Gère l'usage de stamina par une action quelconque
    void depleteStamina(float value)
    {
        stamina -= value;
    }

    // Gère la régénération de stamina au fil du temps
    void recoverStamina()
    {
        if (StaminaRecoveryTimer >= StaminaRecoveryDelay)
        {
            stamina = Mathf.Clamp(stamina + (StaminaRecoveryRate * Time.deltaTime), 0.0f, maxStamina);
        }
        else
        {
            StaminaRecoveryTimer += Time.deltaTime;
        }
    }
}
