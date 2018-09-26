namespace CamundaClient.Worker
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct)
    ]
    public sealed class ExternalTaskTopicAttribute : System.Attribute
    {
        public string TopicName { get; }
        public int Retries { get; } = 5; // default: 5 times
        public long RetryTimeout { get; } = 10 * 1000; // default: 10 seconds

        public ExternalTaskTopicAttribute(string topicName)
        {
            TopicName = topicName;
        }

        public ExternalTaskTopicAttribute(string topicName, int retries, long retryTimeout)
        {
            TopicName = topicName;
            Retries = retries;
            RetryTimeout = retryTimeout;
        }
    }
}
