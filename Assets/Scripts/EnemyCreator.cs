using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField][Min(0)] private float _creationPeriod = 5;
    [SerializeField] private GameObject _enemyPrefab;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _creationPeriod)
        {
            _timer = 0;
            Instantiate(_enemyPrefab, _spawnPoint.position, _spawnPoint.rotation);
        }
    }
}
