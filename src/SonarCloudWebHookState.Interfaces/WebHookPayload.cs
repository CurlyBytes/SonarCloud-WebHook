using Newtonsoft.Json;

namespace SonarCloudWebHookState.Interfaces
{
    /// <summary>
    /// interface describing web hook payload as received by SonarCloud
    /// 
    /// used to deserialize
    /// 
    /// NOTE that this only covers properties that are used currently by the system and can be extended if further data is needed
    /// </summary>
    public class WebHookPayload
    {
        public QualityGate qualityGate { get; set; }
        public Properties properties { get; set; }        
    }

    public class Properties
    {
        [JsonProperty(PropertyName = "sonar.analysis.buildIdentifier")]
        public string buildIdentifier { get; set; }
    }

    public class QualityGate
    {
        public string status { get; set; }
    }
}
