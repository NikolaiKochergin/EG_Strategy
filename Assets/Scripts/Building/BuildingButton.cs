using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private BuildingPlacer _buildingPlacer;
    [SerializeField] private Building _buildingPrefab;

    private Resources _resources;

    private void Start()
    {
        _resources = FindObjectOfType<Resources>();
    }

    public void TryBuy()
    {
        int price = _buildingPrefab.Price;
        
        if (_resources.Money >= price)
        {
            _resources.SpendMoney(price);
            _buildingPlacer.CreateBuilding(_buildingPrefab);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}