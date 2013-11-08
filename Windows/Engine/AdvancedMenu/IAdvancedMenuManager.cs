using System.Collections.Generic;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.DialogModels;

namespace Org.InCommon.InCert.Engine.AdvancedMenu
{
    public interface IAdvancedMenuManager
    {
        Dictionary<string, IAdvancedMenuItem> Items { get; }
        bool ImportItemsFromXml(XElement node);
        bool ImportItems(List<AdvancedMenuItem> items);
        void ShowAdvancedMenu(AbstractDialogModel dialogModel, string group);

        void Initialize();
        
        string DefaultTitle { get; set; }
        string DefaultDescription { get; set; }
        string HelpTopic { get; set; }
        string WindowTitle { get; set; }
        double InitialLeftOffset { get; set; }
        double InitialTopOffset { get; set; }
    }
}
