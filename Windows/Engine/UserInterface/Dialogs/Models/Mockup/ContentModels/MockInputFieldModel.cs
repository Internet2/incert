using System.Windows.Controls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.Mockup.ContentModels
{
    public class MockInputFieldModel : MockBaseContentModel
    {
        public bool IsReadOnly { get { return false; } }

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return ScrollBarVisibility.Hidden; }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get
            {
                return ScrollBarVisibility.Hidden;

            }
        }

        public int MaxLines
        {
            get { return 1; }
        }

        public int MinLines
        {
            get { return 1; }
        }

        public int MaxLength { get { return 100; } }

        public string Text { get { return "testing"; } }
    }
}
