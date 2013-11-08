using System;
using Org.InCommon.InCert.Engine.Results.Misc;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Utilities;

namespace Org.InCommon.InCert.Engine.Conditions.WindowsRegistry
{
    class LongDateWithinRange:AbstractRegistryCondition
    {
        public LongDateWithinRange(IEngine engine):base(engine)
        {
        }

        public TimeSpan Range { get; set; }

        public override BooleanReason Evaluate()
        {
            try
            {
                using (var key = RegistryRoot.OpenRegistryKey(RegistryKey, false))
                {
                    if (key == null)
                        return new BooleanReason(false, "Could not open {0}\\{1}", RegistryRoot, RegistryKey);

                    var bytes = key.GetValue(ValueName) as byte[];
                    if (bytes == null)
                        return new BooleanReason(false, "No value exists for {0} in the key {1}\\{2}, or the value is invalid", ValueName, RegistryRoot, RegistryKey);

                    var longValue = BitConverter.ToInt64(bytes,0);
                    var registryDate = DateTime.FromFileTimeUtc(longValue);

                    var elapsed = DateTime.UtcNow.Subtract(registryDate);

                    return elapsed.Duration()>Range.Duration() ?
                        new BooleanReason(false, "The stored date ({0}) falls outside the given range", registryDate) 
                        : new BooleanReason(true, "The stored date ({0}) is within the given range", registryDate);
                }

            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An exception occurred while evaluating the condition: {0}", e.Message);
            }
        }

        public override bool IsInitialized()
        {
            if (!base.IsInitialized())
                return false;

            return Range.Duration().Ticks>0;
        }
        
        public override void ConfigureFromNode(System.Xml.Linq.XElement node)
        {
            base.ConfigureFromNode(node);

            var rangeText = XmlUtilities.GetTextFromAttribute(node, "range");
            if (!string.IsNullOrWhiteSpace(rangeText))
            {
                TimeSpan rangeValue;
                TimeSpan.TryParse(rangeText, out rangeValue);
                Range = rangeValue;
            }
                

        }
    }
}
