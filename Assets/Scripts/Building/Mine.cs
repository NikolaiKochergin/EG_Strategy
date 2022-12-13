using UnityEngine;

public class Mine : Building
{
    [SerializeField] [Min(0)] private int _money = 10;
    [SerializeField] [Min(0)] private float _perSecond = 1;
    
    private Resources _resources;
    private float _timer = 0;

    public override void Start()
    {
        base.Start();
        _resources = FindObjectOfType<Resources>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _perSecond)
        {
            _timer = 0;
            _resources.AddMoney(_money);
        }
    }
}