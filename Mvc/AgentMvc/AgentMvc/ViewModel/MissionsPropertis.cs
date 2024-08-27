namespace AgentMvc.ViewModel
{
    public class MissionsPropertis
    {
        public int id {  get; set; }    
        public double TimeLeft { get; set; }
        public double Distance { get; set; }


        public string NickName { get; set; }
        public int AgentX { get; set; }
        public int AgentY { get; set; }

        public string TargetName { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }

    }
}
