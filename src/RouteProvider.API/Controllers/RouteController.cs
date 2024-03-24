using Microsoft.AspNetCore.Mvc;
using RouteRepository;
using RR = RouteRepository;

namespace RouteProvider.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class RouteController : Controller
{
    private readonly ILogger<RouteController> _logger;

    public RouteController(ILogger<RouteController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Calculates total distance of travel for given route.
    /// </summary>
    /// <param name="academyStops">Academy names in travel order.</param>
    /// <returns>Total distance of route if it exists. If not, returns error message with Status Code 200.</returns>
    [HttpPost]
    public ActionResult<string> CalculateRouteDistance([FromBody] string academyStops)
    {
        // Validate input
        if (string.IsNullOrEmpty(academyStops))
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            // Iterate through given route to find distance of each step and add it to distance variable
            int distance = 0;
            for (int academyName = 0; academyName < academyStops.Length - 1; academyName++)
            {
                // Below code uses Single method which will throw InvalidOperationException if item matches the condition.
                // This behaviour is used to determine if given route exists.
                distance += AcademyContext.Instance.Academies[academyStops[academyName]].Routes.Single(destination => destination.DestinationAcademy == academyStops[academyName + 1]).Distance;
            }
            return Ok(distance.ToString());
        }
        catch (InvalidOperationException ex) // Catch invalid route.
        {
            _logger.LogError(ex.Message);
            return Ok(Constants.NoRouteFoundText);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Finds the shortest path from an academy to another using Dijkstra's algorithm.
    /// Dijkstra's algorithm uses breadth-first traversal to calculate shortest path to each node from a starting node
    /// </summary>
    /// <param name="request">Names of starting and destionation academies.</param>
    /// <returns>Length of shortest path.</returns>
    /// 
    [HttpPost]
    public ActionResult<int> FindShortestRouteDistance([FromBody] FindShortestRouteDistanceRequest request)
    {
        // Validate input
        if (request is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            // Create a dictionary to store shortest path to each academy from the starting academy
            Dictionary<char, int> distances = [];

            // Create a dictionary entry foreach possible destination academy.
            // -1 means no path has been calculated yet.
            AcademyContext.Instance.Academies.ToList().ForEach(academy =>
            {
                distances[academy.Key] = -1;
            });

            // Set 0 distance to it self.
            distances[request.StartingAcademy] = 0;

            // Create a queue to perform breadth-first traversal
            // Queue is useful here because FIFO approach is able handle breadth-first search
            Queue<char> queue = new();

            // Enqueue the starting academy
            queue.Enqueue(request.StartingAcademy);

            while (queue.Count > 0)
            {
                // Dequeue the current academy
                char currentAcademy = queue.Dequeue();

                // If the current academy has no outgoing routes, continue to the next iteration
                if (AcademyContext.Instance.Academies[currentAcademy].Routes is null
                    || !AcademyContext.Instance.Academies[currentAcademy].Routes.Any())
                {
                    continue;
                }

                // Iterate each possible route from current academy
                foreach (RR.Route route in AcademyContext.Instance.Academies[currentAcademy].Routes)
                {
                    // Add new route distance to the distance of current academy from starting academy
                    int newDistance = distances[currentAcademy] + route.Distance;

                    // Check if newly calculated route is first or shorter than previously found shortest route
                    if (distances[route.DestinationAcademy] == -1 || newDistance < distances[route.DestinationAcademy])
                    {
                        distances[route.DestinationAcademy] = newDistance;
                        queue.Enqueue(route.DestinationAcademy);
                    }
                }
            }

            // Return distance of destination academy
            // If distance is -1, that means there are no such routes.
            return Ok(distances[request.DestinationAcademy]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Count possible routes between two academies with step limit, using recursive depth-first search.
    /// </summary>
    /// <param name="request">Starting academy, destination academy and stop limit.</param>
    /// <returns>Count of possible routes.</returns>
    [HttpPost]
    public ActionResult<int> CountPossibleRoutesWithStopLimit([FromBody] CountPossibleRoutesWithStopLimitRequest request)
    {
        // Validate input
        if (request is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            // Call the recursive helper method to perform the depth-first search.
            return Ok(CountPossibleRoutesWithStopLimitRecursively(request.StartingAcademy, request.DestinationAcademy, request.StopLimit, 0));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Recursive function to calculate possible routes between two academies with stop limit.
    /// </summary>
    /// <param name="currentAcademy">Name of starting academy as char.</param>
    /// <param name="destinationAcademy">Name of destination academy as char.</param>
    /// <param name="stopLimit">Max stop for routes.</param>
    /// <param name="stops">Count of stops for current step. Used in recursive functionality.</param>
    /// <returns>Possible routes count.</returns>
    private int CountPossibleRoutesWithStopLimitRecursively(char currentAcademy, char destinationAcademy, int stopLimit, int stops)
    {
        // Check if number of stops in current iteration exceeds the limitfor stops.
        // This means that stop limit is reached without arriving the destination academy, return 0 to indicate that this route is not valid.
        if (stops > stopLimit)
        {
            return 0;
        }

        // Base Case: If the current academy is the ending academy and the current stops count is greater than 0 (0 means that it is the first iteration), 
        // return 1 to indicate that this route is valid.
        if (currentAcademy == destinationAcademy && stops > 0)
        {
            return 1;
        }

        // Get outgoing routes from current academy and iterate through them.
        IEnumerable<RR.Route> outgoingRoutes = AcademyContext.Instance.Academies[currentAcademy].Routes;
        
        // Create a new variable to store count of possible routes
        int count = 0;
        foreach (RR.Route route in outgoingRoutes)
        {
            // Recursive call to get possible routes as destination academy of the current route as the starting point.
            count += CountPossibleRoutesWithStopLimitRecursively(route.DestinationAcademy, destinationAcademy, stopLimit, stops + 1);
        }
        return count;
    }

    /// <summary>
    /// Count possible routes between two academies with exact number of stops, using recursive depth-first search.
    /// </summary>
    /// <param name="request">Starting academy, destination academy and number of exact stop.</param>
    /// <returns>Count of possible routes.</returns>
    [HttpPost]
    public ActionResult<int> CountPossibleRoutesWithExactStops([FromBody] CountPossibleRoutesWithExactStopsRequest request)
    {
        // Validate input
        if (request is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            // Call the recursive helper method to perform the depth-first search.
            return Ok(CountPossibleRoutesWithExactStopsRecursively(request.StartingAcademy, request.DestinationAcademy, request.ExactStops, 0));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Recursive function to calculate possible routes between two academies with exact number of stops.
    /// </summary>
    /// <param name="currentAcademy">Name of starting academy as char.</param>
    /// <param name="destinationAcademy">Name of destination academy as char.</param>
    /// <param name="exactStops">Exact number of stops on route.</param>
    /// <param name="stops">Count of stops for current step. Used in recursive functionality.</param>
    /// <returns>Possible routes count.</returns>
    private int CountPossibleRoutesWithExactStopsRecursively(char currentAcademy, char destinationAcademy, int exactStops, int stops)
    {
        // Base Case: If the current academy is the ending academy and the current step count is equal to exactStops parameter, 
        // return 1 to indicate that this route is valid
        if (currentAcademy == destinationAcademy && stops == exactStops)
        {
            return 1;
        }

        // Check if number of stops in current iteration exceeds expected number of stops.
        // This means that stop limit is reached without arriving the destination academy, return 0 to indicate that this route is not valid.
        if (stops > exactStops)
        {
            return 0;
        }

        // Get outgoing routes from current academy and iterate through them.
        IEnumerable<RR.Route> outgoingRoutes = AcademyContext.Instance.Academies[currentAcademy].Routes;

        // Create a new variable to store count of possible routes
        int count = 0;
        foreach (RR.Route route in outgoingRoutes)
        {
            // Recursive call to get possible routes as destination academy of the current route as the starting point.
            count += CountPossibleRoutesWithExactStopsRecursively(route.DestinationAcademy, destinationAcademy, exactStops, stops + 1);
        }
        return count;
    }

    /// <summary>
    /// Count possible routes between two academies with a total distance limit, using recursive depth-first search.
    /// </summary>
    /// <param name="request">Starting academy, destination academy and distance limit.</param>
    /// <returns>Count of possible routes.</returns>
    [HttpPost]
    public ActionResult<int> FindNumberOfDistanceRoutesWithDistanceLimit([FromBody] FindNumberOfDistanceRoutesWithDistanceLimitRequest request)
    {
        // Validate input
        if (request is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        try
        {
            // Call the recursive helper method to perform the depth-first search.
            return Ok(FindNumberOfDistanceRoutesWithDistanceLimitRecursively(request.StartingAcademy, request.DestinationAcademy, request.DistanceLimit, 0));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentAcademy">Name of starting academy as char.</param>
    /// <param name="destinationAcademy">Name of destination academy as char.</param>
    /// <param name="distanceLimit">Limit of total distance of route.</param>
    /// <param name="currentDistance">Total distance of current iteration.</param>
    /// <returns></returns>
    private int FindNumberOfDistanceRoutesWithDistanceLimitRecursively(char currentAcademy, char destinationAcademy, int distanceLimit, int currentDistance)
    {
        // Create a new variable to store count of possible routes
        int count = 0;

        // Base case: If the current academy is the destination academy and the current distance is between 0 and distance limit parameter,
        // increase count by 1 to indicate that this route is valid.
        if (currentAcademy == destinationAcademy && 0 < currentDistance && currentDistance <= distanceLimit)
        {
            // Requirement indicates that loops in path is valid, which means starting academy can be visited more than once.
            // That's why count is increased by one here rather then returning 1.
            count++;
        }

        // Check if distance in current iteration exceeds distance limit and if there aren't any outgoing routes from current academy.
        // In both cases, current path is invalid and 0 is returned to indicate this.
        if (currentDistance >= distanceLimit || !AcademyContext.Instance.Academies[currentAcademy].Routes.Any())
        {
            return 0;
        }

        // Get outgoing routes from current academy and iterate through them.
        IEnumerable<RR.Route> outgoingRoutes = AcademyContext.Instance.Academies[currentAcademy].Routes;

        // Iterate through all possible routes from the current academy
        foreach (RR.Route route in AcademyContext.Instance.Academies[currentAcademy].Routes)
        {
            // Calculate the new distance by adding the distance of the current route to the current distance
            int newDistance = currentDistance + route.Distance;

            // Recursively call the method with the destination academy of the current route as the new current academy,
            // passing the new distance and incrementing the count by the result
            count += FindNumberOfDistanceRoutesWithDistanceLimitRecursively(route.DestinationAcademy, destinationAcademy, distanceLimit, newDistance);
        }

        return count;
    }
}
