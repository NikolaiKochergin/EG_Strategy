using System;
using UnityEditor;
using UnityEngine;

public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}
public class Knight : Unit
{
    [SerializeField] [Min(0)] private float _attackPeriod = 1f;
    [SerializeField] [Min(0)] private int _damage = 1;
    [SerializeField] [Min(0)] private float _distanceToAttack = 1f;
    [SerializeField] [Min(0)] private float _distanceToFollow = 7f;
    
    private UnitState _currentUnitState;
    private Vector3 _targetPoint;
    private Enemy _targetEnemy;
    private float _timer;

    public override void Start()
    {
        base.Start();
        SetState(UnitState.WalkToPoint);
    }

    private void Update()
    {
        float distance;
        switch (_currentUnitState)
        {
            case UnitState.Idle:
                FindClosestEnemy();
                break;
            case UnitState.WalkToPoint:
                FindClosestEnemy();
                break;
            case UnitState.WalkToEnemy:
                if (_targetEnemy)
                {
                    NavMeshAgent.SetDestination(_targetEnemy.transform.position);
                    distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
                    if (distance > _distanceToFollow)
                        SetState(UnitState.WalkToPoint);
                    if (distance < _distanceToAttack)
                        SetState(UnitState.Attack);
                }
                else
                {
                    SetState(UnitState.WalkToPoint);
                }

                break;
            case UnitState.Attack:
                if (_targetEnemy)
                {
                    NavMeshAgent.SetDestination(_targetEnemy.transform.position);
                    distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
                    if (distance > _distanceToAttack)
                        SetState(UnitState.WalkToEnemy);
                    _timer += Time.deltaTime;
                    if (_timer > _attackPeriod)
                    {
                        _timer = 0;
                        _targetEnemy.TakeDamage(_damage);
                    }
                }
                else
                {
                    SetState(UnitState.WalkToPoint);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetState(UnitState unitState)
    {
        _currentUnitState = unitState;
        switch (_currentUnitState)
        {
            case UnitState.Idle:
                break;
            case UnitState.WalkToPoint:
                break;
            case UnitState.WalkToEnemy:
                break;
            case UnitState.Attack:
                _timer = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FindClosestEnemy()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        float minDistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (Enemy enemy in allEnemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (minDistance < _distanceToFollow)
        {
            _targetEnemy = closestEnemy;
            SetState(UnitState.WalkToEnemy);
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