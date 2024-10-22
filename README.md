# RoboSimulator

## About
This solution was developed using Visual Studio 2022 and C# .NET 8 to implement the basic User Story below.  
Open RobotSimulator.sln in Visual Studio and run debug with RobotSimulator.ConsoleApp as start-up project to test it.  
Open Test Explorer to run related unit tests.  

#### User Story
As a **user running the Robot simulation**  
I want to **provide the simulator with command input**  
And get **simulation results based on said command input**  
So that **I know if a route is successful or not**.  


## Solution
The solution consist of 2 main projects. **ConsoleApp** for handling input and **Core** implementing the "business" logic. 
This makes it more scalable and let us separate concerns, as e.g. future I/O may change. 
For the same reason there are 2 separate test projects related to the respective main projects.  

Some purpose/readability separation through folders exist inside Core project, but was skipped in ConsoleApp for now.  

**Robot** class focuses on the robot’s state (position/direction) and actions (moving/turning)
Through **IRobot** interface we are prepared for a future case introducing e.g. AdvancedRobot or BadRobot.  

Since the robot move in a room, we have the **Room** class that mainly focus on boundaries. 
It's using a Dimension class to hold the width and depth that define a shape state. 
With **IRoom** interface we offer other implementations in the future.
For now Robot assumes origin being in North East corner with a zero-based position 0,0 (for x,y), but it's possible there could be other origins defined by Room class in the future.

**Dimension** and **PositionalDirection** classes work as state holders used by Room and Robot.   

**CommandService** class (and **ICommandService** interface) separate the command processing from the Robot.
This makes it more flexible in case we want to change/add processing, as if we might have other things than robot commands.
As an example, we may want to expand solution to include a boat simulator that  reuse the same command interpretation, but with other behaviour and contract.
Perhaps we also want to offer the input in JSON format, making CommandService a good choice without changing the Robot.    

Althought this currently being a small solution, intention was to prepare it for future growth and scalability/maintainability.
If there is no expectations on growing, other choices could be better, like focusing more on logic encapsulation.  

**ILogger** is prepared to be injected and handle logging in different ways, currently through **NLog**.  
**IExceptionHandler** is also injected to centralize handling even if it's existing implmentation of **SimpleExceptionHandler** is not offering much at the moment.  

**ConsoleApp** control the console input flow in **Program** and has it's own **InputValidator** class for making sure console input is correct before handing over to **Core**.

Regarding the unit tests, more could be added,  like separate tests for Robot and Room creation. It's also a good idea to refine some of the existing ones. Most likely, they're not all needed, if we try to find and remove "duplicate cases". 

