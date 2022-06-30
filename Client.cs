using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BerkeleyDB;

namespace BDB
{
    [Serializable]
    public class Сlient
    {
        public string Name;/* Фамилия */
        public string City;    /* Имя */
        public string Street;  /* Улица */
        public string House;   /* Дом */
        public string Flat;    /* Квартира */
        public string Entrance;/* Подъезд*/
        public string Storey;  /* Этаж*/
        public string Elevator;/*Наличие лифта*/

        public byte[] GetBytes () {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memStream, this);
            byte [] bytes = memStream.GetBuffer();
            memStream.Close();
            return bytes;
        }
    }
}
