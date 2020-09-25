using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZOI.BAL.Models;

namespace ZOI.APP.Models
{
    public class BookingInfoViewModel
    {
        public BookingInfoViewModel()
        {
            BookedGainLossData = new BookedGainLossData();
            ChartValues = new ChartValues();
        }
        public BookedGainLossData BookedGainLossData { get; set; }
        public ChartValues ChartValues { get; set; }
        public IEnumerable<BookedGainLossData> BookedList { get; set; }
        public IEnumerable<ChartValues> ChartList { get; set; }
    }
}
