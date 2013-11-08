namespace Org.InCommon.InCert.Engine.Results.Errors.Time
{
    class SystemClockWrong: ErrorResult
    {
        public double MinutesOff { get; set; }
        public double MaxAllowedDifference { get; set; }
    }
}
