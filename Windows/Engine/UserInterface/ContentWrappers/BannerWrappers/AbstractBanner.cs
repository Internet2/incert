using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ButtonWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.ControlActions;
using Org.InCommon.InCert.Engine.Utilities;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.BannerWrappers
{
    public abstract class AbstractBanner : AbstractImportable
    {
        private static readonly ILog Log = Logger.Create();

        private readonly List<AbstractImportable> _members = new List<AbstractImportable>();

        protected AbstractBanner(IEngine engine) : base(engine)
        {
        }

        public string Name { get; set; }
        public bool Scrollable { get; set; }
        public bool AlwaysRefresh { get; set; }
        public bool Activate { get; set; }
        public string Background { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public WindowStyle WindowStyle { get; set; }
        public Thickness Margin { get; set; }
        public bool CanClose { get; set; }
        public bool SuppressCloseQuestion { get; set; }
        public Cursor Cursor { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public void AddMember(AbstractImportable value)
        {
            _members.Add(value);
        }

        public List<AbstractContentWrapper> GetParagraphs()
        {
            return _members
                .OfType<AbstractContentWrapper>()
                .ToList<AbstractContentWrapper>();
        }

        public List<AbstractButton> GetButtons()
        {
            return _members
                .OfType<AbstractButton>()
                .ToList<AbstractButton>();
        }

        public List<AbstractControlAction> GetActions()
        {
            return _members.OfType<AbstractControlAction>().ToList<AbstractControlAction>();
        }

        private void AddMembersFromXml<T>(ICollection<XElement> nodes) where T : AbstractImportable
        {
            if (nodes == null)
                return;

            if (nodes.Count == 0)
                return;

            foreach (var instance in nodes.Select((GetInstanceFromNode<T>)))
            {
                if (instance == null || !instance.Initialized())
                {
                    Log.Warn("could not instantiate member from xml");
                    continue;
                }

                AddMember(instance);
            }
        }


        public override bool Initialized()
        {
            if (GetParagraphs().Count == 0)
                return false;

            return (!String.IsNullOrWhiteSpace(Name));
        }

        public override void ConfigureFromNode(XElement node)
        {
            Name = XmlUtilities.GetTextFromAttribute(node, "name");
            AlwaysRefresh = XmlUtilities.GetBooleanFromAttribute(node, "alwaysRefresh", false);
            Activate = XmlUtilities.GetBooleanFromAttribute(node, "activate", true);
            Scrollable = XmlUtilities.GetBooleanFromAttribute(node, "scrollable", false);
            Background = XmlUtilities.GetTextFromAttribute(node, "background", "");
            Width = XmlUtilities.GetDoubleFromAttribute(node, "width", double.NaN);
            Height = XmlUtilities.GetDoubleFromAttribute(node, "height", double.NaN);
            Activate = XmlUtilities.GetBooleanFromAttribute(node, "activate", true);
            WindowStyle = XmlUtilities.GetEnumValueFromAttribute(node, "style", WindowStyle.SingleBorderWindow);

            CanClose = XmlUtilities.GetBooleanFromAttribute(node, "canClose", true);

            SuppressCloseQuestion = XmlUtilities.GetBooleanFromAttribute(node, "noCloseQuestion",false);

            Margin = XmlUtilities.ConvertFromAttributeUsingConverter(node, "margin",
                                                                                 new ThicknessConverter(),
                                                                                 new Thickness(14, 12, 14, 12));

            VerticalAlignment = XmlUtilities.GetEnumValueFromAttribute(node, "verticalAlignment",
                                                                       VerticalAlignment.Stretch);

            HorizontalAlignment = XmlUtilities.GetEnumValueFromAttribute(node, "horizontalAlignment",
                                                                         HorizontalAlignment.Stretch);

            Cursor = XmlUtilities.GetTextFromAttribute(node, "cursor").ConvertToCursor(Cursors.Arrow);

            var contentNode = node.Element("Content");
            if (contentNode != null)
                AddMembersFromXml<AbstractContentWrapper>(contentNode.Elements().ToList());

            var buttonsNode = node.Element("Buttons");
            if (buttonsNode != null)
                AddMembersFromXml<AbstractButton>(buttonsNode.Elements().ToList());

            var actionsNode = node.Element("Actions");
            if (actionsNode != null)
                AddMembersFromXml<AbstractControlAction>(actionsNode.Elements().ToList());
        }

    
    }
}
