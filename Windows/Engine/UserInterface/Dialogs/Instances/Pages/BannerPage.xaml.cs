using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Assistants;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Managers;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Instances.Pages
{
    /// <summary>
    /// Interaction logic for BannerPage.xaml
    /// </summary>
    public partial class BannerPage : IHasControlActions
    {
        private readonly IAppearanceManager _apperanceManager;
        private static readonly ILog Log = Logger.Create();

        private readonly List<AbstractControlAction> _actions = new List<AbstractControlAction>();

        public BannerPage()
        {
            InitializeComponent();
        }

        public BannerPage(AbstractBanner banner, IAppearanceManager apperanceManager)
        {
            _apperanceManager = apperanceManager;
            InitializeComponent();
            LoadBanner(banner);
        }

        public void LoadBanner(AbstractBanner banner)
        {
            if (banner == null)
                return;

            ContentPanel.Children.Clear();

            var binding = new Binding
                {
                    Source = _apperanceManager,
                    Path = new PropertyPath("DefaultFontFamily"),
                    Mode = BindingMode.OneWay
                };

            SetBinding(FontFamilyProperty, binding);

            foreach (var paragraph in banner.GetParagraphs())
            {
                /*var element = paragraph.GetWrappingElement(this);
                if (element == null)
                    continue;

                element.Tag = paragraph.Key;
                ContentPanel.Children.Add(element);*/
            }
        }
        
        public List<T> GetNamedElements<T>(string key) where T : FrameworkElement
        {
            return ContentPanel.Children.OfType<T>()
                .Where(element => element.Tag != null)
                .Where(element => element.Tag.ToString()
                    .Equals(key, StringComparison.Ordinal))
                    .ToList();
        }

        public void ClearDynamicControls()
        {
            _actions.Clear();
        }

        public void AddAction(AbstractControlAction action)
        {
           /* if (action == null)
                return;

            var elements = GetNamedElements<FrameworkElement>(action.Key);
            if (!elements.Any())
                return;

            _actions.Add(action);*/
        }

        public void AddActions(List<AbstractControlAction> actions)
        {
            /*var toAdd = (from action in actions
                         let elements = GetNamedElements<FrameworkElement>(action.Key)
                         where elements.Any()
                         select action).ToList();

            _actions.AddRange(toAdd);*/
        }

        public void DoActions(bool includeOneTime)
        {
           
        }

        public void RemoveOneTimeActions()
        {
            _actions.RemoveAll(c => c.OneTime);
        }

        public void ClearActions()
        {
            try
            {
                _actions.Clear();

                foreach (var child in ContentPanel.Children.OfType<PasswordBox>())
                {
                    if (!PasswordHelper.GetAttach(child))
                        continue;

                    PasswordHelper.SetAttach(child, false);
                    BindingOperations.ClearBinding(child, PasswordHelper.PasswordProperty);
                }
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }
    }
}
