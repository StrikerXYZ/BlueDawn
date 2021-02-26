using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class NeighborCohesion: MonoBehaviour
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
            Vector3 position = Vector3.zero;
            foreach (var v in list)
            {
                position += v.transform.position;
            }
            position /= list.Count;
            _velocityComponent.Acceleration += (position - transform.position).normalized * _velocityComponent.MaxMagnitude - _velocityComponent.Velocity;
        }
    }
}
