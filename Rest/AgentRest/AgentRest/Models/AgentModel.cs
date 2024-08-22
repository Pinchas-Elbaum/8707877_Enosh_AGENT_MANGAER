namespace AgentRest.Models
{
    public enum AgentStatus
    {
        Dormant,
        Activity,
    }

    public class AgentModel
    {
       
        public int Id { get; set; } 
        public string NickName { get; set; }
        public string Image { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public AgentStatus Status { get; set; }
    }
    
}
