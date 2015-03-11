using System;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.Errors;
using Org.InCommon.InCert.Engine.Results.Errors.General;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    class ShowHtmlErrorBanner : ShowHtmlBannerModal
    {
        [PropertyAllowedFromXml]
        public string ErrorKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ErrorInfoKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string TopicKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string AdvancedMenuGroupKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }
        
        public ShowHtmlErrorBanner(IEngine engine) : base(engine)
        {
            Url = "resource://html/error.html";
            Dialog = "Main dialog";
            ErrorInfoKey = "current issue info";
            ErrorKey = "CurrentIssue";
            TopicKey = "current issue help topic";
            AdvancedMenuGroupKey = "current issue advanced menu group";
        }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                var issue = GetErrorResult(previousResults);
                var info = ErrorManager.GetErrorEntry(issue);
                SettingsManager.SetTemporaryObject(ErrorInfoKey, info);
                SettingsManager.SetTemporarySettingString(TopicKey, info.Topic);
                SettingsManager.SetTemporarySettingString(AdvancedMenuGroupKey, info.AdvancedMenuGroup);

                return base.Execute(previousResults);
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private ErrorResult GetErrorResult(IResult previousResults)
        {
            var result = SettingsManager.GetTemporaryObject(ErrorKey) as ErrorResult;
            if (result != null)
                return result;

            if (previousResults is ErrorResult)
                SettingsManager.SetTemporaryObject(ErrorKey, previousResults);
            else
                SettingsManager.SetTemporaryObject(
                    ErrorInfoKey,
                    new ExceptionOccurred(
                        new Exception(
                            "Could not determine issue details. Please consult the utility log for more details.")));

            return SettingsManager.GetTemporaryObject(ErrorKey) as ErrorResult;
        }

        public override string GetFriendlyName()
        {
            return "Show error dialog";
        }

    }
    
}
