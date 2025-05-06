namespace Quorom.ViewModels
{
    public class TwoFactorAuthenticationVM
    {
        public string Code { get; set; }
        public string? Token { get; set; }
        public string? QRCodeURL { get; set; }
    }
}
