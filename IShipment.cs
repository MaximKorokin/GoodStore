using System;

namespace GoodStore
{
    public interface IShipment
    {
        int Id { get; set; }
        bool Import { get; set; }
        DateTime Date { get; set; }
    }
}
