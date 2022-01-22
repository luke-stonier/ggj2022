class Item : IItem
{
    private int _id = -1;
    private string _name = "NONE";
    private int _count = 0;
    private bool _stackable = false;

    public int id()
    {
        return _id;
    }

    public string name()
    {
        return _name;
    }

    public int count()
    {
        return _count;
    }

    public bool stackable()
    {
        return _stackable;
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
            _count = count
        };
    }
}