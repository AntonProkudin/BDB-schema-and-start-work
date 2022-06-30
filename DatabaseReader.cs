
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
    public class DatabaseReader
    {
        private MyDbs myDbs = null;

        public DatabaseReader(MyDbs dbs)
        {
            myDbs = dbs;
        }

        public void showItem(string locateItem)
        {
            SecondaryCursor secCursor;
            DatabaseEntry searchKey = new DatabaseEntry();
            searchKey.Data = System.Text.Encoding.ASCII.GetBytes(locateItem);
            secCursor = this.myDbs.ItemSecbtreeDB.SecondaryCursor();
            if (secCursor.Move(searchKey, true))
            {
                Order theOrder = new Order(
                    secCursor.Current.Value.Value.Data);
                displayOrder(theOrder);
                while (secCursor.MoveNextDuplicate())
                {
                    theOrder = new Order(
                        secCursor.Current.Value.Value.Data);
                    displayOrder(theOrder);
                }
            }
            else
            {
                Console.WriteLine("Could not find client name {0} ", locateItem);
                Console.WriteLine();
                Console.WriteLine();
                return;
            }
            secCursor.Close();
        }

        public void showAllOrder()
        {
            BTreeCursor cursor = myDbs.OrderDB.Cursor();
            Order theOrder;

            while (cursor.MoveNext())
            {
                theOrder = new Order(cursor.Current.Value.Data);
                displayOrder(theOrder);
            }
            cursor.Close();
        }

        public void displayOrder(Order theOrder)
        {
            Console.WriteLine("Название: {0} ", theOrder.Itemname);
            Console.WriteLine("Артикул: {0} ", theOrder.Sku);
            Console.WriteLine("Цена за штуку: {0} ", theOrder.Price);
            Console.WriteLine("Количество: {0} ", theOrder.Quantity);
            Console.WriteLine("Категория: {0} ", theOrder.Category);
            Console.WriteLine("Клиент: {0} ", theOrder.Client);
            displayClient(theOrder);
        }

        private void displayClient(Order theOrder)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            DatabaseEntry foundClient = new DatabaseEntry();
            MemoryStream memStream;
            Сlient theClient;

            foundClient.Data = System.Text.Encoding.ASCII.GetBytes(
                theOrder.Client);
            try
            {
                KeyValuePair<DatabaseEntry, DatabaseEntry> pair =
                    new KeyValuePair<DatabaseEntry, DatabaseEntry>();
                string clientData;

                pair = this.myDbs.ClientDB.Get(foundClient);
                clientData = System.Text.ASCIIEncoding.ASCII.GetString(
                    pair.Value.Data);

                memStream = new MemoryStream(pair.Value.Data.Length);
                memStream.Write(pair.Value.Data, 0, pair.Value.Data.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                theClient = (Сlient)formatter.Deserialize(memStream);

                memStream.Close();

                System.Console.WriteLine("Город: {0}", theClient.City);
                System.Console.Write("Улица: {0}", theClient.Street);
                System.Console.Write("{0} ", theClient.House);
                System.Console.WriteLine("{0} ", theClient.Flat);
                System.Console.WriteLine("Подъезд и этаж: {0} и {1}",
                    theClient.Entrance, theClient.Storey);
                System.Console.WriteLine("Наличие лифта: {0} ", theClient.Elevator);
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("displayClient Error.");
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}
