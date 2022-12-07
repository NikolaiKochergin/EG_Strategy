using System.Collections.Generic;
using UnityEngine;

public class Management : MonoBehaviour
{
    private Camera _camera;
    private SelectableObject _hovered;
    private List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SelectableObject hitSelectable = hit.collider.GetComponent<SelectableCollider>()?.SelectableObject;

            if (hitSelectable)
            {
                if (_hovered)
                {
                    if (_hovered != hitSelectable)
                    {
                        _hovered.OnUnhover();
                        _hovered = hitSelectable;
                        _hovered.OnHover();
                    }
                }
                else
                {
                    _hovered = hitSelectable;
                    _hovered.OnHover();
                }
            }
            else
            {
                UnhoverCurrent();
            }
        }
        else
        {
            UnhoverCurrent();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_hovered)
            {
                if(Input.GetKey(KeyCode.LeftControl) == false)
                    UnselectAll();
                Select(_hovered);
            }
        }
        
        if(Input.GetMouseButtonDown(1))
            UnselectAll();
    }

    private void Select(SelectableObject selectableObject)
    {
        if (ListOfSelected.Contains(selectableObject) == false)
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    private void UnselectAll()
    {
        foreach (var selected in ListOfSelected)
            selected.Unselect();

        ListOfSelected.Clear();
    }

    private void UnhoverCurrent()
    {
        if (!_hovered) return;
        _hovered.OnUnhover();
        _hovered = null;
    }
}