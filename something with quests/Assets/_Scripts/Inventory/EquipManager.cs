using UnityEngine;
using UnityEngine.Serialization;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;
    
    [SerializeField] private GameObject playerHand;
    [SerializeField] private WeaponObject weaponObject;
    private Item _oldWeapon;
    private GameObject _currentWeapon;

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
        
        _currentWeapon = Instantiate(weaponObject.weaponPrefab, playerHand.transform.position, Quaternion.identity);
        
        _currentWeapon.transform.SetParent(playerHand.transform);
    }


    public Item SwapWeapons(WeaponObject newWeapon)
    {
        _oldWeapon = weaponObject.CreateItem();
        weaponObject = newWeapon;
        Destroy(_currentWeapon);
        
        _currentWeapon = Instantiate(weaponObject.weaponPrefab , playerHand.transform.position, Quaternion.identity);
        
        _currentWeapon.transform.SetParent(playerHand.transform);
        _currentWeapon.transform.localRotation = Quaternion.identity;

        return _oldWeapon;
    }
    
}
