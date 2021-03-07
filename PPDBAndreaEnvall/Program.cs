using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Figgle;

namespace PPDBAndreaEnvall
{
    class Program
    {
        static Parking parkering = new Parking();
        static void Main(string[] args)
        {
            bool menu = true;
            int choice = 0;
            do
            {
                choice = 0;
                //Console.Clear();
                Console.WriteLine(FiggleFonts.Standard.Render("     Parkering Prag    "));
                Console.WriteLine("________________________________________________________________");
                Console.WriteLine("{1} Lägg till Fordon | {2} Checka ut fordon  ");
                Console.WriteLine("________________________________________________________________");
                Console.WriteLine("{3} Hitta fordon     | {4} Översikt ");
                Console.WriteLine("________________________________________________________________");
                Console.WriteLine("{5} Tomma P-platser  | {6} Exit   ");
                Console.WriteLine("________________________________________________________________");
                Console.WriteLine("{7} Historik      |");
                Console.WriteLine("________________________________________________________________");

                Console.WriteLine("Menyval:");
                

                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        try
                        {
                            Fordon a = new Fordon();
                            
                            Console.WriteLine("Skriv in Registreringsnummer:");
                            a.regNr = Console.ReadLine();
                            Console.WriteLine("Vad vill du checka in för fordon? ");
                            Console.WriteLine("{1}. Bil  | {2}. MC");
                            int fordon = int.Parse(Console.ReadLine());

                            if (fordon == 1)
                            {
                                parkering.InsertSP(a.regNr, fordon);
                            }
                            else if (fordon == 2)
                            {
                                parkering.InsertSP(a.regNr, fordon);
                            }


                            break;

                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex);
                        }

                        break;
                    case 2:
                        Console.Clear();
                        try
                        {

                            Fordon a = new Fordon();

                            Console.WriteLine("Skriv in Registreringsnummer:");
                            a.regNr = Console.ReadLine();

                            parkering.TaBortFordon(a.regNr);



                            break;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }


                        break;
                    case 3:
                        Console.Clear();
                        try
                        {

                            Fordon a = new Fordon();

                            Console.WriteLine("Skriv in Registreringsnummer:");
                            a.regNr = Console.ReadLine();

                            parkering.HittaFordon(a.regNr);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex);
                        }


                        break;
                    case 4:
                        Console.Clear();
                        try
                        {
                            parkering.Overview();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        break;

                    case 5:
                        Console.Clear();
                        parkering.TommaPlatser();


                        break;
                    case 6:
                        Console.Clear();


                        Console.WriteLine("Exiting program..");
                        menu = false;
                        break;
                    default:
                        break;
                    case 7:
                        Console.Clear();
                        parkering.Historik();


                        break;


                }
            } while (menu == true);






        

    }// END MAIN
} //END CLASS
} // END NAMESPACE
