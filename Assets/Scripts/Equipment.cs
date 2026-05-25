using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData equipedWeapon;

    public System.Action onEquipmentChanged;

    public void EquipWeapon(ItemData item)
    {
        if (!item.isWeapon) return;
        
        equipedWeapon = item;
        onEquipmentChanged?.Invoke();
    }

    public int GetDamage()
    {
        return equipedWeapon != null ? equipedWeapon.damage : 5;
    }

    public void ClearWeapon()
    {
        equipedWeapon = null;

        onEquipmentChanged?.Invoke();
    }
}
