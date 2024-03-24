# RouteProvider

## RouteRepository
Contains AcademyContext which acts as the data source for RouteProvider.API. ın real-world application this layer will be replaced with a DB repository, a connected service etc.

### Academy Context
The AcademyContext class represents the context for managing academies and their routes within the application. It follows the Singleton design pattern to ensure that there is only one instance of the AcademyContext class throughout the application.

## RouteProvider.API
The RouteProvider.API project is responsible for providing an HTTP API interface to interact with the route management functionality. It utilizes the AcademyContext class from the RouteRepository namespace to execute the required operations as per the assignment.
It has only one controller; RouteController.

### RouteController
The RouteController class provides HTTP endpoints for managing routes between academies. It handles various operations such as calculating route distances, finding shortest routes, counting possible routes with specific constraints, and more.

## RouteProvider.Test
The RouteControllerUnitTest class contains unit tests for the RouteController class, covering various scenarios for its endpoints.
The Setup method configures the logging system with console logging, and the TearDown method disposes of the controller after each test execution.