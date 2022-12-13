using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState
{
    UnitSelected,
    Frame,
    Other
}

public class Management : MonoBehaviour
{
    private const string Ground = nameof(Ground);

    [SerializeField] private Image _frameImage;
    
    private Camera _camera;
    private SelectableObject _hovered;
    private List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    private SelectionState _currentSelectionState;

    private void Awake()
    {
        _camera = Camera.main;
        _frameImage.enabled = false;
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
                
                _currentSelectionState = SelectionState.UnitSelected;
                Select(_hovered);
            }
        }
        
        if(_currentSelectionState == SelectionState.UnitSelected)
            if (Input.GetMouseButtonUp(0))
                if (hit.collider.CompareTag(nameof(Ground)))
                    foreach (var selected in ListOfSelected)
                        selected.WhenClickOnGround(hit.point);
        
        if(Input.GetMouseButtonDown(1))
            UnselectAll();

        if (Input.GetMouseButtonDown(0))
            _frameStart = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);
            Vector2 size = max - min;

            if (size.magnitude > 10)
            {
                _frameImage.enabled = true;
                _frameImage.rectTransform.anchoredPosition = min;
                _frameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(min, size);

                UnselectAll();
                Unit[] allUnits = FindObjectsOfType<Unit>();
                foreach (var unit in allUnits)
                {
                    Vector2 screenPoint = _camera.WorldToScreenPoint(unit.transform.position);
                    if (rect.Contains(screenPoint))
                        Select(unit);
                }
                
                _currentSelectionState = SelectionState.Frame;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _frameImage.enabled = false;
            if (ListOfSelected.Count > 0)
                _currentSelectionState = SelectionState.UnitSelected;
            else
                _currentSelectionState = SelectionState.Other;
        }
    }

    private void Select(SelectableObject selectableObject)
    {
        if (ListOfSelected.Contains(selectableObject) == false)
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    public void Unselect(SelectableObject selectableObject)
    {
        if (ListOfSelected.Contains(selectableObject))
            ListOfSelected.Remove(selectableObject);
    }

    private void UnselectAll()
    {
        foreach (var selected in ListOfSelected)
            selected.Unselect();

        ListOfSelected.Clear();
        _currentSelectionState = SelectionState.Other;
    }

    private void UnhoverCurrent()
    {
        if (!_hovered) return;
        _hovered.OnUnhover();
        _hovered = null;
    }
}