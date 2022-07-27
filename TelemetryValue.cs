namespace SimFeedback.telemetry
{
    public class SpecificTelemetryValue : AbstractTelemetryValue
    {
        public SpecificTelemetryValue(string name, object value) : base()
        {
            Name = name;
            Value = value;
        }

        public override object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Value, this.Unit);
        }
    }
}