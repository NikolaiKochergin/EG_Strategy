using System;
using TMPro;
using UnityEngine;

public class Resources : MonoBehaviour
{
    [SerializeField] private TMP_Text _monewViewText;
    [SerializeField] private int _money;

    public int Money => _money;

    private void Start()
    {
        _monewViewText.text = _money.ToString();
    }

    public void SpendMoney(int value)
    {
        _money -= value;
        _monewViewText.text = _money.ToString();
    }

    public void AddMoney(int value)
    {
        _money += value;
        _monewViewText.text = _money.ToString();
    }
}