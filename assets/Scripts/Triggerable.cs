using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Triggerable : MonoBehaviour {
    protected abstract void SetInitialState();
    public abstract void TriggerEffect();
}
