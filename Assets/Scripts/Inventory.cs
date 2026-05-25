using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;
}

public class Inventory : MonoBehaviour
{
   public List<InventorySlot> slots;
   public int maxSlot = 20;
   
   public System.Action onInventoryChanged;

   public void AddItem(ItemData item)
   {
       //stack if possible
       if (item.isStackable)
       {
           foreach (var slot in slots)
           {
               if (slot.item == item && slot.amount < item.maxStackSize)
               {
                   slot.amount++;
                   onInventoryChanged?.Invoke();
                   return;
               }
           }
       }
       
       //add new slot
       if (slots.Count < maxSlot)
       {
           slots.Add(new InventorySlot{ item = item, amount = 1});
           onInventoryChanged?.Invoke();
       }
   }
    public void ClearInventory()
    {
        slots.Clear();

        onInventoryChanged?.Invoke();
    }
}
