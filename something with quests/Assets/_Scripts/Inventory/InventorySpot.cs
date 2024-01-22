using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySpot : MonoBehaviour, IDropHandler
{
    private GameObject _draggedItem;
    public void OnDrop(PointerEventData eventData)
    {
        _draggedItem = eventData.pointerDrag;
        if (_draggedItem != null)
        {
            Draggable draggableComponent = _draggedItem.GetComponent<Draggable>();
            if (transform.childCount == 0)
            {
                // GameObject dropped = eventData.pointerDrag;
                draggableComponent.parentAfterDrag = transform;
            }
            else
            {
                draggableComponent.parentAfterDrag = transform;
                
                // If the slot is not empty, swap parent references
                Transform otherItemTransform = transform.GetChild(0);
                
                // Change parent references
                otherItemTransform.SetParent(draggableComponent.parentBeforeDrag);
                _draggedItem.transform.SetParent(draggableComponent.parentAfterDrag);
            }
        }
    }
    
}
