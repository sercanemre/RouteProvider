namespace RouteProvider.API;

public class FindShortestRouteDistanceRequest
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
}
