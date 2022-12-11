using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] [Min(0)] private int _health = 10;
    [SerializeField] [Min(0)] private float _distanceToFollow = 7f;
    [SerializeField] [Min(0)] private float _distanceToAttack = 1f;
    [SerializeField] [Min(0)] private float _attackPeriod = 1f;
    [SerializeField] [Min(0)] private int _damage = 1;

    private EnemyState _currentEnemyState;
    private Building _targetBuilding;
    private Unit _targetUnit;
    private float _timer;

    private void Start()
    {
        SetState(EnemyState.WalkToBuilding);
    }

    private void Update()
    {
        float distance;
        switch (_currentEnemyState)
        {
            case EnemyState.Idle:
                FindClosestUnit();
                break;
            case EnemyState.WalkToBuilding:
                FindClosestUnit();
                if (_targetBuilding == null)
                {
                    SetState(EnemyState.Idle);
                }
                break;
            case EnemyState.WalkToUnit:
                if (_targetUnit)
                {
                    _navMeshAgent.SetDestination(_targetUnit.transform.position);
                    distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                    if (distance > _distanceToFollow)
                        SetState(EnemyState.WalkToBuilding);
                    if (distance < _distanceToAttack)
                        SetState(EnemyState.Attack);
                }
                else
                {
                    SetState(EnemyState.WalkToBuilding);
                }
                break;
            case EnemyState.Attack:
                if (_targetUnit)
                {
                    distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                    if (distance > _distanceToAttack)
                        SetState(EnemyState.WalkToUnit);
                    _timer += Time.deltaTime;
                    if (_timer > _attackPeriod)
                    {
                        _timer = 0;
                        _targetUnit.TakeDamage(_damage);
                    }
                }
                else
                {
                    SetState(EnemyState.WalkToBuilding);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetState(EnemyState enemyState)
    {
        _currentEnemyState = enemyState;
        switch (_currentEnemyState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.WalkToBuilding:
                FindClosestBuilding();
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
                break;
            case EnemyState.WalkToUnit:
                break;
            case EnemyState.Attack:
                _timer = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FindClosestBuilding()
    {
        var allBuildings = FindObjectsOfType<Building>();
        var minDistance = Mathf.Infinity;
        Building closestBuilding = null;

        foreach (var building in allBuildings)
        {
            var distance = Vector3.Distance(transform.position, building.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = building;
            }
        }

        _targetBuilding = closestBuilding;
    }

    private void FindClosestUnit()
    {
        var allUnits = FindObjectsOfType<Unit>();
        var minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        foreach (var unit in allUnits)
        {
            var distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestUnit = unit;
            }
        }

        if (minDistance < _distanceToFollow)
        {
            _targetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToFollow);
    }
#endif
}