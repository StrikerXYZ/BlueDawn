using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class NeighborAlignment : MonoBehaviour
{
    VelocityComponent _velocityComponent;
    BoidNeighbor _boidNeighbor;

    void Awake()
    {
        _velocityComponent = GetComponent<VelocityComponent>();
        _boidNeighbor = GetComponent<BoidNeighbor>();
    }

    void Update()
    {
        var list = _boidNeighbor.GetVelocityComponents();

        if (list.Count > 0)
        {
            Vector3 velocity = Vector3.zero;
            foreach (var v in list)
            {
                velocity += v.Velocity;
            }

            velocity /= list.Count;
            _velocityComponent.Acceleration += (velocity - _velocityComponent.Velocity).normalized * _velocityComponent.MaxMagnitude;
        }
    }
}
