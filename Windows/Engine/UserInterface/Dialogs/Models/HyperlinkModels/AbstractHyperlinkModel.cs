using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels
{
    abstract class AbstractHyperlinkModel: AbstractModel
    {
        private ICommand _command;

        protected AbstractHyperlinkModel(AbstractModel parentModel) : base(parentModel)
        {
            parentModel.PropertyChanged += PropertyChangedHandler;
        }

        protected Brush LinkBrush;

        public virtual void LoadContent(AbstractLink wrapper)
        {
            var content = new Hyperlink(new Run(wrapper.GetText()))
            {
                Style = GetNamedStyle("HyperlinkStyle")
            };

            LinkBrush = AppearanceManager.GetBrushForColor(wrapper.Color, AppearanceManager.LinkTextBrush);
            SetBindings(content);
            Content = content;
            Enabled = true;
        }

        public virtual Brush TextBrush
        {
            get
            {
                if (!Parent.Enabled)
                    return AppearanceManager.MakeBrushTransparent(LinkBrush as SolidColorBrush, 45);

                if (!Enabled)
                    return AppearanceManager.MakeBrushTransparent(LinkBrush as SolidColorBrush, 45);

                return LinkBrush;

            }
        }
        
        public virtual ICommand Command
        {
            get { return _command;  }
            set { _command = value; OnPropertyChanged(); }
        }

        protected virtual void SetBindings(Hyperlink instance)
        {
            SetEnabledBinding(instance);    
            SetForegroundBinding(instance);
            SetCommandBinding(instance);
        }

        protected virtual void SetEnabledBinding(Hyperlink instance)
        {
            instance.SetBinding(ContentElement.IsEnabledProperty, GetOneWayBinding(Parent, "Enabled"));
        }

        protected virtual void SetForegroundBinding(Hyperlink instance)
        {
            instance.SetBinding(TextElement.ForegroundProperty, GetOneWayBinding(this, "TextBrush"));
        }

        protected virtual void SetCommandBinding(Hyperlink instance)
        {
            instance.SetBinding(Hyperlink.CommandProperty, GetOneWayBinding(this, "Command"));
        }

        protected void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (!args.PropertyName.Equals("Enabled", StringComparison.InvariantCulture))
                return;

            FlagPropertyAsChanged("Enabled");
            FlagPropertyAsChanged("TextBrush");
        }
    }
}
