using System.Collections.Generic;
using System.Linq;

public class Inventory : IInventory
{
    private int maxItems = 6;
    public List<IItem> items = new List<IItem>();

    public bool AddItem(IItem item)
    {
        var existingItemInInventory = GetExistingItem(item.id());
        if (existingItemInInventory != null)
        {
            existingItemInInventory.increaseCount(item.count());
            return true;
        }

        var index = NextAvailableSlot();
        if (index == -1) return false;
        items[index] = item;

        return true;
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
        items[index] = null;
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
}