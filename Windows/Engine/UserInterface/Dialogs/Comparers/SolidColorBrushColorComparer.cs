using System.Collections;
using System.Windows.Media;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Comparers
{
    class SolidColorBrushColorComparer:IComparer
    {
        public int Compare(object x, object y)
        {
            var brush1 = x as SolidColorBrush;
            if (brush1 == null)
                return -1;
            
            var brush2 = y as SolidColorBrush;
            if (brush2 == null)
                return -1;

            if (brush1.Color != brush2.Color)
                return -1;

            return 0;
        }
    }
}
