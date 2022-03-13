using CsvHelper.Configuration;

namespace Case.Services.Schema
{
    public class PowermeasurementCsvSchema
    {
        public int INTERVAL { get; set; }
        public DateTime TIMESTAMP { get; set; }
        public string SERIAL { get; set; }
        public int P_AC { get; set; }
        public double E_DAY { get; set; }
        public int T_WR { get; set; }
        public int U_AC { get; set; }
        public int U_AC_1 { get; set; }
        public int U_AC_2 { get; set; }
        public int U_AC_3 { get; set; }
        public double I_AC { get; set; }
        public double F_AC { get; set; }
        public double U_DC_1 { get; set; }
        public double I_DC_1 { get; set; }
        public double U_DC_2 { get; set; }
        public double I_DC_2 { get; set; }
        public double U_DC_3 { get; set; }
        public double I_DC_3 { get; set; }
        public int S { get; set; }
        public int E_WR { get; set; }
        public int M_WR { get; set; }
        public double I_AC_1 { get; set; }
        public double I_AC_2 { get; set; }
        public double I_AC_3 { get; set; }
        public int P_AC_1 { get; set; }
        public int P_AC_2 { get; set; }
        public int P_AC_3 { get; set; }
        public double F_AC_1 { get; set; }
        public double F_AC_2 { get; set; }
        public double F_AC_3 { get; set; }
        public int R_DC { get; set; }
        public int PC { get; set; }
        public double PCS { get; set; }
        public int PCS_LL { get; set; }
        public double COS_PHI { get; set; }
        public int COS_PHI_LL { get; set; }
        public int S_COS_PHI { get; set; }
        public int Current_Day_Energy { get; set; }
        public int current_Day_Offset { get; set; }
        public int ccEnergyOfDay_WithoutOffset { get; set; }
    }

    public class ModelClassMap : ClassMap<PowermeasurementCsvSchema>
    {
        public ModelClassMap()
        {
            Map(m => m.INTERVAL).Name("INTERVAL");
            Map(m => m.TIMESTAMP).Name("TIMESTAMP");
            Map(m => m.SERIAL).Name("SERIAL");
            Map(m => m.P_AC).Name("P_AC");
            Map(m => m.E_DAY).Name("E_DAY");
            Map(m => m.T_WR).Name("T_WR");
            Map(m => m.U_AC).Name("U_AC");
            Map(m => m.U_AC_1).Name("U_AC_1");
            Map(m => m.U_AC_2).Name("U_AC_2");
            Map(m => m.U_AC_3).Name("U_AC_3");
            Map(m => m.I_AC).Name("I_AC");
            Map(m => m.F_AC).Name("F_AC");
            Map(m => m.U_DC_1).Name("U_DC_1");
            Map(m => m.I_DC_1).Name("I_DC_1");
            Map(m => m.U_DC_2).Name("U_DC_2");
            Map(m => m.I_DC_2).Name("I_DC_2");
            Map(m => m.U_DC_3).Name("U_DC_3");
            Map(m => m.I_DC_3).Name("I_DC_3");
            Map(m => m.S).Name("S");
            Map(m => m.E_WR).Name("E_WR");
            Map(m => m.M_WR).Name("M_WR");
            Map(m => m.I_AC_1).Name("I_AC_1");
            Map(m => m.I_AC_2).Name("I_AC_2");
            Map(m => m.I_AC_3).Name("I_AC_3");
            Map(m => m.P_AC_1).Name("P_AC_1");
            Map(m => m.P_AC_2).Name("P_AC_2");
            Map(m => m.P_AC_3).Name("P_AC_3");
            Map(m => m.F_AC_1).Name("F_AC_1");
            Map(m => m.F_AC_2).Name("F_AC_2");
            Map(m => m.F_AC_3).Name("F_AC_3");
            Map(m => m.R_DC).Name("R_DC");
            Map(m => m.PC).Name("PC");
            Map(m => m.PCS).Name("PCS");
            Map(m => m.PCS_LL).Name("PCS_LL");
            Map(m => m.COS_PHI).Name("COS_PHI");
            Map(m => m.COS_PHI_LL).Name("COS_PHI_LL");
            Map(m => m.S_COS_PHI).Name("S_COS_PHI");
            Map(m => m.Current_Day_Energy).Name("Current_Day_Energy");
            Map(m => m.current_Day_Offset).Name("current_Day_Offset");
            Map(m => m.ccEnergyOfDay_WithoutOffset).Name("ccEnergyOfDay_WithoutOffset");
        }
    }

}