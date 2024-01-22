using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;
    
    [SerializeField] private GameObject playerHand;
    [SerializeField] private WeaponObject startingWeapon;
    [SerializeField] private Animator animator;
    
    private WeaponObject _currentWeaponObject;
    private GameObject _currentWeapon;
    private Item _currentWeaponItem;
    
    public InventoryObject inventory;
    
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int IsHeavyAttacking = Animator.StringToHash("isHeavyAttacking");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        
        EquipWeapon(startingWeapon);
    }


    public void EquipWeapon(WeaponObject newWeapon)
    {
        if (!animator.GetBool(IsAttacking) || !animator.GetBool(IsHeavyAttacking))
        {
            
        
            // Remove old Weapon from inv and get it as item
            if (_currentWeaponObject)
            {
                inventory.RemoveItem(_currentWeaponItem);
                Destroy(_currentWeapon);
            }
        
            // Set New Weapon
            _currentWeaponObject = newWeapon;
        
            // Instantiate the new weapon
            _currentWeapon = Instantiate(_currentWeaponObject.weaponPrefab, playerHand.transform.position, Quaternion.identity);
            _currentWeapon.transform.SetParent(playerHand.transform);
            _currentWeapon.transform.localRotation = Quaternion.identity;
        
            // Create an item instance for new weapon
            _currentWeaponItem = _currentWeaponObject.CreateItem();
        }
    }

    public void SwapWeapons(WeaponObject newWeapon)
    {
        // save current weapon as item
        Item oldWeapon = _currentWeaponItem;
        
        // equip the new weapon
        EquipWeapon(newWeapon);
        
        // if old weapon add to inventory
        if (oldWeapon != null)
        {
            inventory.AddItem(oldWeapon, 1);
        }
    }
    
}
