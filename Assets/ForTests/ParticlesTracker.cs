using UnityEngine;

public class ParticlesTracker : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _trackedSphere;

    private ParticleSystem.ShapeModule _sh;

    private void Start()
    {
        _sh = _particleSystem.shape;
    }

    private void Update()
    {
        _sh.position = _trackedSphere.transform.localPosition;
    }
}