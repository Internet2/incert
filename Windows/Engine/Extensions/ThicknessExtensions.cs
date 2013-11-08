using System.Windows;

namespace Org.InCommon.InCert.Engine.Extensions
{
    public static class ThicknessExtensions
    {
        public static Thickness Add(this Thickness value1, Thickness value2)
        {
            return new Thickness
                       {
                           Top = value1.Top + value2.Top,
                           Bottom = value1.Bottom + value2.Bottom,
                           Left = value1.Left + value2.Left,
                           Right = value1.Right + value2.Right
                       };
        }
    }
}
