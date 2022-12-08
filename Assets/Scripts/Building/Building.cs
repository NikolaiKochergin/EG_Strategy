using UnityEngine;

public class Building : SelectableObject
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] [Min(0)] private int _price;
    [SerializeField] [Min(0)] private int _xSize = 3;
    [SerializeField] [Min(0)] private int _zSize = 3;

    private Color _startColor;
    
    public int Price => _price;
    public int XSize => _xSize;
    public int ZSize => _zSize;

    private void Awake()
    {
        _startColor = _renderer.material.color;
    }

    public void DisplayUnacceptablePosition()
    {
        _renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition()
    {
        _renderer.material.color = _startColor;
    }

    private void OnDrawGizmos()
    {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;
        
        for (int x = 0; x < _xSize; x++)
            for (int z = 0; z < _zSize; z++)
                Gizmos.DrawWireCube(transform.position + new Vector3(x,0, z) * cellSize, new Vector3(1f, 0f, 1f) * cellSize);
    }
}