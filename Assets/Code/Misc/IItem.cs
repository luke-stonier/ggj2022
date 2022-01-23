using UnityEngine;

public interface IItem : IEntity
{
    public int Id { get; }
    public string Name { get; }
    public int Count { get; }
    public Texture2D Sprite { get; }
    public bool Stackable { get; }
    public void increaseCount(int count);
    public void decreaseCount(int count);
    public IItem droppedItemWithCount(int count);
    public IItem newItem();
}