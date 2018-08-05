namespace GoodStore
{
    public interface IGood
    {
        int Id { get; set; }
        string Name { get; set; }
        string Unit { get; set; }
        int Price { get; set; }
        int Amount { get; set; }
    }
}
