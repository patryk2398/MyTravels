using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace MyTravels
{
    public class Places
    {
        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection("Data Source=databaze.db;");
            if (!File.Exists(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "databaze.db")))
            {
                try
                {
                    string myTableString = "CREATE TABLE Places(Country VARCHAR(100), Locality VARCHAR(100), Type VARCHAR(100), Rating INT, Description VARCHAR(100), Image BLOB);";
                    SQLiteCommand sqCommand2 = new SQLiteCommand(myTableString, sqlite_conn);
                    sqlite_conn.Open();
                    sqCommand2.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie udało się nawiązać połączenia z bazą");
                }
            }
            else
            {
                try
                {
                    sqlite_conn.Open();
                }
                catch (Exception)
                {
                    MessageBox.Show("Nie udało się nawiązać połączenia z bazą");
                }
            }
            return sqlite_conn;
        }

        public static void addPlace(string Country, string Locality, string Type, int Rating, string Description, SQLiteConnection conn, string location)
        {
            try
            {
                byte[] image = null;
                FileStream Stream = new FileStream(location, FileMode.Open, FileAccess.Read);
                BinaryReader brs = new BinaryReader(Stream);
                image = brs.ReadBytes((int)Stream.Length);

                string mySelectQuery = "INSERT INTO Places(Country, Locality, Type, Rating, Description, Image) VALUES('" + Country + "','" + Locality + "','" + Type + "'," + Rating + ",'" + Description + "', @image);";
                SQLiteCommand sqCommand = new SQLiteCommand(mySelectQuery, conn);
                sqCommand.Parameters.Add(new SQLiteParameter("@image", image));
                sqCommand.ExecuteNonQuery();
                MessageBox.Show("Miejsce dodane");
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się dodać Miejsca");
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
        }

        public static void editPlace(string Country, string Locality, string Type, int Rating, string Description, string id, SQLiteConnection conn, string location)
        {
            try
            {
                if (location == null)
                {
                    string mySelectQuery = "UPDATE Places SET Country='" + Country + "',Locality='" + Locality + "',Type='" + Type + "',Rating=" + Rating + ",Description='" + Description + "' WHERE ROWID=" + id;
                    SQLiteCommand sqCommand = new SQLiteCommand(mySelectQuery, conn);
                    sqCommand.ExecuteNonQuery();
                    MessageBox.Show("Miejsce zaktualizowane");
                }
                else
                {
                    byte[] image = null;
                    FileStream Stream = new FileStream(location, FileMode.Open, FileAccess.Read);
                    BinaryReader brs = new BinaryReader(Stream);
                    image = brs.ReadBytes((int)Stream.Length);

                    string mySelectQuery = "UPDATE Places SET Country='" + Country + "',Locality='" + Locality + "',Type='" + Type + "',Rating=" + Rating + ",Description='" + Description + "',Image=@image WHERE ROWID=" + id;
                    SQLiteCommand sqCommand = new SQLiteCommand(mySelectQuery, conn);
                    sqCommand.Parameters.Add(new SQLiteParameter("@image", image));
                    sqCommand.ExecuteNonQuery();
                    MessageBox.Show("Miejsce zaktualizowane");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się zaktualizować miejsca");
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
        }

        public static void deletePlace(SQLiteConnection conn, string id)
        {
            try
            {
                string mySelectQuery = "DELETE FROM Places WHERE ROWID=" + id;
                SQLiteCommand sqCommand = new SQLiteCommand(mySelectQuery, conn);
                sqCommand.ExecuteNonQuery();
                MessageBox.Show("Miejsce usunięte");
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się usunąć Miejsca");
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
        }

        public static Place showPlace(SQLiteConnection conn, string id)
        {
            try
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT ROWID, Country, Locality, Type, Rating, Description, Image FROM Places WHERE ROWID=" + id;
                SQLiteDataReader Reader = sqlite_cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    Place m = null;
                    while (Reader.Read())
                    {
                        m = new Place(Reader.GetInt32(0), Reader["Country"].ToString(), Reader["Locality"].ToString(), Reader["Type"].ToString(), Reader.GetInt32(4), Reader["Description"].ToString(), (byte[])Reader["Image"]);
                    }
                    Reader.Close();
                    return m;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się wyświetlić miejsca");
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        public static List<Place> searchPlace(string all)
        {
            List<Place> list = new List<Place>();
            try
            {
                SQLiteConnection conn = CreateConnection();
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT ROWID, Country, Locality, Type, Rating, Description, Image FROM Places WHERE " +
                    all;
                SQLiteDataReader Reader = sqlite_cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        list.Add(new Place(Reader.GetInt32(0), Reader["Country"].ToString(), Reader["Locality"].ToString(), Reader["Type"].ToString(), Reader.GetInt32(4), Reader["Description"].ToString(), (byte[])Reader["Image"]));
                    }
                    Reader.Close();
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wyszukiwanie zakończone niepowodzeniem");
            }
            finally
            {
            }
            return list;
        }

        public static List<Place> searchLocality(string Country)
        {
            List<Place> list = new List<Place>();
            try
            {
                SQLiteConnection conn = CreateConnection();
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT ROWID, Country, Locality, Type, Rating, Description, Image FROM Places WHERE Country='" +
                    Country + "';";
                SQLiteDataReader Reader = sqlite_cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        list.Add(new Place(Reader.GetInt32(0), Reader["Country"].ToString(), Reader["Locality"].ToString(), Reader["Type"].ToString(), Reader.GetInt32(4), Reader["Description"].ToString(), (byte[])Reader["Image"]));
                    }
                    Reader.Close();
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wyszukiwanie zakończone niepowodzeniem");
            }
            finally
            {
            }
            return list;
        }

        public static List<Place> PlaceList
        {
            get
            {
                List<Place> list = new List<Place>();
                try
                {
                    SQLiteConnection conn = CreateConnection();
                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT ROWID, Country, Locality, Type, Rating, Description, Image FROM Places";
                    SQLiteDataReader Reader = sqlite_cmd.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            list.Add(new Place(Reader.GetInt32(0), Reader["Country"].ToString(), Reader["Locality"].ToString(), Reader["Type"].ToString(), Reader.GetInt32(4), Reader["Description"].ToString(), (byte[])Reader["Image"]));
                        }
                        Reader.Close();
                    }
                    conn.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Pobranie miejsc z bazy nieudane. Baza prawdopodobnie jest pusta");
                }
                finally
                {
                }
                return list;
            }
        }
    }
    public class Place
    {
        public int Rowid { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }
        public string Type { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public Place(int Rowid, string Country, string Locality, string Type, int Rating, string Description, byte[] Image)
        {
            this.Rowid = Rowid;
            this.Country = Country;
            this.Locality = Locality;
            this.Type = Type;
            this.Rating = Rating;
            this.Description = Description;
            this.Image = Image;
        }
    }
}
