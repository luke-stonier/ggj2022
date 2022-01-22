using System.Collections.Generic;

public interface IInventory
{
    public void SetSelectedItem(int index);
    public IItem SelectedItem();
    public IItem AddItem(IItem item);
    public IItem DropItem(int index, int count);
    public IItem DropAllOfItem(int index);
    public IItem GetItemFromIndex(int index);
    public IItem GetExistingItem(int itemId);
    public IItem SwapItem(IItem item);
    public IItem SwapItem(IItem oldItem, IItem newItem);
    public void SaveInventory(List<IItem> items);
    public List<IItem> LoadInventory();
    public int NextAvailableSlot();
    public int GetIndexOfItem(IItem item);
}
