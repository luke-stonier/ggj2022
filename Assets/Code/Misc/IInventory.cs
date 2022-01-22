using System.Collections.Generic;

public interface IInventory
{
    public bool AddItem(IItem item);
    public IItem DropItem(int index, int count);
    public IItem DropAllOfItem(int index);
    public IItem GetItemFromIndex(int index);
    public IItem GetExistingItem(int itemId);
    public void SaveInventory(List<IItem> items);
    public List<IItem> LoadInventory();
    public int NextAvailableSlot();
}
