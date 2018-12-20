using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMDirections : MonoBehaviour
{
    [SerializeField]
    ShadowSurface _surface1;

    [SerializeField]
    ShadowSurface _surface2;

    public ShadowSurface getNewSurface(ShadowSurface surface)
    {
        if (surface == _surface1)
        {
            return _surface2;
        }
        else
        {
            return _surface1;
        }
    }
}
