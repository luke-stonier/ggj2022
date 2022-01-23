using UnityEngine;

class ItemSaveMap
{
    public int Id;
    public string Name;
    public Texture2D Sprite;
    public int Count;
    public bool Stackable;

    public IItem MapToIItem()
    {
        return new Item(Id, Name, Count, Stackable);
    }
}
