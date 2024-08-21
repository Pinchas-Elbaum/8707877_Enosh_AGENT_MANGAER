namespace AgentRest.Models
{
    public enum TargetStatus
    {
        live,
        Eliminated,
    }

    public class TargetModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Image { get; set; }
        public string position { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public TargetStatus status { get; set; }
    }
}
