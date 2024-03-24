using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RouteProvider.API;
using RouteProvider.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RouteProvider.Test;

public class RouteControllerUnitTest
{
    private RouteController _routeController;

    [SetUp]
    public void Setup()
    {
        // Configure logging to only include console logging
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        var logger = loggerFactory.CreateLogger<RouteController>();

        _routeController = new RouteController(logger);
    }

    [TearDown]
    public void TearDown()
    {
        _routeController.Dispose();
    }

    #region CalculateRouteDistance
    [Test]
    public void Test_CalculateRouteDistance_InvalidInput()
    {
        // Create the request
        string academyStops = string.Empty;

        // Execute method
        ActionResult<string> result = _routeController.CalculateRouteDistance(academyStops);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<string>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as StatusCodeResult)?.StatusCode, Is.EqualTo(400));
        });
    }

    [Test]
    public void Test_CalculateRouteDistance_ABC()
    {
        // Create the request
        string academyStops = "ABC";

        // Create expected result
        string expectedResult = "9";

        // Execute method
        ActionResult<string> result = _routeController.CalculateRouteDistance(academyStops);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<string>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }

    [Test]
    public void Test_CalculateRouteDistance_AEBCD()
    {
        // Create the request
        string academyStops = "AEBCD";

        // Create expected result
        string expectedResult = "22";

        // Execute method
        ActionResult<string> result = _routeController.CalculateRouteDistance(academyStops);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<string>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }

    [Test]
    public void Test_CalculateRouteDistance_AED()
    {
        // Create the request
        string academyStops = "AED";

        // Create expected result
        string expectedResult = Constants.NoRouteFoundText;

        // Execute method
        ActionResult<string> result = _routeController.CalculateRouteDistance(academyStops);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<string>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }
    #endregion

    #region CountPossibleRoutesWithStopLimit
    [Test]
    public void Test_CountPossibleRoutesWithStopLimit_InvalidInput()
    {
        // Execute method
        ActionResult<int> result = _routeController.CountPossibleRoutesWithStopLimit(null);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as StatusCodeResult)?.StatusCode, Is.EqualTo(400));
        });
    }

    [Test]
    public void Test_CountPossibleRoutesWithStopLimit_CtoC()
    {
        // Create the request
        var request = new CountPossibleRoutesWithStopLimitRequest
        {
            StartingAcademy = 'C',
            DestinationAcademy = 'C',
            StopLimit = 3
        };

        // Create expected result
        int expectedResult = 2;

        // Execute method
        ActionResult<int> result = _routeController.CountPossibleRoutesWithStopLimit(request);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }
    #endregion

    #region CountPossibleRoutesWithExactStops
    [Test]
    public void Test_CountPossibleRoutesWithExactStops_InvalidInput()
    {
        // Execute method
        ActionResult<int> result = _routeController.CountPossibleRoutesWithExactStops(null);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as StatusCodeResult)?.StatusCode, Is.EqualTo(400));
        });
    }

    [Test]
    public void Test_CountPossibleRoutesWithExactStops_AtoC()
    {
        // Create the request
        var request = new CountPossibleRoutesWithExactStopsRequest
        {
            StartingAcademy = 'A',
            DestinationAcademy = 'C',
            ExactStops = 4
        };

        // Create expected result
        int expectedResult = 3;

        // Execute method
        ActionResult<int> result = _routeController.CountPossibleRoutesWithExactStops(request);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }
    #endregion

    #region FindShortestRouteDistance
    [Test]
    public void Test_FindShortestRouteDistance_InvalidInput()
    {
        // Execute method
        ActionResult<int> result = _routeController.FindShortestRouteDistance(null);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as StatusCodeResult)?.StatusCode, Is.EqualTo(400));
        });
    }

    [Test]
    public void Test_FindShortestRouteDistance_AtoC()
    {
        // Create the request
        var request = new FindShortestRouteDistanceRequest
        {
            StartingAcademy = 'A',
            DestinationAcademy = 'C'
        };

        // Create expected result
        int expectedResult = 9;

        // Execute method
        ActionResult<int> result = _routeController.FindShortestRouteDistance(request);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }

    [Test]
    public void Test_FindShortestRouteDistance_BtoD()
    {
        // Create the request
        var request = new FindShortestRouteDistanceRequest
        {
            StartingAcademy = 'B',
            DestinationAcademy = 'D'
        };

        // Create expected result
        int expectedResult = 12;

        // Execute method
        ActionResult<int> result = _routeController.FindShortestRouteDistance(request);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }

    [Test]
    public void Test_FindShortestRouteDistance_DtoB()
    {
        // Create the request
        var request = new FindShortestRouteDistanceRequest
        {
            StartingAcademy = 'D',
            DestinationAcademy = 'B'
        };

        // Create expected result
        int expectedResult = 9;

        // Execute method
        ActionResult<int> result = _routeController.FindShortestRouteDistance(request);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }
    #endregion

    #region FindNumberOfDistanceRoutesWithDistanceLimit
    [Test]
    public void Test_FindNumberOfDistanceRoutesWithDistanceLimit_InvalidInput()
    {
        // Execute method
        ActionResult<int> result = _routeController.FindNumberOfDistanceRoutesWithDistanceLimit(null);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as StatusCodeResult)?.StatusCode, Is.EqualTo(400));
        });
    }

    [Test]
    public void Test_FindNumberOfDistanceRoutesWithDistanceLimit_CtoC()
    {
        // Create the request
        var request = new FindNumberOfDistanceRoutesWithDistanceLimitRequest
        {
            StartingAcademy = 'C',
            DestinationAcademy = 'C',
            DistanceLimit = 30
        };

        // Create expected result
        int expectedResult = 7;

        // Execute method
        ActionResult<int> result = _routeController.FindNumberOfDistanceRoutesWithDistanceLimit(request);

        // Assertions: result type, result status code, result value
        Assert.IsInstanceOf<ActionResult<int>>(result);
        Assert.Multiple(() =>
        {
            Assert.That((result?.Result as ObjectResult)?.StatusCode, Is.EqualTo(200));
            Assert.That((result?.Result as ObjectResult)?.Value, Is.EqualTo(expectedResult));
        });
    }
    #endregion
}
