public interface IInventory
{
    public void SetSelectedItem(int index);
    public IItem SelectedItem();
    public IItem AddItem(IItem item);
    public IItem DropItem(int count = 1);
    public IItem DropItem(int index, int count = 1);
    public IItem DropAllOfItem();
    public IItem DropAllOfItem(int index);
    public IItem GetItemFromIndex(int index);
    public IItem GetExistingItem(int itemId);
    public IItem SwapItem(IItem item);
    public IItem SwapItem(IItem oldItem, IItem newItem);
    public void SaveInventory(int inventoryId);
    public void LoadInventory(int inventoryId);
    public int NextAvailableSlot();
    public int GetIndexOfItem(IItem item);

    public InventoryRenderItem[] Items();
}
