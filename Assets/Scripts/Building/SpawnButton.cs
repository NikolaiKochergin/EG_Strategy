using TMPro;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Barack _barack;
    
    private Resources _resources;

    private void Start()
    {
        _resources = FindObjectOfType<Resources>();
        _priceText.text = _unitPrefab.Price.ToString();
    }

    public void TryBuy()
    {
        int price = _unitPrefab.Price;

        if (_resources.Money >= price)
        {
            _resources.SpendMoney(price);

            _barack.CreateUnit(_unitPrefab);
        }
        else
        {
            Debug.Log("Недостаточно денег");
        }
    }
}