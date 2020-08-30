using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAllocator
{
    class InventoryAllocator
    {
        /*Main program - CODE EXECUTION STARTS HERE*/
        public static void Main(string[] args)
        {
            TestCases();    //Invoke Function to test the cases
        }

        /*Initialization for the Test cases*/
        public static void TestCases()
        {
            //Test case 1 - Happy Case, exact inventory match!
            Dictionary<string, int> order = new Dictionary<string, int> { { "apple", 1 } };
            List<Warehouse> warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 1 } }));
            Test(order, warehouses);

            //Test case 2 - Not enough inventory -> no allocations!
            order = new Dictionary<string, int> { { "apple", 1 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 0 } }));
            Test(order, warehouses);

            //Test case 3 - Should split an item across warehouses if that is the only way to completely ship an item
            order = new Dictionary<string, int> { { "apple", 10 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 5 } }));
            warehouses.Add(new Warehouse("dm", new Dictionary<string, int> { { "apple", 5 } }));
            Test(order, warehouses);

            //Test case 4 - Not enough order -> no allocations!
            order = new Dictionary<string, int> { { "apple", 0 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 5 } }));
            Test(order, warehouses);

            //Test case 5 - Single item, multiple warehouses
            order = new Dictionary<string, int> { { "apple", 16 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 3 } }));
            warehouses.Add(new Warehouse("ft", new Dictionary<string, int> { { "apple", 7 } }));
            warehouses.Add(new Warehouse("dm", new Dictionary<string, int> { { "apple", 8 } }));
            Test(order, warehouses);

            //Test case 6 - Multiple items Single warehouse, partial allocations
            order = new Dictionary<string, int> { { "apple", 1 }, { "banana", 3 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 2 } }));
            Test(order, warehouses);

            //Test case 7 - Multiple items Single warehouse, fulfillment
            order = new Dictionary<string, int> { { "apple", 1 }, { "banana", 3 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("dm", new Dictionary<string, int> { { "apple", 6 }, { "banana", 8 } }));
            Test(order, warehouses);

            //Test case 8 - Multiple items Single warehouse, fulfillment
            order = new Dictionary<string, int> { { "apple", 6 }, { "banana", 4 }, { "orange", 8 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("mgt", new Dictionary<string, int> { { "apple", 3 }, { "banana", 1 }, { "orange", 1 } }));
            warehouses.Add(new Warehouse("nyt", new Dictionary<string, int> { { "apple", 1 }, { "banana", 2 }, { "orange", 3 } }));
            warehouses.Add(new Warehouse("dm", new Dictionary<string, int> { { "apple", 5 }, { "banana", 1 }, { "orange", 7 } }));
            Test(order, warehouses);

            //Test case 9 - Multiple items Multiple warehouses, present in all, not enough
            order = new Dictionary<string, int> { { "apple", 6 }, { "banana", 4 }, { "orange", 8 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("mgt", new Dictionary<string, int> { { "apple", 3 }, { "banana", 1 }, { "orange", 1 } }));
            warehouses.Add(new Warehouse("nyt", new Dictionary<string, int> { { "apple", 1 }, { "banana", 2 }, { "orange", 3 } }));
            warehouses.Add(new Warehouse("dm", new Dictionary<string, int> { { "apple", 5 }, { "banana", 1 }, { "orange", 2 } }));
            Test(order, warehouses);

            //Test case 10 - Multiple items Multiple warehouses
            order = new Dictionary<string, int> { { "apple", 5 }, { "banana", 5 }, { "orange", 5 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 5 }, { "orange", 10 } }));
            warehouses.Add(new Warehouse("dm", new Dictionary<string, int> { { "banana", 5 }, { "orange", 10 } }));
            Test(order, warehouses);

            //Test case 11 - Item exists but no inventory
            order = new Dictionary<string, int> { { "banana", 4 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 5 }, { "banana", 0 } }));
            warehouses.Add(new Warehouse("mgt", new Dictionary<string, int> { { "apple", 10 }, { "banana", 0 } }));
            Test(order, warehouses);

            //Test case 12 - Item does not exist in inventory
            order = new Dictionary<string, int> { { "banana", 4 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 5 } }));
            warehouses.Add(new Warehouse("mgt", new Dictionary<string, int> { { "orange", 10 } }));
            Test(order, warehouses);

            //Test case 13 - Multiple items, one item does not exist in inventory
            order = new Dictionary<string, int> { { "apple", 5 }, { "banana", 4 }, { "orange", 2 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", 2 } }));
            warehouses.Add(new Warehouse("mgt", new Dictionary<string, int> { { "apple", 7 }, { "orange", 10 } }));
            Test(order, warehouses);

            //Test case 14 - Negative inventory -> no allocations
            order = new Dictionary<string, int> { { "apple", 1 } };
            warehouses = new List<Warehouse>();
            warehouses.Add(new Warehouse("owd", new Dictionary<string, int> { { "apple", -2 } }));
            Test(order, warehouses);
        }

        /*Invoke Inventory allocator function and display shipment*/
        public static void Test(Dictionary<string, int> order, List<Warehouse> warehouses)
        {
            List<Warehouse> shipments = Allocation(order, warehouses);

            /***Display purpose only - Shipmant output****/
            Console.WriteLine("SHIPMENT");
            foreach (var shipment in shipments)
            {
                Console.Write($"{{ {shipment._name}: {{ ");
                foreach (var inventory in shipment._inventory)
                {
                    Console.Write($"{inventory.Key}: {inventory.Value} ");
                }
                Console.Write($"}} }} ");
            }
            Console.WriteLine("\n");
        }

        //Warehouse object with warehouse name and a dictionary of inventory for each item
        public class Warehouse
        {
            public string _name;
            public Dictionary<string, int> _inventory;
            public Warehouse(string name, Dictionary<string, int> inventory)
            {
                _name = name;
                _inventory = inventory;
            }
        }

        //Inventory allocator function
        public static List<Warehouse> Allocation(Dictionary<string, int> order, List<Warehouse> warehouses)
        {
            List<Warehouse> shipments = new List<Warehouse>();

            /***Display purpose only - Input order and inventory before****/
            Console.WriteLine("ORDER");
            Console.Write($"{{ ");
            foreach (var item in order)
            {
                Console.Write($"{item.Key}: {item.Value} ");
            }
            Console.WriteLine($"}} ");
            Console.WriteLine("INVENTORY BEFORE");
            foreach (var warehouse in warehouses)
            {
                Console.Write($"{{ {warehouse._name}: {{ ");
                foreach (var inventory in warehouse._inventory)
                {
                    Console.Write($"{inventory.Key}: {inventory.Value} ");
                }
                Console.Write($"}} }} ");
            }
            Console.WriteLine();

            /*Inventory Allocation*/
            foreach (var item in order)
            {
                var item_value = item.Value;
                foreach (var warehouse in warehouses)
                {
                    if (warehouse._inventory.ContainsKey(item.Key) && item_value > 0 &&
                        warehouse._inventory.TryGetValue(item.Key, out int inv_value) && inv_value > 0)
                    {
                        var allocate_value = inv_value >= item_value ? item_value : inv_value;

                        var obj = shipments.FirstOrDefault(x => x._name == warehouse._name);
                        if (obj != null)
                        {
                            obj._inventory.Add(item.Key, allocate_value);
                        }
                        else
                        {
                            shipments.Add(new Warehouse(warehouse._name, new Dictionary<string, int> { { item.Key, allocate_value } }));
                        }

                        item_value -= allocate_value;
                        warehouse._inventory[item.Key] -= allocate_value;
                    }
                }
            }

            /***Display purpose only - Inventory after****/
            Console.WriteLine("INVENTORY AFTER");
            foreach (var warehouse in warehouses)
            {
                Console.Write($"{{ {warehouse._name}: {{ ");
                foreach (var inventory in warehouse._inventory)
                {
                    Console.Write($"{inventory.Key}: {inventory.Value} ");
                }
                Console.Write($"}} }} ");
            }
            Console.WriteLine();

            return shipments.OrderBy(o => o._name).ToList();
        }
    }
}
