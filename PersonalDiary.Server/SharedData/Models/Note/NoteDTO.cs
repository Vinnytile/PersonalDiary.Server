﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class NoteDTO
    {
        public string Description { get; set; }
        public string Text { get; set; }

        public Guid UserId { get; set; }
    }
}
