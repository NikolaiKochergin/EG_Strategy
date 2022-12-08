using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Unit _unitPrefab;
    
    private Resources _resources;

    private void Start()
    {
        _resources = FindObjectOfType<Resources>();
    }

    public void TryBuy()
    {
        int price = _unitPrefab.Price;

        if (_resources.Money >= price)
        {
            _resources.SpendMoney(price);

            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Vector3 spawnPosition =
                new Vector3(_spawnPoint.position.x + offset.x, 0, _spawnPoint.position.z + offset.y);
            
            Instantiate(_unitPrefab, spawnPosition, Quaternion.identity);
        }
    }
}