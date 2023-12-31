using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public static DisplayInventory Instance;
    
    [SerializeField]private GameObject inventoryPrefab;
    public InventoryObject inventory;

    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Image inventoryPanel;
    private GameObject _activeTooltip;

    private EquipManager _equipManager;
    
    private Dictionary<InventoryObject.InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventoryObject.InventorySlot, GameObject>();


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
        CreateDisplay();
        _equipManager = EquipManager.Instance;
    }
    
    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        foreach (var slot in inventory.container.items)
        {
            if (itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                AddEvent(itemsDisplayed[slot], slot);
            }
            else
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryPanel.transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                    inventory.database.getItem[slot.item.Id].uiDisplay;
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                itemsDisplayed.Add(slot, obj);
            }
        }
    }
    
    public void CreateDisplay()
    {
        foreach (var slot in inventory.container.items)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryPanel.transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                inventory.database.getItem[slot.item.Id].uiDisplay;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            
            AddEvent(obj, slot);
            itemsDisplayed.Add(slot, obj);
        }
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
        if (slot.item.type == ItemType.Consumable)
        {
            PotionUse(slot);
        }

        if (slot.item.type == ItemType.Weapon)
        {
            Item oldWeapon = _equipManager.SwapWeapons(slot.item.weaponObject);
            if (oldWeapon != null)
            {
                CheckRemainingInventory(slot);
                inventory.AddItem(oldWeapon, 1);
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

        if (remainingAmount <= 1 && itemsDisplayed.ContainsKey(slot))
        {
            Destroy(itemsDisplayed[slot]);
            itemsDisplayed.Remove(slot);
            tooltipPrefab.SetActive(false);
        }
    }


    private void OnPointerEnterTooltip(InventoryObject.InventorySlot slot)
    {
        if (tooltipPrefab && !tooltipPrefab.activeSelf)
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
    
   
}