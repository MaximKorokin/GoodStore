namespace GoodStore
{
    public class ShipmentPart : IShipmentPart
    {
        public int GoodId { get; set; }
        public int ShipmentId { get; set; }
        public int Amount { get; set; }
    }
}
