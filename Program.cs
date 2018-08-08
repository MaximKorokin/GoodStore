namespace GoodStore
{
    class Program
    {
        private static void Main()
        {
            using(GoodStoreRepository rep = new GoodStoreRepository())
            {
                new StoreInterface(rep).Start();
            }
        }
    }
}
