using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject : ItemObject
{
    public GameObject weaponPrefab;
    private void Awake()
    {
       
        type = ItemType.Weapon;
    }

    // public override Item CreateItem()
    // {
    //     WeaponObject weaponObject = new WeaponObject(this);
    //     // WeaponItem newWeapon = new WeaponItem(this);
    //     // newWeapon.WeaponItemPrefab = weaponPrefab;
    //     // return newWeapon;
    //     return weaponObject;
    // }
}
