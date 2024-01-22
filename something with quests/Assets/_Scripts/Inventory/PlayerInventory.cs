using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public bool resetInventory;

    private void Awake()
    {
        if (resetInventory)
        {
            inventory.Clear();
        }
        // inventory.Load();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     inventory.Save();
        // }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     var item = other.GetComponent<ItemToPickUp>();
    //     if (item)
    //     {
    //         inventory.AddItem(new Item(item.item), 1);
    //         Destroy(other.gameObject);
    //     }
    // }
}
