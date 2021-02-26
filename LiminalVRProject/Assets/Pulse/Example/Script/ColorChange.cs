using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour, IPulseListener
{
    [SerializeField]
    Color defaultColor = Color.white;

    [SerializeField]
    Color activeColor = Color.red;

    Material mat;

    void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void OnPulseRim(float value)
    {
        mat.color = Color.Lerp(defaultColor, activeColor, value);
    }

    public void OnPulseScale(float value)
    {

    }
}