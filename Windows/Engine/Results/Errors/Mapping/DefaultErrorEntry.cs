using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.TextWrappers;

namespace Org.InCommon.InCert.Engine.Results.Errors.Mapping
{
    class DefaultErrorEntry:AbstractErrorEntry
    {
        public DefaultErrorEntry(ErrorResult value):base(null)
        {
            Title = "Unknown Issue Occurred";
            Summary = "This entry gets returned if there is no entry in the error map for an issue";

            AddLine(value == null
                        ? new DirectTextContent {BaseText = "[unknown issue]"}
                        : new DirectTextContent {BaseText = value.GetType().ToString()});
        }
    }
}
