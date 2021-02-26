using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CausticEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public float animFrames = 0.5f;         //footage fps
    public Texture2D[] frames;      //caustics images

    Projector projector;    //Projector GameObject
    int currentFrame = 0;
    float frameTime = 0;

    void Awake()
    {
        projector = GetComponent<Projector>();
        frameTime = animFrames;
    }

    void Update()
    {
        //float sinValue = Mathf.Sin(Time.time * Mathf.PI / animFrames);
        //int currentFrame = ((int)((sinValue) * frames.Length/2 + frames.Length / 2)) % frames.Length;
        frameTime -= Time.deltaTime;
        if (frameTime <= 0)
        {
            frameTime = animFrames;
            currentFrame += 1;
            currentFrame %= frames.Length;
            projector.material.SetTexture("_MainTex", frames[currentFrame]);
        }

    }
}
