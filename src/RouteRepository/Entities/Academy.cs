using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteRepository;

public class Academy
{
	private char _name;

	public char Name
	{
		get { return _name; }
		private set { _name = value; }
	}

	private IEnumerable<Route> _routes;

	public IEnumerable<Route> Routes
	{
		get { return _routes; }
		set { _routes = value; }
	}

    public Academy(char academyNameParam, IEnumerable<Route> routesParam)
    {
        this.Name = academyNameParam;
		this.Routes = routesParam;
    }
}
