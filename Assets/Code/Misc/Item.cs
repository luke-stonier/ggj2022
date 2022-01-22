using UnityEngine;

class Item : IItem
{
    private int _id = -1;
    private string _name = "NONE";
    private Texture2D _sprite;
    private int _count = 1;
    private bool _stackable = false;

    private Item() { }

    public Item(int __id, string __name, bool __stackable) {
        _id = __id;
        _name = __name;
        _stackable = __stackable;
        _sprite = Resources.Load<Texture2D>($"item_{_id}_sprite");
    }

    public int id()
    {
        return _id;
    }

    public string name()
    {
        return _name;
    }

    public Texture2D sprite()
    {
        return _sprite;
    }

    public int count()
    {
        return _count;
    }

    public bool stackable()
    {
        if (_count < 10)
            return _stackable;
        return false;
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
            _stackable = _stackable
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
            _stackable = _stackable
        };
    }
}