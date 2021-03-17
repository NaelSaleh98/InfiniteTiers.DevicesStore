using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Presentation.Models
{
    public class MailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
