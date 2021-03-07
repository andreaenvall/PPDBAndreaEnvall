using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDBAndreaEnvall
{
    public enum FordonsTyp
    {
        Car,
        MC
    }
    class Fordon
    {
        public string regNr { get; set; }
        public FordonsTyp fordonstyp { get; set; }
        public DateTime tidsstämpel { get; set; }
        public int pplats { get; set; }

        public Fordon()
        {

        }
    }
}
