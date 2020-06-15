namespace RingSoft.DataEntryControls.WPF
{
    public static class ExtensionMethods
    {
        public static System.Windows.Media.Color GetMediaColor(this System.Drawing.Color drawingColor)
        {
            return System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
        }
    }
}
