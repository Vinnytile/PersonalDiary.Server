using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models.Auth
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
