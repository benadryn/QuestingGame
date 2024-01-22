using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
    [HideInInspector]public Transform parentAfterDrag;
    [HideInInspector] public Transform parentBeforeDrag;

    public static event Action<bool> OnDraggingStateChanged;

    private void OnEnable()
    {
        OnDraggingStateChanged?.Invoke(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var parent = transform.parent;
        // this will stay the same to swap old item with new item
        parentBeforeDrag = parent;
        // this will change within inventory spot
        parentAfterDrag = parent;
        
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        
        OnDraggingStateChanged?.Invoke(true); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        
        OnDraggingStateChanged?.Invoke(false); 
    }


}
