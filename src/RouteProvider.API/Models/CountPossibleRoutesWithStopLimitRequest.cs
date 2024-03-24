namespace RouteProvider.API;

public class CountPossibleRoutesWithStopLimitRequest
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

    private int _stopLimit;

    public int StopLimit
    {
        get { return _stopLimit; }
        set { _stopLimit = value; }
    }
}
