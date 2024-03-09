namespace ABPTest.Models
{
    public class ExperimentResult : BaseDbModel
    {
        public ExperimentType ExperimentType { get; set; }
        public string DeviceToken { get; set; }
        public string Value { get; set; }
    }
}
