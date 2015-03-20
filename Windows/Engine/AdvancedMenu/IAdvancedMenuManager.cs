using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.AdvancedMenu
{
    public interface IAdvancedMenuManager
    {
        Dictionary<string, IAdvancedMenuItem> Items { get; }
        bool ImportItemsFromXml(XElement node);
        bool ImportItems(List<AdvancedMenuItem> items);

        IResult ShowAdvancedMenu(IEngine engine, AbstractDialogModel parent, string group = "");

        void Initialize();
        
        string DefaultTitle { get; set; }
        string DefaultDescription { get; set; }
        string HelpTopic { get; set; }
        string WindowTitle { get; set; }
        double InitialLeftOffset { get; set; }
        double InitialTopOffset { get; set; }
        Brush DialogForeground { get; set; }
        Brush ContainerForeground { get; set; }
        Brush TopBannerForeground { get; set; }
        Brush DialogBackground { get; set; }
        Brush ContainerBackground { get; set; }
        Brush TopBannerBackground { get; set; }

        string HelpButtonText { get; set; }
        string RunButtonText { get; set; }
        string CloseButtonText { get; set; }

        string RunButtonImageKey { get; set; }
        string RunButtonMouseOverImageKey { get; set; }

        string HelpButtonImageKey { get; set; }
        string HelpButtonMouseOverImageKey { get; set; }

        string CloseButtonImageKey { get; set; }
        string CloseButtonMouseOverImageKey { get; set; }

        
    }
}
