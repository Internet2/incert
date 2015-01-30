﻿using System;
using System.Windows;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Results.Errors.UserInterface;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowHtmlBannerModal : AbstractTask
    {
        public ShowHtmlBannerModal(IEngine engine)
            : base(engine)
        {
        }

        private readonly string _identifier = Guid.NewGuid().ToString();

        [PropertyAllowedFromXml]
        public string Url
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string Dialog
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var dialog = DialogsManager.GetDialog<BorderedDialogModel>(Dialog);
                if (dialog == null)
                    return new DialogInstanceNotFound { Dialog = Dialog };

                var banner = GetOrCreateBanner();

                DialogsManager.ActiveDialogKey = Dialog;

                return dialog.ShowBannerModal(banner);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private AbstractBanner GetOrCreateBanner()
        {
            var banner = BannerManager.GetBanner(_identifier);
            if (banner != null)
            {
                return banner;
            }

            var wrapper = new BrowserContentWrapper(Engine)
            {
                Uri = new Uri(Url),
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                SilentMode = true
            };

            banner = new SimpleBanner(Engine)
            {
                Width = 600,
                Height = 600,
                CanClose = true,
                Margin = new Thickness(0)
            };

            banner.AddMember(wrapper);
            return BannerManager.SetBanner(_identifier, banner);
        }

        public override string GetFriendlyName()
        {
            return string.Format("Show html banner {0} in model dialog {1}", Url, Dialog);
        }
    }
}
