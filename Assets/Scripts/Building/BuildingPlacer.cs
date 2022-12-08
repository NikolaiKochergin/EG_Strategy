using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;

    private Building _currentBuilding;
    private Plane _plane= new Plane(Vector3.up, Vector3.zero);
    private Camera _raycastCamera;

    private Dictionary<Vector2Int, Building> BuildingsDictionary = new Dictionary<Vector2Int, Building>();

    private void Awake() =>
        _raycastCamera = Camera.main;

    private void Update()
    {
        if (_currentBuilding == null) return;
        
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
    
        if (_plane.Raycast(ray, out var distance))
        {
            var point = ray.GetPoint(distance) / CellSize;

            int x = Mathf.RoundToInt(point.x);
            int z = Mathf.RoundToInt(point.z);
    
            _currentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;

            if (CheckAllow(x, z, _currentBuilding))
            {
                _currentBuilding.DisplayAcceptablePosition();
                if (Input.GetMouseButtonDown(0))
                {
                    InstallBuilding(x, z, _currentBuilding);
                    _currentBuilding = null;
                } 
            }
            else
            {
                _currentBuilding.DisplayUnacceptablePosition();
            }
        }
    }

    private bool CheckAllow(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if(BuildingsDictionary.ContainsKey(coordinate))
                    return false;
            }
        }
        return true;
    }

    public void InstallBuilding(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                BuildingsDictionary.Add(coordinate, _currentBuilding);
            }
        }
    }

    public void CreateBuilding(Building buildingPrefab)
    {
        Building newBuilding = Instantiate(buildingPrefab);
        _currentBuilding = newBuilding;
    }
}