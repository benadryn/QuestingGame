using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    // public string savePath;
    public ItemDataBaseObject database;
    public Inventory container;

    
    public void AddItem(Item _item, int _amount)
    {
        if (_item.buffs is { Length: > 0 } && _item.type != ItemType.Consumable)
        {
            container.items.Add(new InventorySlot(_item.Id, _item, _amount));
            return;
        }

        foreach (var inventorySlot in container.items)
        {
            if (inventorySlot.item.Id != _item.Id) continue;
            inventorySlot.AddAmount(_amount);
            return;
        }

        container.items.Add(new InventorySlot(_item.Id, _item , _amount));
    }

    public int RemoveItem(Item _item)
    {
        for (int i = 0; i < container.items.Count; i++)
        {
            var inventorySlot = container.items[i];
            if (inventorySlot.item == _item)
            {
                if (inventorySlot.amount > 1)
                {
                    inventorySlot.RemoveAmount(1);
                    return inventorySlot.amount;
                }
                
                container.items.RemoveAt(i);
                return 0;
            }
        }

        return -1;
    }
    

    // [ContextMenu("Save")]
    // public void Save()
    // {
    //     IFormatter formatter = new BinaryFormatter();
    //     Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create,
    //         FileAccess.Write);
    //     formatter.Serialize(stream, container);
    //     stream.Close();
    // }
    //
    // [ContextMenu("Load")]
    // public void Load()
    // {
    //     if (!File.Exists(string.Concat(Application.persistentDataPath, savePath))) return;
    //     IFormatter formatter = new BinaryFormatter();
    //     Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open,
    //         FileAccess.Read);
    //     container = (Inventory)formatter.Deserialize(stream);
    //     stream.Close();
    // }

    [ContextMenu("Clear")]
    public void Clear()
    {
        container = new Inventory();
    }

    [System.Serializable]
    public class Inventory
    {
        public List<InventorySlot> items = new List<InventorySlot>();
    }

    [System.Serializable]
    public class InventorySlot
    {
        public int id;
        public Item item;
        public int amount;

        public InventorySlot(int _id, Item _item,int _amount)
        {
            id = _id;
            item = _item;
            amount = _amount;
        }
        public void AddAmount(int value)
        {
            amount += value;
        }

        public int RemoveAmount(int value)
        {
            amount -= value;
            return amount;
        }
    }

}