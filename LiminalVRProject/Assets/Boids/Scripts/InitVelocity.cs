using UnityEngine;

public class InitVelocity : MonoBehaviour
{
    [SerializeField]
    private float _maxMagnitude;

    private VelocityComponent _velocityComponent;

    void Awake()
    {
        _velocityComponent = GetComponent<VelocityComponent>();
    }

    void Start()
    {
        _velocityComponent.Velocity = GetRandomVector();
    }

    Vector3 GetRandomVector()
    {
        return new Vector3(Random.Range(-_maxMagnitude, _maxMagnitude), Random.Range(-_maxMagnitude, _maxMagnitude), Random.Range(-_maxMagnitude, _maxMagnitude));
    }
}
