using System.Windows;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.CustomControls;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class CheckedParagraphModel : AbstractContentModel
    {
        private CheckedParagraphState _state;
        private string _text;
        private AbstractContentModel _lowerContent;

        public enum CheckedParagraphState
        {
            Inactive,
            Active,
            Complete,
            Disabled
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                if (!value)
                    State=CheckedParagraphState.Disabled;

                base.Enabled = value;
                FlagPropertyAsChanged("TextBrush");
                FlagPropertyAsChanged("CheckBrush");
            }
        }

        public CheckedParagraphModel(AbstractModel parentModel)
            : base(parentModel)
        {
            _state = CheckedParagraphState.Inactive;
        }

        public AbstractContentModel LowerContent
        {
            get { return _lowerContent; }
            set { _lowerContent = value; OnPropertyChanged(); }
        }

        public CheckedParagraphState State
        {
            get { return _state; }
            set
            {
                _state = value;
                FlagPropertyAsChanged("CheckBrush");
                FlagPropertyAsChanged("TextBrush");
            }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }

        public Brush CheckBrush
        {
            get
            {
                return State == CheckedParagraphState.Complete ? AppearanceManager.LinkTextBrush : new SolidColorBrush(Colors.Transparent);
            }
        }

        public override Brush TextBrush
        {
            get
            {
                if (State == CheckedParagraphState.Disabled)
                    return base.TextBrush.MakeTransparent(45);
                
                if (State == CheckedParagraphState.Active)
                    return AppearanceManager.LinkTextBrush;

                return base.TextBrush;
            }
        }

        public override T LoadContent<T>(ContentWrappers.ContentControlWrappers.AbstractContentWrapper wrapper)
        {
            var content = new CheckedParagraph { DataContext = this };
            Text = wrapper.GetText();

            InitializeBindings(content);
            InitializeValues(wrapper);

            Content = content;
            Padding = new Thickness(0);
          
            return content as T;
        }



    }
}
