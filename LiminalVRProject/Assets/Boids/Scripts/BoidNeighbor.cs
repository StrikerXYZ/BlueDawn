using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BoidNeighbor : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)]
    float _alignment = 1;

    [SerializeField, Range(0.0f, 1.0f)]
    float _cohesion = 1;

    [SerializeField, Range(0.0f, 1.0f)]
    float _seperation = 1;

    [SerializeField]
    float _avoidance = 1;

    public int fishType = 0;

    Dictionary<int, VelocityComponent> _dictionary = new Dictionary<int, VelocityComponent>();
    VelocityComponent _velocityComponent;
    BoidNeighbor _boidNeighbor;
    WrapSpace _space;

    public AvoidanceSphere[] avoidance;

    void Awake()
    {
        _velocityComponent = GetComponent<VelocityComponent>();
        _boidNeighbor = GetComponent<BoidNeighbor>();
        _space = GetComponent<WrapSpace>();
    }

    void OnCollisionEnter(Collision collision)
    {
        var velocityComponent = collision.collider.GetComponent<VelocityComponent>();
        var neighborComponent = collision.collider.GetComponent<BoidNeighbor>();
        if (velocityComponent)
        {
            if (Vector3.Dot(transform.forward, collision.collider.transform.forward) > -0.5)
            {
                if (!_dictionary.ContainsKey(collision.collider.GetInstanceID()) && neighborComponent.fishType == fishType)
                {
                    _dictionary.Add(collision.collider.GetInstanceID(), velocityComponent);
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (_dictionary.ContainsKey(collision.collider.GetInstanceID()))
            _dictionary.Remove(collision.collider.GetInstanceID());
    }

    public List<VelocityComponent> GetVelocityComponents()
    {
        return _dictionary.Values.ToList();
    }

    void Update()
    {
        var list = _boidNeighbor.GetVelocityComponents();

        Vector3 boidForce = Vector3.zero;
        if (list.Count > 0)
        {
            boidForce = Alignment(list) * _alignment + Cohesion(list) * _cohesion + Seperation(list) * _seperation;
        }

        var avoidForce = AvoidanceForce();
        float avoidance = avoidForce.magnitude;
        boidForce = Vector3.Lerp(boidForce, avoidForce * _velocityComponent.MaxMagnitude, avoidance);

        Vector3 envForce = AvoidBounds();
        float strength = envForce.magnitude / _velocityComponent.MaxMagnitude;
        _velocityComponent.Acceleration = Vector3.Lerp(boidForce, envForce, strength);
    }

    Vector3 Alignment(List<VelocityComponent> list)
    {
        Vector3 velocity = Vector3.zero;
        foreach (var v in list)
        {
            velocity += v.Velocity;
        }
        velocity /= list.Count;
        return (velocity - _velocityComponent.Velocity).normalized * _velocityComponent.MaxMagnitude;
    }

    Vector3 Cohesion(List<VelocityComponent> list)
    {
        Vector3 position = Vector3.zero;
        foreach (var v in list)
        {
            position += v.transform.position;
        }
        position /= list.Count;
        return (position - transform.position).normalized * _velocityComponent.MaxMagnitude - _velocityComponent.Velocity;
    }

    Vector3 Seperation(List<VelocityComponent> list)
    {
        Vector3 positionDiff = Vector3.zero;
        foreach (var v in list)
        {
            positionDiff += (transform.position - v.transform.position);
            positionDiff = positionDiff / positionDiff.magnitude;
        }
        positionDiff /= list.Count;
        return positionDiff.normalized * _velocityComponent.MaxMagnitude;
    }

    Vector3 AvoidBounds()
    {
        Bounds bounds = _space.GetBounds();

        Vector3 position = transform.position;
        Vector3 force = Vector3.zero;
        float posX = Mathf.Abs(position.x - (bounds.center.x - bounds.size.x / 2));
        float negX = Mathf.Abs(position.x - (bounds.center.x + bounds.size.x / 2));
        float posY = Mathf.Abs(position.y - (bounds.center.y - bounds.size.y / 2));
        float negY = Mathf.Abs(position.y - (bounds.center.y + bounds.size.y / 2));
        float posZ = Mathf.Abs(position.z - (bounds.center.z - bounds.size.z / 2));
        float negZ = Mathf.Abs(position.z - (bounds.center.z + bounds.size.z / 2));
        if (posX < _avoidance)
        {
            force.x = _avoidance - posX;
        }
        else if (negX < _avoidance)
        {
            force.x = negX - _avoidance;
        }

        if (posY < _avoidance)
        {
            force.y = _avoidance - posY;
        }
        else if (negY < _avoidance)
        {
            force.y = negY - _avoidance;
        }

        if (posZ < _avoidance)
        {
            force.z = _avoidance - posZ;
        }
        else if (negZ < _avoidance)
        {
            force.z = negZ - _avoidance;
        }

        return force.normalized * _velocityComponent.MaxMagnitude;
    }

    public void SetParams(float alignment, float cohesion, float seperation)
    {
        _alignment = alignment;
        _cohesion = cohesion;
        _seperation = seperation;
    }

    public Vector3 AvoidanceForce()
    {
        for(int i = 0; i < avoidance.Length; ++i)
        {
            var posA = avoidance[i].transform.position;
            var posB = transform.position;
            var direction = posB - posA;
            float distSquare = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
            float affectRadiusSq = avoidance[i].transform.localScale.z;
            affectRadiusSq *= affectRadiusSq;
            if (affectRadiusSq > distSquare)
            {
                transform.position = posA + direction.normalized * avoidance[i].transform.localScale.z;
                return direction.normalized;// * (affectRadiusSq / Mathf.Max(Mathf.Epsilon, distSquare));
            }
        }

        return Vector3.zero;
    }
}
