using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class Inventory : IInventory
{
    private int maxItems = 6;
    private int selectedSlot = 0;
    public IItem[] items = new IItem[0];

    public Inventory(int inventoryId)
    {
        items = new IItem[maxItems];
        LoadInventory(inventoryId);
    }

    public void SetSelectedItem(int index)
    {
        selectedSlot = Mathf.Clamp(index, 0, maxItems);
    }

    public IItem SelectedItem()
    {
        SetSelectedItem(selectedSlot);
        return items[selectedSlot];
    }

    /// <summary>
    /// Add item to inventory, if inventory is full replaces selected item with new item and drops selected
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public IItem AddItem(IItem item)
    {
        var existingItemInInventory = GetExistingItem(item.Id);
        if (existingItemInInventory != null)
        {
            // If we have the item and its stackable, increase count
            if (existingItemInInventory.Stackable)
                existingItemInInventory.increaseCount(item.Count);
            else
                return SwapItem(existingItemInInventory, item);

            return null;
        }

        var index = NextAvailableSlot();
        if (index == -1) return SwapItem(item);
        else items[index] = item;

        return null;
    }

    /// <summary>
    /// Swaps currently selected item with new item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public IItem SwapItem(IItem item)
    {
        var droppedItem = DropAllOfItem(selectedSlot);
        items[selectedSlot] = item;
        return droppedItem;
    }

    /// <summary>
    /// Swaps old item with new item
    /// </summary>
    /// <param name="oldItem"></param>
    /// <param name="newItem"></param>
    /// <returns></returns>
    public IItem SwapItem(IItem oldItem, IItem newItem)
    {
        SetSelectedItem(GetIndexOfItem(oldItem));
        items[selectedSlot] = newItem;
        return oldItem;
    }

    public IItem DropItem(int count = 1)
    {
        return DropItem(selectedSlot, count);
    }

    public IItem DropItem(int index, int count = 1)
    {
        if (index <= -1 || index >= maxItems) return null;
        var itemToDrop = items[index];
        if (itemToDrop == null) return null;
        if (itemToDrop.Count == 1) RemoveItem(index);
        else itemToDrop.decreaseCount(count);
        return itemToDrop.droppedItemWithCount(count);
    }

    public IItem DropAllOfItem()
    {
        return DropAllOfItem(selectedSlot);
    }

    public IItem DropAllOfItem(int index)
    {
        if (index <= -1 || index >= maxItems) return null;
        var itemToDrop = items[index];
        if (itemToDrop == null) return null;
        RemoveItem(index);
        return itemToDrop;
    }

    private void RemoveItem(int index)
    {
        try
        {
            items[index] = null;
        } catch(ArgumentOutOfRangeException ex)
        {
            Console.WriteLine("Tried to remove item from out of bounds");
            Console.WriteLine(ex.Message);
        }
    }

    public IItem GetItemFromIndex(int index)
    {
        if (index <= -1 || index >= maxItems) return null;
        var item = items[index];
        return item;
    }

    public void SaveInventory(int inventoryId)
    {
        var jsonInv = JsonConvert.SerializeObject(items);
        PlayerPrefs.SetString($"inv_{inventoryId}", jsonInv);
    }

    public void LoadInventory(int inventoryId)
    {
        var jsonInv = PlayerPrefs.GetString($"inv_{inventoryId}");
        if (jsonInv == "{}"|| jsonInv == null || jsonInv == "") return;   // No inventory to load
        var mapItems = JsonConvert.DeserializeObject<ItemSaveMap[]>(jsonInv);
        for(var i = 0; i < mapItems.Length; i++)
        {
            items[i] = mapItems[i]?.MapToIItem();
        }
    }

    public IItem GetExistingItem(int itemId)
    {
        try
        {
            var exitingItem = items.SingleOrDefault((IItem i) => i != null && i.Id == itemId);
            return exitingItem;
        } catch(Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public int NextAvailableSlot()
    {
        var nextSlot = -1;
        for (var i = 0; i < maxItems; i++)
            if (items[i] == null && nextSlot == -1) return i;

        return nextSlot;
    }

    public int GetIndexOfItem(IItem item)
    {
        for (var i = 0; i < maxItems; i++)
            if (items[i] != null && items[i].Id == item.Id) return i;

        return -1;
    }

    public InventoryRenderItem[] Items()
    {
        var renderItems = new InventoryRenderItem[maxItems];
        var i = 0;
        foreach (IItem item in items)
        {
            var _ = new InventoryRenderItem
            {
                item = item,
                selected = (i == selectedSlot)
            };
            renderItems[i] = _;
            i++;
        }
        return renderItems;
    }
}