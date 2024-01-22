using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToggleInventory : MonoBehaviour
{
    private PlayerControls _playerControls;
    private InputAction _toggleInventory;
    private DisplayInventory _displayInventory;

    [SerializeField] private Image bagIcon;
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

    public void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            var swapEnabled = inventoryUI.enabled;
            inventoryUI.enabled = !swapEnabled;
            _displayInventory.CheckBagStatus(swapEnabled);
            ChangeBagAlpha(swapEnabled);
        }
    }

    private void ChangeBagAlpha(bool swapEnabled)
    {
        var currentColor = bagIcon.color;
        
        if (!swapEnabled)
        {
            currentColor.a = 0.5f;
            bagIcon.color = currentColor;
        }
        else
        {
            currentColor.a = 1.0f;
            bagIcon.color = currentColor;
        }
    }
}