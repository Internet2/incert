using Org.InCommon.InCert.Engine.Dynamics;
using Org.InCommon.InCert.Engine.Engines;

namespace EngineTests.Dynamics
{
    class MockDynamicObject:AbstractDynamicPropertyContainer
    {
        public MockDynamicObject(IEngine engine) : base(engine)
        {
        }

        public string DynamicProperty1
        {
            get { return GetDynamicValue(); }
            set {SetDynamicValue(value);}
        }

        public string DynamicProperty2
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        public string StaticProperty1 { get; set; }
        public string StaticProperty2 { get; set; }
    }
}
