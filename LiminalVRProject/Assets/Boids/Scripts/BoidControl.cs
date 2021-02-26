using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidControl : MonoBehaviour
{
    [SerializeField]
    int spawnAmount;

    [SerializeField, Range(0.0f, 1.0f)]
    float _alignment = 1;

    [SerializeField, Range(0.0f, 1.0f)]
    float _cohesion = 1;

    [SerializeField, Range(0.0f, 1.0f)]
    float _seperation = 1;

    SpawnVolume _spawner;
    float changeControl;
    List<BoidNeighbor> _boids = new List<BoidNeighbor>();

    [SerializeField]
    GameObject[] _prefabs;

    [SerializeField]
    int _initialSchoolSize = 10;

    [SerializeField]
    float _initialSchoolRadius = 3.0f;

    [SerializeField]
    int _maxFishTypes = -1;

    [SerializeField]
    Vector3 _fishRotation;

    [SerializeField]
    float _fishScale = 1;

    [SerializeField]
    float _minSpeed = 1;

    [SerializeField]
    float _maxSpeed = 5;

    [SerializeField]
    float _rotationSpeed = 90;

    [SerializeField]
    private AvoidanceSphere[] _avoidance;

    Mesh _mesh;

    void Awake()
    {
        _spawner = GetComponent<SpawnVolume>();
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        changeControl = 1;
        for (int i = 0; i < spawnAmount/ _initialSchoolSize; ++i)
        {
            var list = _spawner.Spawn(_avoidance, _initialSchoolSize, _initialSchoolRadius);
            foreach(GameObject boid in list)
            {
                boid.GetComponent<LookAtVelocity>().SetSpeed(_rotationSpeed);
                boid.GetComponent<VelocityComponent>().MinMagnitude = _minSpeed;
                boid.GetComponent<VelocityComponent>().MaxMagnitude = _maxSpeed;
                boid.GetComponent<WrapSpace>().SetBoundFromTransform(transform);
                var neighborComponent = boid.GetComponent<BoidNeighbor>();
                neighborComponent.SetParams(_alignment, _cohesion, _seperation);
                _boids.Add(neighborComponent);
                if (_prefabs.Length > 0)
                {
                    int fishType = (_maxFishTypes > 0) ? i % Mathf.Min(_prefabs.Length, _maxFishTypes) : i % _prefabs.Length;
                    neighborComponent.fishType = fishType;
                    neighborComponent.avoidance = _avoidance;
                    var fishObj = Instantiate(_prefabs[fishType], boid.transform) as GameObject;
                    fishObj.transform.localPosition = Vector3.zero;
                    fishObj.transform.localRotation = Quaternion.Euler(_fishRotation);
                    fishObj.transform.localScale = _fishScale * Vector3.one;
                }
            }
        }
        SetBoidParams();
    }

    void Update()
    {
        if (changeControl != _alignment + _cohesion + _seperation + _minSpeed + _maxSpeed + _rotationSpeed)
        {
            changeControl = _alignment + _cohesion + _seperation + _minSpeed + _maxSpeed + _rotationSpeed;
            SetBoidParams();
        }
    }

    void SetBoidParams()
    {
        for (int i = 0; i < _boids.Count; ++i)
        {
            _boids[i].SetParams(_alignment, _cohesion, _seperation);
            _boids[i].GetComponent<LookAtVelocity>().SetSpeed(_rotationSpeed);
            _boids[i].GetComponent<VelocityComponent>().MinMagnitude = _minSpeed;
            _boids[i].GetComponent<VelocityComponent>().MaxMagnitude = _maxSpeed;
        }
    }
}
