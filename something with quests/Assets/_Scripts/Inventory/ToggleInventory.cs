using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInventory : MonoBehaviour
{
    private PlayerControls _playerControls;
    private InputAction _toggleInventory;

    [SerializeField] private GameObject inventoryUI;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _toggleInventory = _playerControls.Player.ShowBag;

        _toggleInventory.performed += context => { ToggleInventoryUI(); };
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
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}