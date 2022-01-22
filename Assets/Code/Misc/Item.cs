class Item : IItem
{
    private int _id = -1;
    private int _count = 0;

    public int count()
    {
        return _count;
    }

    public int id()
    {
        return _id;
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