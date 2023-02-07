using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows;

namespace DZO
{
    class dbConn
    {
        public string host = "ep-proud-darkness-984642.eu-central-1.aws.neon.tech";
        public string db = "neondb";
        public string user = "Cverlllly";
        public string pass = "JyQifcNA7kr8";
        NpgsqlConnection con = new NpgsqlConnection("Host=ep-proud-darkness-984642.eu-central-1.aws.neon.tech; Port=5432; User Id=Cverlllly;Password=JyQifcNA7kr8;Database=neondb");
        public List<string> Kraji()
        {
            List<object> list = new List<object>();
            List<string> list1 = new List<string>();
            con.Open();
            NpgsqlTransaction tran = con.BeginTransaction();
            string stm = "SELECT krajidata()";
            NpgsqlCommand cmdd = new NpgsqlCommand(stm, con);

            var dr = cmdd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(dr.GetValue(0));
                
            }
            con.Close();
            foreach (var item in list)
            {
                Object ime = ((object[])item)[0];
                Object zip = ((object[])item)[1];
                list1.Add(ime + " " + zip);
            }
                return list1;
        }
        public List<string> Lokacije(string ime, string postna_st)
        {
            List<string> list = new List<string>();
            List<object> list1 = new List<object>();
            con.Open();
            string stm = "SELECT get_location_info('"+ime+"','"+postna_st+"')";
            NpgsqlCommand cmdd = new NpgsqlCommand(stm, con);
            NpgsqlDataReader dr = cmdd.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(dr.GetValue(0));
            }
            con.Close();
            foreach (var item in list1)
            {
                Object name = ((object[])item)[0];
                Object zap = ((object[])item)[1];
                Object ost = ((object[])item)[2];
                Object spb = ((object[])item)[3];
                list.Add(name + " " + zap + " "+ ost+ " "+spb);
            }
            return list;
        }
        public List<string> lokacija(string ime)
        {
            List<string> list = new List<string>();
            List<object> list1 = new List<object>();
            con.Open();
            string stm2 = "SELECT find_location_names('" + ime + "')";
            NpgsqlCommand cmdd2 = new NpgsqlCommand(stm2, con);
            NpgsqlDataReader dr = cmdd2.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(dr.GetValue(0));
            }
            con.Close();
            foreach (object item in list1)
            {
                list.Add(item.ToString());
            }
            return list;
        }
        public void Insert_Lokacija(string kraj, string ime, int zap, int ost, int sob)
        {
            con.Open();
            string stm = "SELECT insert_location('"+kraj+"','"+ime+"',"+zap+","+ost+","+sob+")";
            NpgsqlCommand cmd = new NpgsqlCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public List<string> Lokacije_vse(string ime)
        {
            List<string> list = new List<string>();
            List<object> list1 = new List<object>();
            con.Open();

            string stm2 = "SELECT lokacije_vse('"+ime+"')";
            NpgsqlCommand cmdd2 = new NpgsqlCommand(stm2, con);
            NpgsqlDataReader dr2 = cmdd2.ExecuteReader();
            while (dr2.Read())
            {
                list1.Add(dr2.GetValue(0));
            }
            con.Close();
            foreach (var item in list1)
            {
                Object name = ((object[])item)[0];
                Object zap = ((object[])item)[1];
                Object ost = ((object[])item)[2];
                Object spb = ((object[])item)[3];
                list.Add(name + " " + zap + " " + ost + " " + spb);
            }
            return list;
        }
        public void UpdateLokacije(string kraj, string ime, int zap, int ost, int sobe)
        {
            con.Open();
            string stm = "SELECT updatelokacijee('" + ime + "','" + kraj + "'," + zap + "," + ost + "," + sobe + ")";
            NpgsqlCommand cmd = new NpgsqlCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public List<string> Get_zaposleni(string ime_kraja)
        {
            List<string> list = new List<string>();
            List<object> list1 = new List<object>();
            con.Open();

            string stm2 = "SELECT select_zaposleni('" + ime_kraja + "')";
            NpgsqlCommand cmdd2 = new NpgsqlCommand(stm2, con);
            NpgsqlDataReader dr2 = cmdd2.ExecuteReader();
            while (dr2.Read())
            {
                list1.Add(dr2.GetValue(0));
            }
            con.Close();
            foreach (var item in list1)
            {
                Object id = ((object[])item)[0];
                Object ime = ((object[])item)[1];
                Object priimek = ((object[])item)[2];
                Object naziv = ((object[])item)[3];
                Object telefon = ((object[])item)[4];
                list.Add(id + " " + ime+" "+priimek+" "+ naziv+ " "+ telefon);
            }
            return list;
        }
        public void UpdateZaposleni(string ime, string priimek, string naziv, string telefonska, int id)
        {
            con.Open();
            string stm = String.Format("SELECT update_zaposleni('"+ime+"','"+priimek+"','"+naziv+"','"+telefonska+"',"+id+")");
            NpgsqlCommand cmd = new NpgsqlCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void DeleteZaposleni(int id)
        {
            con.Open();
            string stm = "SELECT delete_zaposleni(" + id+")";
            NpgsqlCommand cmd = new NpgsqlCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public string GetCountZaposleni(string kraj)
        {
            con.Open();
            string stm = "SELECT count_zaposleni('"+kraj+"')";
            NpgsqlCommand cmdd = new NpgsqlCommand(stm, con);
            string neki = cmdd.ExecuteScalar().ToString();
            con.Close();
            return neki;


        }
        public string GetCountOstareli(string kraj)
        {
            con.Open();
            string stm = "SELECT count_ostareli_by_location('" + kraj+"')";
            NpgsqlCommand cmdd = new NpgsqlCommand(stm, con);
            string neki = cmdd.ExecuteScalar().ToString();
            con.Close();
            return neki;
        }
        public void InsertUser(string ime, string priimek, string naziv, string telefon, string kraj)
        {
            con.Open();
            string stm = "SELECT add_zaposleni('" + ime + "','" + priimek + "','" + naziv + "','" + telefon + "','" + kraj + "'" + ")";
            NpgsqlCommand cmd = new NpgsqlCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public string GetCountStSob(string kraj)
        {
            con.Open();
            string stm = "SELECT get_st_sob('" + kraj + "')";
            NpgsqlCommand cmdd = new NpgsqlCommand(stm, con);
            string neki = cmdd.ExecuteScalar().ToString();
            con.Close();
            return neki;
        }
        public List<string> Get_osaterli(string ime_kraja)
        {
            List<string> list = new List<string>();
            List<object> list1 = new List<object>();
            con.Open();
            string stm = "SELECT get_ostareli_by_location('" + ime_kraja + "')";
            NpgsqlCommand cmdd = new NpgsqlCommand(stm, con);
            NpgsqlDataReader dr = cmdd.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(dr.GetValue(0));
            }
            con.Close();
            foreach (var item in list1)
            {
                Object id = ((object[])item)[0];
                Object ime = ((object[])item)[1];
                Object priimek = ((object[])item)[2];
                Object st_sobe = ((object[])item)[3];
                Object roj = ((object[])item)[4];
                list.Add(id + " " + ime + " " + priimek + " " + st_sobe + " " + roj);
            }
            return list;
        }
        public void InsertOstareli(string ime, string priimek, string naziv, int telefon, string kraj)
        {
            con.Open();
            string stm = "SELECT insert_ostareli ('" + ime + "','" + priimek + "','" + naziv + "'," + telefon + ",'" + kraj + "')";
            NpgsqlCommand cmd = new NpgsqlCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void EditOstareli(string ime, string priimek, string st_sobe, int leto, int id)
        {
            con.Open();
            string stm = "SELECT edit_ostareli('" + ime + "','" + priimek + "','" + st_sobe + "'," + leto + "," + id + ")";
            NpgsqlCommand cmd = new NpgsqlCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void DeleteOstareli(int id)
        {
            con.Open();
            string stm = "SELECT delete_ostareli(" + id+")";
            NpgsqlCommand cmd = new NpgsqlCommand(stm, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public bool Login(string username, string password)
        {
            con.Open();
            string stm = "SELECT login('" + username + "' , '"+password+"')";
            NpgsqlCommand cmdd = new NpgsqlCommand(stm, con);
            string neki = cmdd.ExecuteScalar().ToString();
            con.Close();
            if (neki.Equals("f"))
            {
                return false;
            }
            else
            {
                con.Open();
                string stm2 = "SELECT update_last_login(" + username + ")";
                NpgsqlCommand cmd = new NpgsqlCommand(stm2, con);
                cmd.ExecuteNonQuery();
                con.Close();
                return true;

            }
        }
    }
}

