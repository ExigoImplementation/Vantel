using ExigoService;
using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class WeekSalesTotalNode
    {
        public int Week { get; set; }
        public decimal Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActiveWeek { get; set; }
    }
}