namespace FMA.Contracts
{
    public class CustomLogo
    {
        public CustomLogo()
        {

        }

        public CustomLogo(byte[] logo, double logoLeftMargin, double logoTopMargin, double logoWidth, double logoHeight)
        {
            Logo = logo;
            LogoLeftMargin = logoLeftMargin;
            LogoTopMargin = logoTopMargin;
            LogoWidth = logoWidth;
            LogoHeight = logoHeight;
        }

        public bool HasLogo => Logo != null && Logo.Length > 0;

        public byte[] Logo { get; set; }
        public double LogoLeftMargin { get; set; }
        public double LogoTopMargin { get; set; }
        public double LogoWidth { get; set; }
        public double LogoHeight { get; set; }
    }
}