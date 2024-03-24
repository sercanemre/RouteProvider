using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteRepository;

public class Route
{
	private readonly char _startingAcademy;

	public char StartingAcademy
	{
		get { return _startingAcademy; }
	}

	private readonly char _destinationAcademy;

	public char DestinationAcademy
	{
		get { return _destinationAcademy; }
	}

	private readonly int _distance;

	public int Distance
	{
		get { return _distance; }
	}

    public Route(char startingLocationParam, char endLocationParam, int distanceParam)
    {
        this._startingAcademy = startingLocationParam;
		this._destinationAcademy = endLocationParam;
		this._distance = distanceParam;
    }
}
