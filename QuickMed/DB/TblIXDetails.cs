﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.DB
{
    public class TblIXDetails
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TblIXTempMasterId { get; set; }
    }
}