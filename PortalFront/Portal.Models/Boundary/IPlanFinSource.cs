namespace Portal.Models
{
    public interface IPlanFinSource
    {
        FinancingType FinSource { get; }
        int NumPlaces { get; }
    }
}