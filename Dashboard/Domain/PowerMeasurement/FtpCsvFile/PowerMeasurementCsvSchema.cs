namespace Domain.PowerMeasurement.FtpCsvFile
{
    public class PowerMeasurementCsvSchema
    {
        public int INTERVAL { get; set; }
        public DateTime TIMESTAMP { get; set; }
        public string? SERIAL { get; set; }
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
}