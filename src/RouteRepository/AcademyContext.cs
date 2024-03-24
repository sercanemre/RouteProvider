namespace RouteRepository;

public class AcademyContext
{
    #region Singleton
    private static readonly object _padLock = new object();

    private static AcademyContext _instance;

    public static AcademyContext Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_padLock)
                {
                    if (_instance is null)
                    {
                        _instance = new AcademyContext();
                    }
                }
            }
            return _instance;
        }
    }
    #endregion

    public Dictionary<char, Academy> Academies
    { get; private set; }

    private AcademyContext()
    {
        this.Academies = new();
        Academies.Add('A',
            new Academy('A', new List<Route> {
                new Route('A', 'B', 5),
                new Route('A', 'D', 5),
                new Route('A', 'E', 7)
            }));
        Academies.Add('B',
            new Academy('B', new List<Route> {
                new Route('B', 'C', 4)
            }));
        Academies.Add('C',
            new Academy('C', new List<Route> {
                new Route('C', 'D', 8),
                new Route('C', 'E', 2)
            }));
        Academies.Add('D',
            new Academy('D', new List<Route> {
                new Route('D', 'C', 8),
                new Route('D', 'E', 6)
            }));
        Academies.Add('E',
            new Academy('E', new List<Route> {
                new Route('E', 'B', 3)
            }));
    }
}
