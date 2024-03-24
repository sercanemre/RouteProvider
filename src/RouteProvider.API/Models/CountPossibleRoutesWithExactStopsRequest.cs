namespace RouteProvider.API;

public class CountPossibleRoutesWithExactStopsRequest
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

    private int _exactStops;

    public int ExactStops
    {
        get { return _exactStops; }
        set { _exactStops = value; }
    }
}
