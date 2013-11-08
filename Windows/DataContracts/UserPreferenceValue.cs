namespace Org.InCommon.InCert.DataContracts
{
    public class UserPreferenceValue
    {
        public long Id { get; set; }
        public virtual User User { get; set; }
        public virtual Preference Preference { get; set; }
        public string Value { get; set; }
    }
}
