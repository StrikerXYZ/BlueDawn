using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    float fps;
    float cpu;
    float gpu;

    void Update()
    {
        fps = 1 / Time.unscaledDeltaTime;
        cpu = FrameTimingManager.GetCpuTimerFrequency();
        gpu = FrameTimingManager.GetGpuTimerFrequency();
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "FPS: "+ fps);
        GUI.Label(new Rect(10, 30, 100, 20), "CPU: "+ cpu);
        GUI.Label(new Rect(10, 50, 100, 20), "GPU: "+ gpu);
    }
}
