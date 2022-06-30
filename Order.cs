using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BerkeleyDB;

namespace BDB
{
    [Serializable]
    public class Order
    {
        private string category;//Категория
        private string itemname;//Наименование
        private float  price;    //Цена
        private int    quantity;   //Количество
        private string sku;     //Артикул
        private string сlient;  //Клииент

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public string Itemname
        {
            get { return itemname; }
            set { itemname = value; }
        }
        public float Price
        {
            get { return price; }
            set { price = value; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public string Sku
        {
            get { return sku; }
            set { sku = value; }
        }

        public string Client
        {
            get { return сlient; }
            set { сlient = value; }
        }
        public Order()
        {
            itemname = System.String.Empty;
            category = System.String.Empty;
            price = 0.0F;
            quantity = 0;
            sku = System.String.Empty;
            сlient = System.String.Empty;
        }

        public Order(byte[] buffer)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream(buffer);
            Order tmp = (Order)formatter.Deserialize(memStream);

            this.itemname = tmp.itemname;
            this.sku = tmp.sku;
            this.price = tmp.price;
            this.quantity = tmp.quantity;
            this.category = tmp.category;
            this.сlient = tmp.сlient;
            memStream.Close();
        }
        public byte[] getBytes()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            formatter.Serialize(memStream, this);
            byte[] bytes = memStream.GetBuffer();
            memStream.Close();
            return bytes;
        }
    }
}
