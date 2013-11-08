using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers;

namespace Org.InCommon.InCert.Engine.Results.Errors.Mapping
{
    public abstract class AbstractErrorEntry : AbstractImportable
    {
        private readonly List<AbstractTextContentWrapper> _lines = new List<AbstractTextContentWrapper>();
        private readonly List<AbstractLink> _links = new List<AbstractLink>();

        protected AbstractErrorEntry(IEngine engine) : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Key { get; set; }

        [PropertyAllowedFromXml]
        public string Title { get; set; }

        [PropertyAllowedFromXml]
        public string Summary { get; set; }

        [PropertyAllowedFromXml]
        public string Topic { get; set; }

        [PropertyAllowedFromXml]
        public string AdvancedMenuGroup { get; set; }

        [PropertyAllowedFromXml]
        public List<AbstractLink> Links { get { return _links; } }
        
        [PropertyAllowedFromXml]
        public string Text
        {
            get
            {
                var buffer = new StringBuilder();
                foreach (var line in _lines)
                {
                    buffer.AppendLine(line.GetText());
                    buffer.AppendLine();
                }


                return buffer.ToString();
            }
        }

        public void AddLine(AbstractTextContentWrapper value)
        {
            if (value == null)
                return;

            _lines.Add(value);
        }

        public List<AbstractTextContentWrapper> GetLines()
        {
            return _lines;
        }

        public override void ConfigureFromNode(XElement node)
        {

            MapChildrenToProperties(node.Element("Properties"), this);

            var contentNode = node.Element("Content");
            if (contentNode != null)
            {
                foreach (var element in contentNode.Elements())
                {
                    var line = GetInstanceFromNode<AbstractTextContentWrapper>(element);
                    if (line == null)
                        continue;

                    if (!line.Initialized())
                        continue;

                    _lines.Add(line);
                }    
            }

            var linkNode = node.Element("Links");
            if (linkNode != null)
            {
                foreach (var element in linkNode.Elements())
                {
                    var link = GetInstanceFromNode<AbstractLink>(element);
                    if (link == null)
                        continue;

                    if (!link.Initialized())
                        continue;
                    
                    _links.Add(link);
                }
            }



        }

        public override bool Initialized()
        {
            if (string.IsNullOrWhiteSpace(Key))
                return false;

            if (string.IsNullOrWhiteSpace(Title))
                return false;

            if (string.IsNullOrWhiteSpace(Summary))
                return false;

            if (_lines.Count == 0)
                return false;

            return true;
        }
    }
}
