using StarStrap.Enums;

namespace StarStrap.AppData
{
    public class AppSettings
    {
        public string CustomFontLocation { get; set; } = string.Empty;
        public CursorType CursorType { get; set; } = CursorType.Default;
        public bool UseFastFlagManager { get; set; }
        public bool StarStrapRPCReal { get; set; }
        public bool WPFSoftwareRender { get; set; }
        public string Locale { get; set; } = "nil";
        public string? SelectedCustomTheme { get; set; }
    }
}
