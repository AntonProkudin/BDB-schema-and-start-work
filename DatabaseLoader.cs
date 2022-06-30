using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using BerkeleyDB;

namespace BDB
{
    public class DatabaseLoader
    {
        private MyDbs myDbs = null;

        public DatabaseLoader(MyDbs dbs)
        {
            myDbs = dbs;
        }

        public void LoadOrderDB(string dataDir)
        {
            DatabaseEntry key, data;
            string order_text_file = dataDir + "\\" + "order.txt";

            if (!File.Exists(order_text_file))
            {
                Console.WriteLine("{0} does not exist.", order_text_file);
                return;
            }

            using (StreamReader sr = File.OpenText(order_text_file))
            {
                Order order = new Order();
                string input;
                while ((input = sr.ReadLine()) != null)
                {
                    char[] delimiterPound = { '#' };
                    string[] fields = input.Split(delimiterPound);
                    order.Itemname = fields[0];
                    order.Sku = fields[1];
                    order.Price = float.Parse(fields[2]);
                    order.Quantity = int.Parse(fields[3]);
                    order.Category = fields[4];
                    order.Client = fields[5];
                    key = new DatabaseEntry();
                    key.Data = System.Text.Encoding.ASCII.GetBytes(
                        order.Sku);

                    byte[] bytes = order.getBytes();
                    data = new DatabaseEntry(bytes);

                    try
                    {
                        myDbs.OrderDB.Put(key, data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("LoadrderDB Error.");
                        Console.WriteLine(e.Message);
                        throw e;
                    }
                }
            }
        }

        public void LoadClientDB(string dataDir)
        {
            DatabaseEntry key;
            DatabaseEntry data;
            string client_text_file = dataDir + "\\" + "clients.txt";

            if (!File.Exists(client_text_file))
            {
                Console.WriteLine("{0} does not exist.", client_text_file);
                return;
            }

            using (StreamReader sr = File.OpenText(client_text_file))
            {
                Ñlient client = new Ñlient();
                string input;
                while ((input = sr.ReadLine()) != null)
                {
                    char[] delimiterPound = { '#' };
                    string[] fields = input.Split(delimiterPound);
                    client.Name = fields[0];
                    client.City = fields[1];
                    client.Street = fields[2];
                    client.House = fields[3];
                    client.Flat = fields[4];
                    client.Entrance = fields[5];
                    client.Storey = fields[6];
                    client.Elevator = fields[7];
                    key = new DatabaseEntry();
                    key.Data = System.Text.Encoding.ASCII.GetBytes(client.Name);

                    byte[] bytes = client.GetBytes();
                    data = new DatabaseEntry(bytes);

                    try
                    {
                        this.myDbs.ClientDB.Put(key, data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("LoadClientDB Error.");
                        Console.WriteLine(e.Message);
                        throw e;
                    }
                }
            }
        }
    }
}
