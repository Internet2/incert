namespace Org.InCommon.InCert.DataContracts
{
    public class FileInfoWrapper
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileSha1 { get; set; }
        public long FileSize { get; set; }
        public string Version { get; set; }
        public bool Vital { get; set; }
        public string MsiProductCode { get; set; }
        public string MsiUpgradeCode { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public string BaseUrl { get; set; }


        
    }
}
