namespace Shared.Models
{
    public record BuildingUpgradeCost
    {
        public int Wood { get; init; }
        public int Stone { get; init; }
        public int Silver { get; init; }
        public int Population { get; init; }
    }
}
