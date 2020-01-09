namespace ReplicatedSite.ViewModels
{
    public class BootstrapColumnConfigViewModel
    {
        /// <summary>
        /// (optional) Number of columns to display at XS size
        /// <para>Must be greater than 0 and divisible by 12 otherwise it defaults to full width columns</para>
        /// </summary>
        public int Xs_Column_Qty { get; set; }
        /// <summary>
        /// Dynamically generated column class for XS responsive columns
        /// <para>Defaults to col-xs-12 when Xs_Column_Qty is invalid</para>
        /// </summary>
        public string Xs_Column_Class { get
            {
                if (this.Xs_Column_Qty <= 0) { return "col-xs-12"; }
                if (12 % this.Xs_Column_Qty != 0) { return "col-xs-12"; }
                return $"col-{12/this.Xs_Column_Qty}";
            }
        }
        /// <summary>
        /// (optional) Number of columns to display at SM size
        /// <para>Must be greater than 0 and divisible by 12 otherwise it is not used</para>
        /// </summary>
        public int? Sm_Column_Qty
        {
            get { return this._Sm_Column_Qty ?? this.Xs_Column_Qty; }// use the next smallest column quantity if not provided (needed for clearfix)
            set { this._Sm_Column_Qty = value; }
        }
        public int? _Sm_Column_Qty { get; set; }
        /// <summary>
        /// Dynamically generated column class for SM responsive columns
        /// <para>Defaults to empty string when Sm_Column_Qty is invalid</para>
        /// </summary>
        public string Sm_Column_Class
        {
            get
            {
                if (!this.Sm_Column_Qty.HasValue) { return string.Empty; }
                if (this.Sm_Column_Qty.Value == this.Xs_Column_Qty) { return string.Empty; }
                if (this.Sm_Column_Qty.Value <= 0) { return string.Empty; }
                if (12 % this.Sm_Column_Qty.Value != 0) { return string.Empty; }
                return $"col-sm-{12 / this.Sm_Column_Qty.Value}";
            }
        }
        /// <summary>
        /// (optional) Number of columns to display at MD size
        /// <para>Must be greater than 0 and divisible by 12 otherwise it is not used</para>
        /// </summary>
        public int? Md_Column_Qty {
            get { return this._Md_Column_Qty ?? this.Sm_Column_Qty; }// use the next smallest column quantity if not provided (needed for clearfix)
            set { this._Md_Column_Qty = value; }
        }
        public int? _Md_Column_Qty { get; set; }
        /// <summary>
        /// Dynamically generated column class for MD responsive columns
        /// <para>Defaults to empty string when Md_Column_Qty is invalid</para>
        /// </summary>
        public string Md_Column_Class
        {
            get
            {
                if (!this.Md_Column_Qty.HasValue) { return string.Empty; }
                if (this.Md_Column_Qty.Value == this.Sm_Column_Qty) { return string.Empty; }
                if (this.Md_Column_Qty.Value <= 0) { return string.Empty; }
                if (12 % this.Md_Column_Qty.Value != 0) { return string.Empty; }
                return $"col-md-{12 / this.Md_Column_Qty.Value}";
            }
        }
        /// <summary>
        /// (optional) Number of columns to display at LG size
        /// <para>Must be greater than 0 and divisible by 12 otherwise it is not used</para>
        /// </summary>
        public int? Lg_Column_Qty
        {
            get { return this._Lg_Column_Qty ?? this.Md_Column_Qty; }// use the next smallest column quantity if not provided (needed for clearfix)
            set { this._Lg_Column_Qty = value; }
        }
        public int? _Lg_Column_Qty { get; set; }
        /// <summary>
        /// Dynamically generated column class for LG responsive columns
        /// <para>Defaults to empty string when Lg_Column_Qty is invalid</para>
        /// </summary>
        public string Lg_Column_Class
        {
            get
            {
                if (!this.Lg_Column_Qty.HasValue) { return string.Empty; }
                if (this.Lg_Column_Qty.Value == this.Md_Column_Qty) { return string.Empty; }
                if (this.Lg_Column_Qty <= 0) { return string.Empty; }
                if (12 % this.Lg_Column_Qty != 0) { return string.Empty; }
                return $"col-lg-{12 / this.Lg_Column_Qty}";
            }
        }
    }
}