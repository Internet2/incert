namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ScriptingModels
{
    public enum LinkPolicy
    {
        None = 0, // allow none -- all links will be opened in external browser
        Internal = 1, // allow same uri and custom schemes (resource://, archive://)
        InternalAndSameHost = 2, // Internal + uri with same host as original
        All =3 // allow all
    }
}
