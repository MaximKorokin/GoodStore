using System;

namespace GoodStore
{
    public class Shipment : IShipment
    {
        public int Id { get; set; }
        public bool Import { get; set; }
        public DateTime Date { get; set; }
    }
}
