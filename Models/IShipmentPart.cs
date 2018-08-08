namespace GoodStore
{
    public interface IShipmentPart
    {
        int GoodId { get; set; }
        int ShipmentId { get; set; }
        int Amount { get; set; }
    }
}
