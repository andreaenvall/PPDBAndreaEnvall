using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Figgle;

namespace PPDBAndreaEnvall
{

    class Parking
    {
        public void InsertSP(string regNr, int fordon)
        {
            try
            {
                string ConnectionString = "Data Source=LAPTOP-K2JKI9TE\\SQLEXPRESS;Initial Catalog=PPDBAndreaEnvall;Integrated Security=SSPI";

                string sql = $"Select f.RegNr, p.PlatsNummer From Parkering p Inner join Fordon f ON f.ParkeringsRuta = p.PlatsID WHERE RegNr = '{regNr}'";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //Create the command object
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "fordonstorlekprocedure",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@FordonTypID", fordon);


                    //Set SqlParameter
                    SqlParameter outParameter = new SqlParameter
                    {
                        ParameterName = "@FordonStorlek", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    //add the parameter to the SqlCommand object
                    cmd.Parameters.Add(outParameter);

                    connection.Open();
                    cmd.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand()
                    {
                        CommandText = "HittaPlats",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd2.Parameters.AddWithValue("@FordonStorlek", outParameter.Value);

                    SqlParameter outParameter2 = new SqlParameter
                    {
                        ParameterName = "@PlatsID", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    //add the parameter to the SqlCommand object
                    cmd2.Parameters.Add(outParameter2);


                    cmd2.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand()
                    {
                        CommandText = "Updateraproc",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd3.Parameters.AddWithValue("@PlatsID", outParameter2.Value);
                    cmd3.Parameters.AddWithValue("@FordonStorlek", outParameter.Value);

                    cmd3.ExecuteNonQuery();

                    SqlCommand cmd4 = new SqlCommand()
                    {
                        CommandText = "insertprocedure",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd4.Parameters.AddWithValue("@RegNr", regNr);
                    cmd4.Parameters.AddWithValue("@FordonsTypID", fordon);
                    cmd4.Parameters.AddWithValue("@Ankomst", System.DateTime.Now.ToString());
                    cmd4.Parameters.AddWithValue("@ParkeringsRuta", outParameter2.Value);

                    cmd4.ExecuteNonQuery();

                    SqlCommand cmd5 = new SqlCommand(sql, connection);

                    SqlDataReader reader = cmd5.ExecuteReader();

                        while (reader.Read())
                        {
                            Console.WriteLine("Fordon {0} är nu Incheckad på plats {1}!", regNr, reader[1]);
                          

                        }

                    
                    

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AJDÅ! Något gick fel, finns redan fordonet inlagt eller var inmatning av regnummer fel?" + e);
            }
            Console.ReadKey();
        }

        public void TaBortFordon(string regNr)
        {
            try
            {
                string ConnectionString = "Data Source=LAPTOP-K2JKI9TE\\SQLEXPRESS;Initial Catalog=PPDBAndreaEnvall;Integrated Security=SSPI";

                string sql = $"SELECT RegNr, Ankomst, Utcheckning, Summakostnad, AntalTimmar FROM Historik WHERE ID = (SELECT IDENT_CURRENT('Historik'))";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //Create the command object
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = "HittaFordonsTyp",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@RegNr", regNr);


                    //Set SqlParameter
                    SqlParameter outParameter = new SqlParameter()
                    {
                        ParameterName = "@FordonID", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    //add the parameter to the SqlCommand object
                    cmd.Parameters.Add(outParameter);

                    connection.Open();

                    cmd.ExecuteNonQuery();

                    SqlCommand command = new SqlCommand()
                    {
                        CommandText = "HittaPlatsID",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@RegNr", regNr);


                    //Set SqlParameter
                    SqlParameter outParam = new SqlParameter()
                    {
                        ParameterName = "@PlatsID", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    //add the parameter to the SqlCommand object
                    command.Parameters.Add(outParam);

                    
                    command.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand()
                    {
                        CommandText = "FordonProce",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd2.Parameters.AddWithValue("@FordonID", outParameter.Value);

                    SqlParameter outParameter2 = new SqlParameter()
                    {
                        ParameterName = "@FordonStorlek", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    //add the parameter to the SqlCommand object
                    cmd2.Parameters.Add(outParameter2);


                    cmd2.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand()
                    {
                        CommandText = "HittaPlatsdelete",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd3.Parameters.AddWithValue("@PlatsID", outParam.Value);
                    cmd3.Parameters.AddWithValue("@FordonStorlek", outParameter2.Value);

                    cmd3.ExecuteNonQuery();

                    SqlCommand cmd4 = new SqlCommand()
                    {
                        CommandText = "InsertHistorik",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd4.Parameters.AddWithValue("@RegNr", regNr);
                    

                    cmd4.ExecuteNonQuery();

                    SqlCommand cmd6 = new SqlCommand()
                    {
                        CommandText = "Pris",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd6.Parameters.AddWithValue("@RegNr", regNr);

                    SqlParameter outParameter3 = new SqlParameter()
                    {
                        ParameterName = "@Pris", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    //add the parameter to the SqlCommand object
                    cmd6.Parameters.Add(outParameter3);

                    cmd6.ExecuteNonQuery();


                    SqlCommand cmd5 = new SqlCommand()
                    {
                        CommandText = "Deleteproc",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd5.Parameters.AddWithValue("@RegNr", regNr);
                    


                    cmd5.ExecuteNonQuery();

                    SqlCommand cmd8 = new SqlCommand()
                    {
                        CommandText = "minutsproc",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd8.Parameters.AddWithValue("@RegNr", regNr);
                    cmd8.Parameters.AddWithValue("@Pris", outParameter3.Value);

                    SqlParameter outParameter4 = new SqlParameter()
                    {
                        ParameterName = "@Minuter", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    //add the parameter to the SqlCommand object
                    cmd8.Parameters.Add(outParameter4);


                    cmd8.ExecuteNonQuery();

                    SqlCommand cmd9 = new SqlCommand()
                    {
                        CommandText = "insertpris",
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd9.Parameters.AddWithValue("@RegNr", regNr);
                    cmd9.Parameters.AddWithValue("@Pris", outParameter3.Value);
                    cmd9.Parameters.AddWithValue("Minuter", outParameter4.Value);



                    cmd9.ExecuteNonQuery();



                    SqlCommand cmd10 = new SqlCommand(sql, connection);

                    SqlDataReader reader = cmd10.ExecuteReader();


                    if(reader.HasRows)

                        while (reader.Read())
                        {
                            Console.WriteLine("Fordon {0} är nu utcheckad!", regNr);
                            Console.WriteLine("RegNr: {0} | Ankomst: {1} | Utcheckning: {2} | Pris: {3}kr | Antaltimmar: {4} ", reader[0], reader[1], reader[2], reader[3], reader[4]);

                        }

                    else
                    {
                        Console.WriteLine("hej");
                    }
                    

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("AJDÅ! Något gick fel, kolla så att inmatningen av regnummer verkligen är korrekt" + e);
            }
            Console.ReadKey();
        }

        public void TommaPlatser()
        {
            string connectionstring = "Data Source=LAPTOP-K2JKI9TE\\SQLEXPRESS;Initial Catalog=PPDBAndreaEnvall;Integrated Security=SSPI";


            string sql = $"SELECT * FROM TommaPlatser ORDER BY PlatsNummer";



            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();


                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        Console.WriteLine(reader[0].ToString());

                    }
                }
                else
                {

                }
                Console.ReadLine();
                reader.Close();
            }
            
        }

        public void HittaFordon(string regNr)
        {

            string connectionstring = "Data Source=LAPTOP-K2JKI9TE\\SQLEXPRESS;Initial Catalog=PPDBAndreaEnvall;Integrated Security=SSPI";

            string sql = $"SELECT f.RegNr, p.PlatsNummer, f.Ankomst FROM Fordon f INNER JOIN Parkering p ON p.PlatsID = f.ParkeringsRuta WHERE RegNr = '{regNr}'";



            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                try
                {
                    SqlCommand cmd2 = new SqlCommand()
                    {
                        CommandText = "Pris",
                        Connection = con,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd2.Parameters.AddWithValue("@RegNr", regNr);



                    //Set SqlParameter
                    SqlParameter outParameter = new SqlParameter()
                    {
                        ParameterName = "@Pris", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Money, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    cmd2.Parameters.Add(outParameter);

                    con.Open();

                    cmd2.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand()
                    {
                        CommandText = "nuvarnademinuter",
                        Connection = con,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd3.Parameters.AddWithValue("@RegNr", regNr);
                    cmd3.Parameters.AddWithValue("@Pris", outParameter.Value);

                    SqlParameter outParameter2 = new SqlParameter()
                    {
                        ParameterName = "@Minuter", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Money, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    cmd3.Parameters.Add(outParameter2);

                    //Set SqlParameter


                    cmd3.ExecuteNonQuery();

                    SqlCommand cmd4 = new SqlCommand()
                    {
                        CommandText = "aktuellkostnad",
                        Connection = con,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd4.Parameters.AddWithValue("@RegNr", regNr);
                    cmd4.Parameters.AddWithValue("@Pris", outParameter.Value);
                    cmd4.Parameters.AddWithValue("@Minuter", outParameter2.Value);

                    SqlParameter outParameter4 = new SqlParameter()
                    {
                        ParameterName = "@Kostnad", //Parameter name defined in stored procedure
                        SqlDbType = SqlDbType.Money, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };
                    cmd4.Parameters.Add(outParameter4);

                    //Set SqlParameter


                    cmd4.ExecuteNonQuery();

                    SqlCommand cmd = new SqlCommand(sql, con);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {


                        while (reader.Read())
                        {

                            Console.WriteLine("RegNr: {0} | Plats: {1} | Ankomst: {2} | NuvarandePris: {3}", reader[0], reader[1], reader[2], outParameter4.Value.ToString());

                        }
                    }
                    else
                        Console.WriteLine("AJDÅ! Något gick fel, finns redan fordonet inlagt eller var inmatning av regnummer fel?");
                    reader.Close();
                }


                catch (FormatException ex)
                {
                    Console.WriteLine("AJDÅ! Något gick fel, finns redan fordonet inlagt eller var inmatning av regnummer fel?" + ex);
                }
                Console.ReadLine();
                
            }
        }
                
        
        public void Overview()
        {
            string connectionstring = "Data Source=LAPTOP-K2JKI9TE\\SQLEXPRESS;Initial Catalog=PPDBAndreaEnvall;Integrated Security=SSPI";


            string sql = $"SELECT f.RegNr, ft.FordonsTyp, p.PlatsNummer, f.Ankomst, f.AntalTimmar FROM Fordon f INNER JOIN FordonsTyp ft ON f.FordonsTypID = ft.FordonTypID INNER JOIN Parkering p ON p.PlatsID = f.ParkeringsRuta ORDER BY p.PlatsNummer";



            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();


                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        Console.WriteLine("RegNr: {0} | {1} | Plats: {2} | Ankomst: {3} | AntalTimmar: {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);

                    }
                }
                else
                {

                }
                Console.ReadLine();
                reader.Close();
            }

        
    }

        public void Historik()
        {
            string connectionstring = "Data Source=LAPTOP-K2JKI9TE\\SQLEXPRESS;Initial Catalog=PPDBAndreaEnvall;Integrated Security=SSPI";


            string sql = $"SELECT * FROM Historik";



            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();


                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine(FiggleFonts.Standard.Render("    Historik    "));
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        Console.WriteLine("RegNr: {0} | FordonTypID: {1}  | SummaKostnad {2} | AntalTimmar: {3} |\n",reader[0], reader[1], reader[4], reader[5]);
                        Console.WriteLine("Ankomst: {0} | Utcheckning: {1} | \n", reader[2], reader[3]);
                        Console.WriteLine("_______________________________________________________________________________________________");
                    }
                }
                else
                {

                }
                Console.ReadLine();
                reader.Close();
            }

        }
    } //END CLASS
}// END NAMESPACE

