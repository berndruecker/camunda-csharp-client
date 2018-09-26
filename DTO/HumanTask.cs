using System;
using System.Collections.Generic;

namespace CamundaClient.Dto
{
    public class HumanTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Assignee { get; set; }
        public string Owner { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Due { get; set; }
        public DateTime? FollowUp { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string FormKey { get; set; }
        public string ProcessInstanceId { get; set; }
        public string ProcessDefinitionId { get; set; }
        public string TaskDefinitionKey { get; set; }
        // more attributes see https://docs.camunda.org/manual/latest/reference/rest/task/get-query/

        public override string ToString() => $"HumanTask [Id={Id}, Name={Name}]";
    }



}
