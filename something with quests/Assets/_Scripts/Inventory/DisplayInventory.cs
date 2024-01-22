using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public static DisplayInventory Instance;
    
    public InventoryObject inventory;

    [SerializeField]private GameObject inventoryPrefab;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private TextMeshProUGUI inventoryUsedSlotAmountText;

    private EquipManager _equipManager;

    private GameObject _activeTooltip;
    private GameObject[] _objectsWithTag;
    
    private int _fullSlot = 0;
    private int _totalSlots = 0;
    
    private bool _isBagClosed = true;
    private bool _isDragging = false;
    
    private Dictionary<InventoryObject.InventorySlot, GameObject> _itemsDisplayed = new Dictionary<InventoryObject.InventorySlot, GameObject>();


    private void OnEnable()
    {
        Draggable.OnDraggingStateChanged += HandleDraggingStateChange;
    }
    
    private void OnDisable()
    {
        Draggable.OnDraggingStateChanged -= HandleDraggingStateChange;
    }

    private void HandleDraggingStateChange(bool isDragging)
    {
        _isDragging = isDragging;
        SetRaycastTargets(!_isDragging);
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        _equipManager = EquipManager.Instance;
        SortBagSpots();
        CreateDisplay();

    }

    private void SortBagSpots()
    {
        _objectsWithTag = GameObject.FindGameObjectsWithTag("BagSlot");

        Array.Sort(_objectsWithTag, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

 
        foreach (var slot in _objectsWithTag)
        {
            _totalSlots++;
            if (slot.transform.childCount > 0)
            {
                _fullSlot++;
            }
        }
        inventoryUsedSlotAmountText.text = $"{_fullSlot}/{_totalSlots}";

    }


    public void UpdateDisplay()
    {
        foreach (var slot in inventory.container.items)
        {
            if (_itemsDisplayed.ContainsKey(slot))
            {
                _itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                AddEvent(_itemsDisplayed[slot], slot);
            }
            else
            {
                    CreateInventoryItemInBag(slot);
                    DisableInventoryWithItems(_isBagClosed);
            }
        }
        inventoryUsedSlotAmountText.text = $"{_fullSlot}/{_totalSlots}";

    }

    private void SetRaycastTargets(bool value)
    {
        foreach (var itemDisplayed in _itemsDisplayed.Values)
        {
            var image = itemDisplayed.GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = value;
            }
        }
    }

    private Transform FindEmptyInventorySlot()
    {
        foreach (var bagSlot in _objectsWithTag)
        {
            if (bagSlot.transform.childCount <= 0)
            {
                return bagSlot.transform;
            }
        }

        return null;
    }
    
    private void CreateDisplay()
    {
        foreach (var slot in inventory.container.items)
        {
            CreateInventoryItemInBag(slot);
        }

    }

    private void CreateInventoryItemInBag(InventoryObject.InventorySlot slot)
    {
            _fullSlot++;
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, FindEmptyInventorySlot());
            obj.transform.GetComponent<Image>().sprite =
                inventory.database.getItem[slot.item.Id].uiDisplay;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

            AddEvent(obj, slot);
            _itemsDisplayed.Add(slot, obj);
    }

    private void AddEvent(GameObject obj, InventoryObject.InventorySlot slot)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }
        
        // Mouse Enter event
        EventTrigger.Entry entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((data) => { OnPointerEnterTooltip(slot);});
        trigger.triggers.Add(entryEnter);
        
        // Mouse Exit event
        EventTrigger.Entry entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((data) => {OnPointerExitTooltip();});
        trigger.triggers.Add(entryExit);
        
        // Click event
        EventTrigger.Entry entryClick = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerClick
        };
        if (trigger.triggers.Exists(entry => entry.eventID == EventTriggerType.PointerClick)) return;
        {
            entryClick.callback.AddListener((data) => { OnPointerClickItem(slot);});
            trigger.triggers.Add(entryClick);
        }
        
    }
    

    private void OnPointerClickItem(InventoryObject.InventorySlot slot)
    {
        if (!_isDragging)
        {
            
            if (slot.item.type == ItemType.Consumable)
            {
                PotionUse(slot);
            }

            if (slot.item.type == ItemType.Weapon)
            {
                _equipManager.SwapWeapons(slot.item.weaponObject);
                CheckRemainingInventory(slot);
            
            }
        }
    }

    private void PotionUse(InventoryObject.InventorySlot slot)
    {
        PlayerHealth.OnPotionUsed?.Invoke(slot.item.buffs[0].value);

        CheckRemainingInventory(slot);
    }

    private void CheckRemainingInventory(InventoryObject.InventorySlot slot)
    {
        int remainingAmount = inventory.RemoveItem(slot.item);
        if (remainingAmount <= 0 && _itemsDisplayed.ContainsKey(slot))
        {
            Destroy(_itemsDisplayed[slot]);
            _itemsDisplayed.Remove(slot);
            tooltipPrefab.SetActive(false);
            _fullSlot--;
        }
        UpdateDisplay();
    }


    private void OnPointerEnterTooltip(InventoryObject.InventorySlot slot)
    {
        if (tooltipPrefab && !tooltipPrefab.activeSelf && !_isDragging)
        {
            tooltipPrefab.SetActive(true);

        
            TextMeshProUGUI tooltipText = tooltipPrefab.GetComponentInChildren<TextMeshProUGUI>();
            tooltipText.text = $"{slot.item.name.ToUpper()} \n\nStats: \n{GetStatsText(slot)} \n{slot.item.description}";

            RectTransform tooltipRect = tooltipPrefab.GetComponent<RectTransform>();
            Vector3 mousePos = Input.mousePosition;
            tooltipRect.position = new Vector3(mousePos.x - 100f, mousePos.y + 200f, 0f);
        }

        
    }

    private string GetStatsText(InventoryObject.InventorySlot slot)
    {
        string statsText = "";
        foreach (var itemBuff in slot.item.buffs)
        {
            statsText += $"{itemBuff.attribute}  {itemBuff.value} \n";
        }

        return statsText;
    }

    private void OnPointerExitTooltip()
    {
        tooltipPrefab.SetActive(false);
    }

    public void CheckBagStatus(bool isBagClosed)
    {
        _isBagClosed = isBagClosed;
        if (_isBagClosed)
        {
            OnPointerExitTooltip();
        }
        DisableInventoryWithItems(isBagClosed);
    }

    private void DisableInventoryWithItems(bool isBagClosed)
    {
        foreach (var slot in _objectsWithTag)
        {
            if (slot.transform.childCount >= 1)
            {
                slot.gameObject.SetActive(!isBagClosed);
            }
        }
    }
}