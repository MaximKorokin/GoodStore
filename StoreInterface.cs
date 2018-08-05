using System;
using System.Collections.Generic;

namespace GoodStore
{
    class StoreInterface
    {
        private readonly GoodStoreRepository repository;

        public StoreInterface(GoodStoreRepository rep)
        {
            repository = rep;
        }

        public void Start(GoodStoreRepository rep)
        {
            Console.WriteLine("\tGood Store");
            Menu();
            //foreach (var e in rep.GetGoods())
            //    Console.WriteLine(e.Name);
            //IShipment s = new Shipment
            //{
            //    Import = false,
            //    Date = DateTime.Now
            //};
            //System.Collections.Generic.List<IShipmentPart> sps = new System.Collections.Generic.List<IShipmentPart>
            //    { new ShipmentPart { GoodId = 6, Amount = 5 }, new ShipmentPart { GoodId = 5, Amount = 5 } };
            //rep.AddShipment(s, sps);
        }

        private void Menu()
        {
            ShowMenu();
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        AddGood();
                        ShowMenu();
                        break;
                    case "2":
                        AddShipment();
                        ShowMenu();
                        break;
                    case "3":
                        ShowGoods();
                        ShowMenu();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Enter valid number!");
                        break;
                }
            }
        }
        private void ShowMenu()
        {
            Console.WriteLine(@"
Menu
1. Add new good
2. Add shipment
3. Show goods in store
4. Exit
");
        }

        private void AddGood()
        {
            Console.WriteLine("Adding new good.\nEnter it's name:");
            string name;
            while ((name = Console.ReadLine()) == string.Empty)
            {
                Console.WriteLine("Enter valid name!");
            }

            Console.WriteLine("Enter it's unit:");
            string unit;
            while ((unit = Console.ReadLine()) == string.Empty)
            {
                Console.WriteLine("Enter valid unit!");
            }

            Console.WriteLine("Enter it's price:");
            int price;
            while (!int.TryParse(Console.ReadLine(), out price))
            {
                Console.WriteLine("Enter valid price!");
            }

            IGood good = new Good
            {
                Name = name,
                Unit = unit,
                Price = price
            };
            if (repository.AddGood(good))
                Console.WriteLine("New good added successfully");
            else
                Console.WriteLine("An error occured while adding new good");
        }

        private void AddShipment()
        {
            Console.WriteLine("Adding shipment.\n1. Import\n2. Export");
            bool import;
            while (true)
            {
                string importStr = Console.ReadLine();
                if (importStr == "1")
                {
                    import = true;
                    break;
                }
                if (importStr == "2")
                {
                    import = false;
                    break;
                }
                Console.WriteLine("Enter valid number!");
            }
            IShipment shipment = new Shipment
            {
                Import = import,
                Date = DateTime.Now
            };

            List<IShipmentPart> parts = new List<IShipmentPart>();
            var goodsList = repository.GetGoods();
            ShowShipmentPartMenu();
            string input;
            while((input = Console.ReadLine()) != "2")
            {
                if(input == "1")
                {
                    parts.Add(ShipmentPart(shipment, goodsList));
                    ShowShipmentPartMenu();
                }
                else
                    Console.WriteLine("Enter valid number!");
            }

            if (repository.AddShipment(shipment, parts))
                Console.WriteLine("New shipment added successfully");
            else
                Console.WriteLine("An error occured while adding new shipment");
            
        }
        private void ShowShipmentPartMenu()
        {
            Console.WriteLine("1. Add new good to the shipment\n2. Complete shipment");
        }
        private IShipmentPart ShipmentPart(IShipment shipment, List<IGood> goodsList)
        {
            Console.WriteLine("Goods list:\n");
            for (int i = 0; i < goodsList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {goodsList[i].Name}");
            }
            Console.WriteLine("\nChoose a good to add to the shipment:");

            int goodIndex;
            while(!int.TryParse(Console.ReadLine(), out goodIndex) || (--goodIndex < 0 || goodIndex >= goodsList.Count))
            {
                Console.WriteLine("Enter valid index!");
            }

            Console.WriteLine("Enter a number of the chosen good:");
            int amount;
            while(!int.TryParse(Console.ReadLine(), out amount) || amount < 1)
            {
                Console.WriteLine("Amount must be a positive number!");
            }

            return new ShipmentPart
            {
                GoodId = goodsList[goodIndex].Id,
                ShipmentId = shipment.Id,
                Amount = amount
            };
        }

        private void ShowGoods()
        {
            Console.WriteLine("Showing goods in store.\nName - Amount\n");
            foreach (IGood good in repository.GetGoods())
            {
                if (good.Amount > 0)
                    Console.WriteLine($"{good.Name} - {good.Amount}");
            }
        }
    }
}
