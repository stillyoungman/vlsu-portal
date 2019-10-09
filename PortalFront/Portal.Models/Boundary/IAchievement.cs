namespace Portal.Models
{
    public interface IAchievement
    {
        long AchievementId { get; }
        long OwnerId { get; }
        long PlanEntryId { get; }
        int Score { get; }
        string Title { get; }
    }
}