using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class Message : BaseEntity
    {
        public string Title { get; set; }

        public string EmailAddress { get; set; }

        public string Content { get; set; }

        public DateTime SendOn { get; set; }

        public bool IsAnswered { get; set; }
    }
}
