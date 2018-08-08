namespace GoodStore
{
    public class Good : IGood
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
    }
}
