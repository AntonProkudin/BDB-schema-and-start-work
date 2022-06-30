
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using BerkeleyDB;

namespace BDB
{
    public class MyDbs
    {
        private string vDbName;
        private string iDbName;
        private string itemSDbName;

        private BTreeDatabase ibtreeDB, vbtreeDB;
        private BTreeDatabaseConfig btreeConfig;
        private SecondaryBTreeDatabase itemSecbtreeDB;
        private SecondaryBTreeDatabaseConfig itemSecbtreeConfig;

        public MyDbs(string databaseHome)
        {
            vDbName = "clientdb.db";
            iDbName = "orderdb.db";
            itemSDbName = "itemname.sdb";

            if (databaseHome != null)
            {
                vDbName = databaseHome + "\\" + vDbName;
                iDbName = databaseHome + "\\" + iDbName;
                itemSDbName = databaseHome + "\\" + itemSDbName;
            }

            btreeConfig = new BTreeDatabaseConfig();
            btreeConfig.Creation = CreatePolicy.IF_NEEDED;
            btreeConfig.CacheSize = new CacheInfo(0, 64 * 1024, 1);
            btreeConfig.ErrorPrefix = "get start";
            btreeConfig.PageSize = 8 * 1024;
            try
            {
                RemoveDbFile(vDbName);
                RemoveDbFile(iDbName);
                RemoveDbFile(itemSDbName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleting db.");
                Console.WriteLine(e.Message);
                throw e;
            }
            try
            {
                vbtreeDB = BTreeDatabase.Open(vDbName, btreeConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening {0}.", vDbName);
                Console.WriteLine(e.Message);
                throw e;
            }

            try
            {
                ibtreeDB = BTreeDatabase.Open(iDbName, btreeConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening {0}.", iDbName);
                Console.WriteLine(e.Message);
                throw e;
            }

            try
            {
                itemSecbtreeConfig = new SecondaryBTreeDatabaseConfig(
                    ibtreeDB, new SecondaryKeyGenDelegate(
                    CreateSecondaryKey));

                itemSecbtreeConfig.Creation = CreatePolicy.IF_NEEDED;
                itemSecbtreeConfig.Duplicates = DuplicatesPolicy.UNSORTED;
                itemSecbtreeDB = SecondaryBTreeDatabase.Open(
                    itemSDbName, itemSecbtreeConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening secondary {0}", itemSDbName);
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        ~MyDbs()
        {
            try
            {
                if (itemSecbtreeDB != null)
                {
                    itemSecbtreeDB.Close();
                }
                if (ibtreeDB != null)
                {
                    ibtreeDB.Close();
                }
                if (vbtreeDB != null)
                {
                    vbtreeDB.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error closing DB");
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public string VDbName
        {
            get { return vDbName; }
            set { vDbName = value; }
        }

        public string IDbName
        {
            get { return iDbName; }
            set { iDbName = value; }
        }
        public string ItemSDbName
        {
            get { return itemSDbName; }
            set { itemSDbName = value; }
        }

        public BTreeDatabase ClientDB
        {
            get { return vbtreeDB; }
        }

        public BTreeDatabase OrderDB
        {
            get { return ibtreeDB; }
        }

        public SecondaryBTreeDatabase ItemSecbtreeDB
        {
            get { return itemSecbtreeDB; }
        }
        public DatabaseEntry CreateSecondaryKey(
            DatabaseEntry key, DatabaseEntry data)
        {

            DatabaseEntry SecKeyDbt = new DatabaseEntry();
            Order order = new Order(data.Data);

            SecKeyDbt.Data = System.Text.Encoding.ASCII.GetBytes(
                order.Itemname);
            return SecKeyDbt;
        }

        private void RemoveDbFile(string dbName)
        {
            string buff;

            if (File.Exists(dbName))
            {
                while (true)
                {
                    Console.Write("{0} существует.  Удалить? (y/n)", dbName);
                    buff = Console.ReadLine().ToLower();
                    if (buff == "y" || buff == "n")
                        break;
                }
                if (buff == "y")
                {
                    try
                    {
                        File.Delete(dbName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        throw e;
                    }
                }
            }
        }
    }
}
