using System;

namespace AgentRest.Models
{
    public enum MissionStatus
    {
        Suggestion,
        LinkedToTask,
        Done

    }
    public class MissionModel
    {
        public int Id {  get; set; }    
        public int AgentId { get; set; }    
        public int TargetId { get; set; }
        public AgentModel Agent { get; set; }
        public TargetModel Target { get; set; }
        public double TimeLeft {  get; set; }  
        public DateTime ActualExecutionTime {  get; set; }  
        public MissionStatus Status { get; set; }   

    }
}
