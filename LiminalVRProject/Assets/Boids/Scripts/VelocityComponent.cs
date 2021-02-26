using UnityEngine;

public class VelocityComponent : MonoBehaviour
{
    //visible variables
    [SerializeField]
    private bool _limitMagnitude = false;

    [SerializeField]
    private float _minMagnitude = 0;

    [SerializeField]
    private float _maxMagnitude = 10;

    //definitions
    public delegate void OnVelocityChanged();

    //private variables
    private Vector3 _velocity;
    private Vector3 _acceleration;

    //properties
    public Vector3 Velocity
    {
        get => _velocity; 
        set 
        {
            if (_velocity != value)
            {
                _velocity = value;
                ValidateVelocity(_velocity);
            }
        }
    }
    public Vector3 Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }

    public float Magnitude 
    {
        get => _velocity.magnitude;
        set 
        {
            if (_limitMagnitude)
            {
                value = Mathf.Clamp(value, _minMagnitude, _maxMagnitude);
            }
            if (_velocity.magnitude != value)
            {
                _velocity.Normalize();
                _velocity *= value;
            }
        }
    }
    public float MinMagnitude
    {
        get => _minMagnitude;
        set
        {
            _limitMagnitude = true;
            _minMagnitude = Mathf.Max(0, value);
            if(_velocity.magnitude < _minMagnitude)
            {
                _velocity.Normalize();
                _velocity *= _minMagnitude;
            }
        }
    }
    public float MaxMagnitude
    {
        get => _maxMagnitude;
        set
        {
            _limitMagnitude = true;
            _maxMagnitude = Mathf.Max(0, value);
            if (_velocity.magnitude > _maxMagnitude)
            {
                _velocity.Normalize();
                _velocity *= _maxMagnitude;
            }
        }
    }

    //events
    private void Update()
    {
        ApplyAcceleration(_acceleration, Time.deltaTime);
        ApplyVelocity(_velocity, Time.deltaTime);
    }

    //manipulators
    private bool ValidateVelocity(Vector3 velocity)
    {
        if (_limitMagnitude)
        {
            float magnitude = Mathf.Clamp(velocity.magnitude, _minMagnitude, _maxMagnitude);
            if (magnitude != velocity.magnitude)
            {
                Magnitude = magnitude;
                return false;
            }
        }

        return true;
    }
    private void ApplyVelocity(Vector3 velocity, float time)
    {
        transform.position += velocity * time;
    }

    private void ApplyAcceleration(Vector3 acceleration, float time)
    {
        Velocity += acceleration * time;
    }

}
