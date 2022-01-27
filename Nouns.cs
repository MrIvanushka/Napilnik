class Player { }
class Gun { }
class TrackingTarget { }
class UnitPool
{
    public IReadOnlyCollection<Unit> AvailableUnits { get; private set; }
}