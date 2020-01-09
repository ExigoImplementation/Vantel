namespace ExigoService
{
    public class DataRequest : IDataRequest
    {
        public DataRequest()
        {
            Page = 1;
            RowCount = 50;
        }

        public int Page { get; set; }
        public int RowCount { get; set; }
        public int TotalRowCount { get; set; }

        public int Skip
        {
            get { return (this.Page - 1) * this.RowCount; }
        }
        public int Take
        {
            get { return this.RowCount; }
        }
    }
}