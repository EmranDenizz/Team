using System;
using System.Collections.Generic;
using System.Text;

namespace Team.Business.Helpers
{
    public class GetSettingsFile //localdb adresi kullanıldı.
    {
        public static readonly string ConnectionString = GenerateEncryptedString.Decrypt(@"RBLcyR7EvuaUc7gCfSr3WnnDbonffaopuBUAnP1qKOSoKSAw3mu5h1y0ppx165t3FRdyqBYP3FLo9rbMmWDnomFPGUluyIDHfarLEAkLjwE=");
    }
}
