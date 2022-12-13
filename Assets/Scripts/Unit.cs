using System;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] [Min(0)] private int _price = 5;
    [SerializeField] [Min(0)] private int _health = 10;
    [SerializeField] private HealthBar _healthBarPrefab;

    private HealthBar _healthBar;
    private int _maxHealth;

    protected NavMeshAgent NavMeshAgent => _navMeshAgent;
    public int Price => _price;

    public override void Start()
    {
        base.Start();
        _maxHealth = _health;
        _healthBar = Instantiate(_healthBarPrefab, transform);
        _healthBar.Setup(transform);
    }

    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);

        _navMeshAgent.SetDestination(point);
    }

    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        _healthBar.SetHealth(_health, _maxHealth);
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        FindObjectOfType<Management>()?.Unselect(this);
    }
}