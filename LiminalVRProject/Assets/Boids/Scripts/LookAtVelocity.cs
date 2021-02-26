using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtVelocity : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 10;
    private VelocityComponent _velocityComponent;

    void Awake()
    {
        _velocityComponent = GetComponent<VelocityComponent>();
    }

    void Update()
    {
        var targetDirection = _velocityComponent.Velocity;
        var lookAt = Quaternion.LookRotation(targetDirection.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAt, Time.deltaTime * _rotationSpeed);
    }

    public void SetSpeed(float speed)
    {
        _rotationSpeed = speed;
    }
}
