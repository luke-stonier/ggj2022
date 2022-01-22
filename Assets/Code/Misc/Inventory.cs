using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : IInventory
{
    private int maxItems = 6;
    private int selectedSlot = 0;
    public IItem[] items = new IItem[0];

    public Inventory(int inventoryId)
    {
        items = new IItem[maxItems];
        // LoadInventory(inventoryId);
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
        var existingItemInInventory = GetExistingItem(item.id());
        if (existingItemInInventory != null)
        {
            // If we have the item and its stackable, increase count
            if (existingItemInInventory.stackable())
                existingItemInInventory.increaseCount(item.count());
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

    public IItem DropItem(int index, int count = 1)
    {
        if (index <= -1 || index >= maxItems) return null;
        var itemToDrop = items[index];
        if (itemToDrop == null) return null;
        if (itemToDrop.count() == 1) RemoveItem(index);
        else itemToDrop.decreaseCount(count);
        return itemToDrop.droppedItemWithCount(count);
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

    public void SaveInventory(int inventoryId, IItem[] items)
    {
        throw new System.NotImplementedException();
    }

    public IItem[] LoadInventory(int inventoryId)
    {
        throw new System.NotImplementedException();
    }

    public IItem GetExistingItem(int itemId)
    {
        try
        {
            var exitingItem = items.SingleOrDefault((IItem i) => i != null && i.id() == itemId);
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
            if (items[i] != null && items[i].id() == item.id()) return i;

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