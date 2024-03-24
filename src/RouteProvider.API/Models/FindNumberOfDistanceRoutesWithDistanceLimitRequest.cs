namespace RouteProvider.API;

public class FindNumberOfDistanceRoutesWithDistanceLimitRequest
{
    private char _startingAcademy;

    public char StartingAcademy
    {
        get { return _startingAcademy; }
        set { _startingAcademy = value; }
    }

    private char _destinationAcademy;

    public char DestinationAcademy
    {
        get { return _destinationAcademy; }
        set { _destinationAcademy = value; }
    }

    private int _distanceLimit;

    public int DistanceLimit
    {
        get { return _distanceLimit; }
        set { _distanceLimit = value; }
    }

}
