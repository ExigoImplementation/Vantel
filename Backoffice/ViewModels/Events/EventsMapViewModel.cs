using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backoffice.ViewModels
{
    public class EventsMapViewModel
    {
        public CalendarViewModel CalendarFilters { get; set; }
        public IEnumerable<CalendarEvent> Events { get; set; }
    }
}