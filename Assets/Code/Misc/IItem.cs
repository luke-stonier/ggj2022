using UnityEngine;

public interface IItem
{
    public int id();
    public string name();
    public Texture2D sprite();
    public int count();
    public bool stackable();
    public void increaseCount(int count);
    public void decreaseCount(int count);
    public IItem droppedItemWithCount(int count);
    public IItem newItem();
}