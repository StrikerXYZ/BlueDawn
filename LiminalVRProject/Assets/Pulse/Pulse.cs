using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pulse : MonoBehaviour
{
    [SerializeField]
    float minSize = 1.0f;

    [SerializeField]
    float maxSize = 10.0f;

    [SerializeField]
    float rimSize = 1.0f;

    [SerializeField]
    float breathInPeriod = 3;
    [SerializeField]
    float breathOutPeriod = 7;

    [SerializeField]
    AudioSource breathSound;
    bool isPlayed = false;

    [SerializeField]
    Projector projector;

    float period;

    List<IPulseListener> listeners = new List<IPulseListener>();

    private void Awake()
    {
        CaptureListeners();
    }

    private void Start()
    {
        CaptureListeners();
    }

    void Update()
    {
        float breathRatio = period % 2;
        if (breathRatio < 1)
        {
            if(!isPlayed)
            {
                breathSound.Play();
                isPlayed = true;
            }
            period += Time.deltaTime / breathInPeriod;
        }
        else
        {
            if (isPlayed)
            {
                isPlayed = false;
            }
            period += Time.deltaTime / breathOutPeriod;
        }
        

        float scale = minSize + maxSize/2 + (maxSize/2 * Mathf.Cos(Mathf.PI * period));
        transform.localScale = scale * Vector3.one;

        var pulseScalePercent = Mathf.Abs(breathRatio - 1);
        foreach (var listener in listeners)
        {
            var pos = (listener as MonoBehaviour).transform.position;
            var dist = Vector3.Distance(transform.position, pos);
            if (dist > scale + rimSize)
            {
                listener.OnPulseRim(0);
            }
            else if (dist < scale - rimSize)
            {
                listener.OnPulseRim(1);
            }
            else
            {
                listener.OnPulseRim((scale - dist) * 2 / rimSize + 1);
            }


            listener.OnPulseScale(pulseScalePercent);
        }

        projector.orthographicSize = scale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, 1.0f);
    }

    void CaptureListeners()
    {
        listeners.Clear();
        var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in rootObjs)
        {
            listeners.AddRange(root.GetComponentsInChildren<IPulseListener>());
        }
    }
}
