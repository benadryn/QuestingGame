
using System;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private bool _isDragActive = false;

    private Vector2 _screenPosition;

    private Vector3 _worldPosition;

    private Draggable _lastDragged;
    private LayerMask _layerMask;

    private void Awake()
    {
        _layerMask = LayerMask.GetMask("UI");
    }

    void Update()
    {
        if (_isDragActive && Input.GetMouseButtonUp(0))
        {
            Drop();
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            _screenPosition = new Vector2(mousePos.x, mousePos.y);
        }else return;
        
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);
        if (_isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero, _layerMask);
            if (hit.collider != null)
            {
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    _lastDragged = draggable;
                    InitDrag();
                }
            }
        }
    }

    private void InitDrag()
    {
        _isDragActive = true;
    }


    private void Drag()
    {
        _lastDragged.transform.position = new Vector2(_worldPosition.x, _worldPosition.y);
        Debug.Log(_lastDragged.name);
    }
    private void Drop()
    {
        _isDragActive = false;
    }
}
