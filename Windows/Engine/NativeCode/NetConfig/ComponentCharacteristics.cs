namespace Org.InCommon.InCert.Engine.NativeCode.NetConfig
{
    // adapted from http://www.codeproject.com/Articles/29700/AdapterList
    enum ComponentCharacteristics
    {
        Virtual = 0x00000001,
        SoftwareEnumerated = 0x00000002,
        Physical = 0x00000004,
        Hidden = 0x00000008,
        NoService = 0x00000010,
        NotUserRemovable = 0x00000020,
        MultiportInstancedAdapter = 0x00000040, // This adapter has separate instances for each port 
        HasUi = 0x00000080,
        SingleInstance = 0x00000100,
        // = 0x00000200, // filter device 
        Filter = 0x00000400, // filter component 
        Dontexposelower = 0x00001000,
        HideBinding = 0x00002000, // don't show in binding page 
        NdisProtocol = 0x00004000, // Needs UNLOAD notifications 
        // = 0x00008000, 
        // = 0x00010000, // pnp notifications forced through service controller 
        FixedBinding = 0x00020000 // UI ability to change binding is disabled 
    }
}