using Org.InCommon.InCert.Engine.Engines;

namespace Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers
{
    class DirectTextContent:AbstractTextContentWrapper
    {
        public DirectTextContent() : base(null)
        {
        }
        
        public DirectTextContent(IEngine engine) : base(engine)
        {
        }

        public override string GetText()
        {
            return BaseText;
        }

        public override bool Initialized()
        {
            return true;
        }
    }
}
