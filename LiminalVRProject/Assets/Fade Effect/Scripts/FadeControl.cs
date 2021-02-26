using Liminal.Core.Fader;
using UnityEngine;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour, IPulseListener
{
    [SerializeField]
    Color lightColor_;

    [SerializeField]
    Color darkColor_;

    [SerializeField]
    Image targetImage;

    public void OnPulseRim(float value)
    {

    }

    public void OnPulseScale(float value)
    {
        targetImage.color = Color.Lerp(darkColor_, lightColor_, value);
        //ScreenFader.Instance.FadeTo(Color.Lerp(darkColor_, lightColor_, value));
    }
}

