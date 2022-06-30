using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BerkeleyDB;

namespace BDB
{
    [Serializable]
    public class �lient
    {
        public string Name;/* ������� */
        public string City;    /* ��� */
        public string Street;  /* ����� */
        public string House;   /* ��� */
        public string Flat;    /* �������� */
        public string Entrance;/* �������*/
        public string Storey;  /* ����*/
        public string Elevator;/*������� �����*/

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
