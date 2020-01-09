namespace ExigoService
{
    public class VerifyAddressResponse
    {
        public bool IsValid { get; set; }
        public IAddress OriginalAddress { get; set; }
        public IAddress VerifiedAddress { get; set; }
    }
}