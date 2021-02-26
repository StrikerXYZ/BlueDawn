using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPulseListener
{
    void OnPulseRim(float value);

    void OnPulseScale(float value);
}
