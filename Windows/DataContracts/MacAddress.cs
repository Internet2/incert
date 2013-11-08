using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Org.InCommon.InCert.DataContracts
{
    public class MacAddress
    {
        private string _address;

        public long Id { get; set; }
        public string Address
        {
            get { return _address; }
            set { _address = NormalizeAddress(value); }
        }

        public string Description { get; set; }
        
        public static string NormalizeAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";

            value = value.Trim();
            if (string.IsNullOrWhiteSpace(value))
                return "";

            value = value.Replace("-", "");
            value = value.Replace(":", "");
            return value;
        }

        public static string FormatForLogging(string address)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address))
                    return address;

                var result = new StringBuilder(address, address.Length + 5);

                //var result = Address;
                var insertedCount = 0;
                for (var i = 2; i < address.Length; i = i + 2)
                    result = result.Insert(i + insertedCount++, ':');

                return result.ToString().ToUpperInvariant();
            }
            catch (Exception)
            {
                return address;
            }
        }
        public string FormatForLogging()
        {
            return FormatForLogging(Address);
        }

        private static readonly Regex ValidationPattern = new Regex("^([0-9a-f]{2}){5}([0-9a-f]{2})$", RegexOptions.Compiled);

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Address))
                yield return new ValidationResult("Address cannot be null or empty", new[] { "Address" });

            if (!ValidationPattern.IsMatch(Address))
                yield return new ValidationResult("Address must be a valid mac address");
        }
        
    }
}