namespace CamundaClient.Dto
{
    public class ProcessDefinitionXml
    {
        public string Id { get; set; }       
        public string Bpmn20Xml { get; set; }

        public override string ToString() => $"ProcessDefinitionXml [Id={Id}]";
    }

}
