using System.Collections.Generic;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.Help
{
    public interface IHelpManager
    {
        void ShowHelpTopic(string value, AbstractDialogModel model);
        bool TopicExists(string value);
        void Initialize();
        bool ImportTopicsFromXml(XElement node);
        bool ImportTopics(List<HelpTopic> topics);
        string TopBannerText { get; set; }
        string HomeUrl { get; set; }
        string DialogTitle { get; set; }
        string PreserveContentText { get; set; }
        string AppendToExternalUris { get; set; }
        string BaseHelpUrl { get; set; }
        string ReportingEntry { get; set; }
        bool PreserveContentOnExit { get; set; }

        double InitialLeftOffset { get; set; }
        double InitialTopOffset { get; set; }
        
        void OpenCurrentViewInExternalWindow();
    }
}