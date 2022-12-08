using UnityEngine;

public class Resources : MonoBehaviour
{
    [SerializeField] private int _money;

    public int Money => _money;

    public void SpendMoney(int value)
    {
        _money -= value;
    }
}