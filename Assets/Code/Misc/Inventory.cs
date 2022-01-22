using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : IInventory
{
    private int maxItems = 6;
    private int selectedSlot = 0;
    public List<IItem> items = new List<IItem>();

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
                return SwapItem(item, existingItemInInventory);

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

    public void SaveInventory(List<IItem> items)
    {
        throw new System.NotImplementedException();
    }

    public List<IItem> LoadInventory()
    {
        throw new System.NotImplementedException();
    }

    public IItem GetExistingItem(int itemId)
    {
        var exitingItem = items.SingleOrDefault((IItem i) => i.id() == itemId);
        return exitingItem;
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
}