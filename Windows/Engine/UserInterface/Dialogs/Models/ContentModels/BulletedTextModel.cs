using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class BulletedTextModel : AbstractContentModel
    {
        private static readonly ILog Log = Logger.Create();
        
        private TextContentModel _contentModel;

        public BulletedTextModel(AbstractModel parentModel)
            : base(parentModel)
        {
        }

        public TextContentModel ContentModel
        {
            get
            {
                return _contentModel;
            }
            private set
            {
                _contentModel = value;
                ChildModels.Clear();
                ChildModels.Add(value.ControlKey, value);
                OnPropertyChanged();
            }
        }

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            try
            {
                var indent = GetIndentFromWrapper(wrapper as BulletParagraph);
                
                var contentModel = new TextContentModel(this);
                contentModel.LoadContent<TextBlock>(wrapper);
                contentModel.ControlKey = wrapper.ControlKey;
                contentModel.Margin = new Thickness(indent + contentModel.FontSize,0,4,4);
                ContentModel = contentModel;

                var size = ContentModel.FontSize * .5;
                if (size <= 0)
                    size = 6;

                var decorator = new BulletDecorator
                    {
                        Bullet = new Ellipse
                            {
                                Height = size,
                                Width = size,
                                Margin = new Thickness(indent, ContentModel.Padding.Top + 1, 8, 0)
                            },
                        Child = ContentModel.Content as UIElement
                    };

                DockPanel.SetDock(decorator, Dock.Top);
                Content = decorator;

                InitializeBindings(decorator);
                InitializeValues(wrapper);

                return decorator as T;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return default(T);
            }
        }

        protected override void InitializeBindings(DependencyObject target)
        {
            base.InitializeBindings(target);

            var decorator = target as BulletDecorator;
            if (decorator == null)
                return;

            var ellipse = decorator.Bullet as Ellipse;
            if (ellipse == null)
                return;

            ellipse.SetBinding(Shape.FillProperty, GetOneWayBinding(this, "TextBrush"));
        }

        private static int GetIndentFromWrapper(BulletParagraph wrapper)
        {
            return wrapper == null ? 0 : wrapper.Indent;
        }
    }
}
