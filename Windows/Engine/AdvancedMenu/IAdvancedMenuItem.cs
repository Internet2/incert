namespace Org.InCommon.InCert.Engine.AdvancedMenu
{
    public interface IAdvancedMenuItem
    {
        bool Show { get; }
        string Group { get; set; }
        string ButtonText { get; set; }
        string Branch { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string WorkingTitle { get; set; }
        string WorkingDescription { get; set; }
    }
}
