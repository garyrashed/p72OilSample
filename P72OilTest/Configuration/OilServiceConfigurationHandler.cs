using System;
using System.Configuration;

namespace P72OilTest.Configuration
{
    public interface IOilServiceConfig
    {
        int Drills { get; }
        int DrillPeriod { get; }
        int DecayRate { get; }
        int MaxOutput { get; }
    }

    public class OilServiceConfig : IOilServiceConfig
    {
        public int Drills { get; }
        public int DrillPeriod { get; }
        public int DecayRate { get; }
        public int MaxOutput { get; }

        public OilServiceConfig()
        {
            var configHandler = (OilServiceConfigurationSectionHandler) ConfigurationManager.GetSection("OilService");
            Drills = configHandler.Drills;
            DrillPeriod = configHandler.DrillPeriod;
            DecayRate = configHandler.DecayRate;
            MaxOutput = configHandler.MaxOutput;
        }
    }

    public class OilServiceConfigurationSectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("drills", IsRequired = true)]
        public int Drills
        {
            get
            {
                return Convert.ToInt32(this["drills"]);
            }
            set { this["drills"] = value; }
        }

        [ConfigurationProperty("drillPeriod", IsRequired = true)]
        public int DrillPeriod
        {
            get
            {
                return Convert.ToInt32(this["drillPeriod"]);
            }
            set { this["drillPeriod"] = value; }
        }

        [ConfigurationProperty("decayRate", IsRequired = true)]
        public int DecayRate
        {
            get
            {
                return Convert.ToInt32(this["decayRate"]);
            }
            set { this["decayRate"] = value; }
        }


        [ConfigurationProperty("maxOutput", IsRequired = true)]
        public int MaxOutput
        {
            get
            {
                return Convert.ToInt32(this["maxOutput"]);
            }
            set { this["maxOutput"] = value; }
        }

    }
}
