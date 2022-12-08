using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private Resources _resources;
    [SerializeField] private BuildingPlacer _buildingPlacer;
    [SerializeField] private Building _buildingPrefab;

    public void TryBuy()
    {
        int price = _buildingPrefab.GetComponent<Building>().Price;
        
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