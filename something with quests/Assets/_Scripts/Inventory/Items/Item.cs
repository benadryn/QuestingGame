using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum ItemType
{
    Consumable,
    Weapon,
    Armor
}

public enum Attributes
{
    Stamina,
    Strength,
    Heal
}

public abstract class ItemObject : ScriptableObject
{
    public int id;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(10, 15)] public string description;
    public ItemBuff[] buffs;
    public WeaponObject weaponObject;

    public virtual Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;
}

[System.Serializable]
public class Item
{
    public string name;
    public int Id;
    [TextArea(10, 15)] public string description;
    public ItemType type;
    public ItemBuff[] buffs;
    public WeaponObject weaponObject;
    

    public Item()
    {
        
    }
    
    public Item(ItemObject item)
    {
        name = item.name;
        Id = item.id;
        description = item.description;
        type = item.type;
        buffs = new ItemBuff[item.buffs.Length];
        weaponObject = item.weaponObject;

        if (item.buffs.Length <= 0) return;
       
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff()
            {
                attribute = item.buffs[i].attribute,
                value = item.buffs[i].value
            };
        }
    }

}
    // [Serializable]
    // public class WeaponItem : Item
    // {
    //     [SerializeField] private GameObject weaponItemPrefab;
    //     public GameObject WeaponItemPrefab
    //     {
    //         get => weaponItemPrefab;
    //         set => weaponItemPrefab = value;
    //     }
    //
    //     public WeaponItem(WeaponObject weapon) : base(weapon)
    //     {
    //         weaponItemPrefab = weapon.weaponPrefab;
    //     }
    // }
