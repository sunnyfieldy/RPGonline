using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventorySlotUI : MonoBehaviour
{
    public Inventory inventory;
    public Equipment equipment;
    
    public Image icon;
    public TextMeshProUGUI amountText;
    
    private InventorySlot currentSlot;

    public ItemData[] chestRewards;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Set(InventorySlot slot)
    {
        currentSlot = slot;
        
        if (slot == null)
        {
            icon.enabled = false;
            amountText.text = "";
            return;
        }
        
        icon.sprite = slot.item.icon;
        icon.enabled = true;
        
        amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
    }

    public void Clear()
    {
        icon.enabled = false;
        amountText.text = "";
    }

    public void OnClick()
    {
        if(currentSlot == null) return;
        
        if (currentSlot.item.isWeapon)
        {
            equipment.EquipWeapon(currentSlot.item);
        }

        if (currentSlot.item.isChest)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        Debug.Log("Chest Opened!");

        ItemData reward =
            chestRewards[Random.Range(0, chestRewards.Length)];

        inventory.AddItem(reward);

        currentSlot.amount--;

        if (currentSlot.amount <= 0)
        {
            inventory.slots.Remove(currentSlot);
        }

        inventory.onInventoryChanged?.Invoke();
    }
}
