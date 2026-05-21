namespace DACS.Models
{
    public class RoadmapViewModel
    {
        public int ProgressPercentage { get; set; }
        public List<SkillScore> Skills { get; set; } = new();
        public string AiAdvice { get; set; } = string.Empty;
        public int TotalQuestionsDone { get; set; }
    }

    public class SkillScore
    {
        public string SkillName { get; set; } = string.Empty;
        public double Score { get; set; }
    }
}
