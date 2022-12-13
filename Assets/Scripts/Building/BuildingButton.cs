using TMPro;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _buildingPriceText;
    [SerializeField] private BuildingPlacer _buildingPlacer;
    [SerializeField] private Building _buildingPrefab;

    private Resources _resources;

    private void Start()
    {
        _resources = FindObjectOfType<Resources>();
        _buildingPriceText.text = _buildingPrefab.Price.ToString();
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