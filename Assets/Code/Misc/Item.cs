using Newtonsoft.Json;
using UnityEngine;

public class Item : IItem
{
    private int _id = -1;
    private string _name = "NONE";
    [JsonIgnore]
    private Texture2D _sprite;
    private int _count = 1;
    private bool _stackable = false;
    private string _entityResourceName;

    public int Id => _id;

    public string Name => _name;

    public int Count => _count;

    [JsonIgnore]
    public Texture2D Sprite => _sprite;

    public bool Stackable {
        get {
            if (_count < 10)
                return _stackable;
            return false;
        }
    }

    public string entityResourceName => _entityResourceName;

    private Item() { }

    public Item(int __id, string __name, int __count, bool __stackable) {
        _id = __id;
        _name = __name;
        _count = __count;
        _stackable = __stackable;

        _sprite = Resources.Load<Texture2D>($"Items/item_{_id}_sprite");
        _entityResourceName = $"Items/item_{_id}_entity";
    }

    public void increaseCount(int count)
    {
        _count += count;
    }

    public void decreaseCount(int count)
    {
        _count -= count;
    }

    public IItem droppedItemWithCount(int count)
    {
        return new Item
        {
            _id = _id,
            _name = _name,
            _sprite = _sprite,
            _count = count,
            _stackable = _stackable,
            _entityResourceName = _entityResourceName
        };
    }

    public IItem newItem()
    {
        return new Item
        {
            _id = _id,
            _name = _name,
            _sprite = _sprite,
            _count = _count,
            _stackable = _stackable,
            _entityResourceName = _entityResourceName
        };
    }
}