
using UnityEngine;

public class Barack : Building
{
    [SerializeField] private Transform _spawnPoint;
    
    public void CreateUnit(Unit unitPrefab)
    {
        Unit newUnit = Instantiate(unitPrefab, _spawnPoint.position, Quaternion.identity);
        
        Vector2 offset = Random.insideUnitCircle * 1.5f;
        Vector3 walkPosition =
            new Vector3(_spawnPoint.position.x + offset.x, 0, _spawnPoint.position.z + offset.y);
        
        newUnit.WhenClickOnGround(walkPosition);
    }
}