public interface IItem
{
    public int id();
    public int count();

    public void increaseCount(int count);
    public void decreaseCount(int count);
    public IItem droppedItemWithCount(int count);
}