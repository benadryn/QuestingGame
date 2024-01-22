using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInventory : MonoBehaviour
{
    private PlayerControls _playerControls;
    private InputAction _toggleInventory;
    private DisplayInventory _displayInventory;

    [SerializeField] private Canvas inventoryUI;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _toggleInventory = _playerControls.Player.ShowBag;
        _toggleInventory.performed += context => { ToggleInventoryUI(); };
    }

    private void Start()
    {
        _displayInventory = DisplayInventory.Instance;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            var swapEnabled = inventoryUI.enabled;
            inventoryUI.enabled = !swapEnabled;
            _displayInventory.CheckBagStatus(swapEnabled);
        }
    }
}