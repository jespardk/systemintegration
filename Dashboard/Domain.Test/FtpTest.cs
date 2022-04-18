﻿using Common.Helpers;
using Xunit;
using Domain.PowerMeasurement;
using Domain.Caching;
using Domain.Configuration;
using Infrastructure.Caching;
using Infrastructure.Configuration;

namespace Domain.Test
{
    public class FtpTest
    {
        [Fact]
        public async Task TryServiceAsync()
        {
            Environment.SetEnvironmentVariable("PowerMeasurements.Username", "");
            Environment.SetEnvironmentVariable("PowerMeasurements.Password", "");
            Environment.SetEnvironmentVariable("PowerMeasurements.Url", "");

            var configurationRetriever = new ConfigurationRetriever(null);
            var cacheService = new InMemoryCache();
            var service = new PowerMeasurementRetriever(configurationRetriever, cacheService);
            await service.GetMeasurementsAsync();
        }

        [Fact]
        public void CanDecodeCsv()
        {
            var converted = CsvConverter.Convert<PowerMeasurementCsvSchema>(CsvExample);
        }

        private string CsvExample = @"
20211231;170002;danfoss;My Plant;+0100
[wr_def_start]
895541N541;Inverter Type;Inverter Group;Gruppe 1
[wr_def_end]
[wr]
INTERVAL;TIMESTAMP;SERIAL;P_AC;E_DAY;T_WR;U_AC;U_AC_1;U_AC_2;U_AC_3;I_AC;F_AC;U_DC_1;I_DC_1;U_DC_2;I_DC_2;U_DC_3;I_DC_3;S;E_WR;M_WR;I_AC_1;I_AC_2;I_AC_3;P_AC_1;P_AC_2;P_AC_3;F_AC_1;F_AC_2;F_AC_3;R_DC;PC;PCS;PCS_LL;COS_PHI;COS_PHI_LL;S_COS_PHI;Current_Day_Energy;current_Day_Offset;ccEnergyOfDay_WithoutOffset
60;2021-12-31 16:01:00;895541N541;22;8.245;33;233;231;233;235;0.065;50.03;524.8;0.050;227.5;0.000;284.6;0.000;60;0;0;0.047;0.064;0.085;5;8;7;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8245;8245;8245
60;2021-12-31 16:02:00;895541N541;18;8.247;33;233;232;232;235;0.050;50.04;525.8;0.042;246.1;0.000;255.1;0.000;60;0;0;0.046;0.052;0.054;5;5;6;50.04;50.04;50.04;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:03:00;895541N541;14;8.247;33;233;231;232;235;0.038;50.04;519.6;0.036;251.0;0.000;254.5;0.000;60;0;0;0.028;0.044;0.043;3;5;4;50.04;50.04;50.04;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:04:00;895541N541;10;8.247;33;233;231;232;235;0.028;50.03;494.2;0.031;250.9;0.000;254.5;0.000;60;0;0;0.020;0.044;0.021;2;5;2;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:05:00;895541N541;8;8.247;33;233;231;232;235;0.024;50.03;505.4;0.026;250.8;0.000;253.3;0.000;60;0;0;0.016;0.037;0.019;1;4;2;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:06:00;895541N541;5;8.247;32;233;231;233;235;0.015;50.03;451.9;0.024;250.8;0.000;252.9;0.000;60;0;0;0.008;0.028;0.009;1;3;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:07:00;895541N541;3;8.247;32;233;230;233;235;0.010;50.03;435.9;0.022;250.9;0.000;253.0;0.000;60;0;0;0.005;0.016;0.011;0;1;1;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:08:00;895541N541;2;8.247;32;233;231;233;234;0.010;50.03;434.3;0.022;251.0;0.000;252.6;0.000;60;0;0;0.005;0.015;0.011;0;1;1;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:09:00;895541N541;3;8.247;32;233;231;233;234;0.011;50.03;443.7;0.021;251.0;0.000;252.4;0.000;60;0;0;0.003;0.020;0.010;0;2;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:10:00;895541N541;3;8.247;32;233;231;233;234;0.009;50.03;444.2;0.019;250.9;0.000;252.4;0.000;60;0;0;0.005;0.009;0.015;0;1;1;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:11:00;895541N541;2;8.247;32;233;231;233;235;0.008;50.03;469.1;0.017;250.9;0.000;252.6;0.000;60;0;0;0.003;0.016;0.006;0;1;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:12:00;895541N541;0;8.247;32;232;231;232;235;0.003;50.03;457.3;0.013;250.8;0.000;251.9;0.000;60;0;0;0.001;0.006;0.002;0;0;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:13:00;895541N541;0;8.247;32;232;231;232;234;0.000;50.03;438.7;0.010;250.5;0.000;250.7;0.000;60;0;0;0.000;0.000;0.000;0;0;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:14:00;895541N541;0;8.247;32;232;231;233;233;0.000;50.03;423.9;0.007;250.5;0.000;250.4;0.000;60;0;0;0.000;0.000;0.000;0;0;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:15:00;895541N541;0;8.247;32;232;231;233;233;0.001;50.03;397.5;0.005;250.4;0.000;250.3;0.000;60;0;0;0.000;0.000;0.003;0;0;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:16:00;895541N541;0;8.247;32;228;227;230;228;0.000;50.03;468.9;0.000;250.1;0.000;250.1;0.000;50;0;0;0.000;0.000;0.000;0;0;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:17:00;895541N541;0;8.247;32;229;228;231;229;0.000;50.03;473.7;0.000;250.0;0.000;250.2;0.000;50;0;0;0.000;0.000;0.000;0;0;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:18:00;895541N541;0;8.247;31;230;228;232;230;0.000;50.03;452.1;0.000;249.9;0.000;250.3;0.000;50;0;0;0.000;0.000;0.000;0;0;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:19:00;895541N541;0;8.247;31;228;226;230;227;0.000;50.03;485.7;0.002;250.0;0.000;250.4;0.000;50;0;0;0.000;0.000;0.000;0;0;0;50.03;50.03;50.03;4352;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:20:00;895541N541;0;8.247;31;229;228;232;229;0.002;50.03;503.6;0.014;255.5;0.000;272.7;0.000;51;0;0;0.003;0.002;0.003;0;0;0;50.03;50.03;50.03;2867;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:21:00;895541N541;0;8.247;31;231;229;233;230;0.006;50.02;460.6;0.030;222.6;0.000;224.6;0.000;60;0;0;0.013;0.003;0.002;0;0;0;50.02;50.02;50.02;1154;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:22:00;895541N541;2;8.247;30;232;230;234;233;0.008;50.01;499.0;0.035;251.1;0.000;251.3;0.000;60;0;0;0.002;0.020;0.004;0;1;0;50.01;50.01;50.01;1154;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:23:00;895541N541;3;8.247;30;232;230;233;233;0.011;50.01;504.7;0.038;251.2;0.000;251.5;0.000;60;0;0;0.002;0.021;0.010;0;2;1;50.01;50.01;50.01;1154;1000;0.0;0;1.0000;0;2;8247;8247;8247
60;2021-12-31 16:24:00;895541N541;3;8.248;30;232;229;233;233;0.013;50.01;504.1;0.039;250.9;0.000;251.6;0.000;60;0;0;0.004;0.025;0.012;0;2;1;50.01;50.01;50.01;1154;1000;0.0;0;1.0000;0;2;8248;8248;8248
60;2021-12-31 16:25:00;895541N541;3;8.248;30;232;230;234;234;0.013;50.00;503.7;0.037;250.7;0.000;250.8;0.000;60;0;0;0.007;0.024;0.008;1;1;1;50.00;50.00;50.00;1154;1000;0.0;0;1.0000;0;2;8248;8248;8248
60;2021-12-31 16:26:00;895541N541;1;8.248;30;232;230;234;234;0.004;50.00;496.9;0.032;251.1;0.000;251.2;0.000;60;0;0;0.003;0.008;0.002;0;0;0;50.00;50.00;50.00;1154;1000;0.0;0;1.0000;0;2;8248;8248;8248
60;2021-12-31 16:27:00;895541N541;0;8.248;30;232;230;233;233;0.000;50.00;479.3;0.025;253.3;0.000;252.3;0.000;60;0;0;0.000;0.001;0.000;0;0;0;50.00;50.00;50.00;1154;1000;0.0;0;1.0000;0;2;8248;8248;8248
60;2021-12-31 16:28:00;895541N541;0;8.248;30;233;231;234;234;0.016;49.99;468.4;0.016;238.4;0.000;256.3;0.000;60;0;0;0.000;0.048;0.000;0;0;0;49.99;49.99;49.99;1154;1000;0.0;0;1.0000;0;2;8248;8248;8247
60;2021-12-31 16:29:00;895541N541;0;8.248;30;233;230;234;234;0.029;50.00;425.7;0.012;208.0;0.000;258.1;0.000;60;0;0;0.000;0.041;0.046;0;0;0;50.00;50.00;50.00;1154;1000;0.0;0;1.0000;0;2;8248;8248;8247
60;2021-12-31 16:30:00;895541N541;0;8.248;30;232;230;233;234;0.036;50.01;401.2;0.007;181.0;0.000;253.7;0.000;60;0;0;0.000;0.000;0.109;0;0;0;50.01;50.01;50.01;1154;1000;0.0;0;1.0000;0;2;8248;8248;8247
60;2021-12-31 16:31:00;895541N541;0;8.248;30;231;230;232;232;0.031;50.02;344.1;0.004;138.2;0.000;236.6;0.000;80;0;0;0.000;0.014;0.080;0;0;0;50.02;50.02;50.02;1154;1000;0.0;0;1.0000;0;2;8248;8248;8246
60;2021-12-31 16:32:00;895541N541;0;8.248;25;195;194;196;195;0.000;42.12;204.9;0.000;83.9;0.000;142.1;0.000;50;0;0;0.000;0.000;0.000;0;0;0;41.94;41.94;41.95;283;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:33:00;895541N541;0;8.248;30;232;230;233;232;0.000;50.01;228.1;0.000;112.1;0.000;162.4;0.000;50;0;0;0.000;0.000;0.000;0;0;0;50.01;50.01;50.01;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:34:00;895541N541;0;8.248;30;231;230;233;231;0.000;50.00;229.8;0.000;101.0;0.000;152.7;0.000;50;0;0;0.000;0.000;0.000;0;0;0;50.00;50.00;50.00;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:35:00;895541N541;0;8.248;29;231;230;234;231;0.000;49.99;226.9;0.000;91.8;0.000;144.4;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.99;49.99;49.99;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:36:00;895541N541;0;8.248;29;231;230;233;231;0.000;50.00;227.0;0.000;84.1;0.000;137.5;0.000;50;0;0;0.000;0.000;0.000;0;0;0;50.00;50.00;50.00;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:37:00;895541N541;0;8.248;29;232;230;234;232;0.000;49.99;226.6;0.000;77.0;0.000;131.2;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.99;49.99;49.99;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:38:00;895541N541;0;8.248;29;232;230;234;232;0.000;49.98;230.0;0.000;70.5;0.000;125.6;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.98;49.98;49.98;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:39:00;895541N541;0;8.248;28;232;230;233;232;0.000;49.98;227.0;0.000;64.0;0.000;119.6;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.99;49.99;49.99;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:40:00;895541N541;0;8.248;28;232;230;233;232;0.000;49.98;225.9;0.000;57.7;0.000;113.8;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.98;49.98;49.98;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:41:00;895541N541;0;8.248;28;231;230;233;231;0.000;49.97;228.2;0.000;51.8;0.000;108.5;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.97;49.97;49.97;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:42:00;895541N541;0;8.248;28;231;230;233;231;0.000;49.97;226.5;0.000;46.1;0.000;103.2;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.97;49.97;49.97;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:43:00;895541N541;0;8.248;27;231;230;233;231;0.000;49.97;228.2;0.000;41.0;0.000;98.6;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.97;49.97;49.97;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:44:00;895541N541;0;8.248;27;231;230;233;231;0.000;49.97;230.4;0.000;36.5;0.000;94.5;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.97;49.97;49.97;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:45:00;895541N541;0;8.248;27;231;230;233;231;0.000;49.96;222.3;0.000;32.1;0.000;90.0;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.96;49.96;49.96;0;1000;0.0;0;1.0000;0;2;8248;8248;0
60;2021-12-31 16:46:00;895541N541;0;8.248;27;231;230;233;231;0.000;49.98;208.7;0.000;27.9;0.000;85.4;0.000;50;0;0;0.000;0.000;0.000;0;0;0;49.98;49.98;49.98;0;1000;0.0;0;1.0000;0;2;8248;8248;0
[wr_ende]
";
    }
}
