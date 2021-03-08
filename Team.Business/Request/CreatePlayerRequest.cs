using System;
using System.Collections.Generic;
using System.Text;

namespace Team.Business.Request
{
    public class CreatePlayerRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Country { get; set; }
        public string Position { get; set; }
    }
}
