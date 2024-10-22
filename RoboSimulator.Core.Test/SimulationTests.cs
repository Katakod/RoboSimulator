using Microsoft.Extensions.Logging.Abstractions;
using RoboSimulator.Core.Infrastructure;
using RoboSimulator.Core.Model;
using RoboSimulator.Core.Services;
using Xunit;

namespace RoboSimulator.Tests;

public class SimulationTests
{
    private const Direction NORTH = Direction.N;
    private const Direction EAST = Direction.E;
    private const Direction SOUTH = Direction.S;
    private const Direction WEST = Direction.W;

    private static Robot CreateRobot(Room room, PositionalDirection positionalDirection)
    {
        var robot = new Robot(positionalDirection, room);

        return robot;
    }

    private static CommandService CreateCommandServiceForTests()
    {
        var nullLogger = NullLogger<CommandService>.Instance;
        var exceptionHandler = new SimpleExceptionHandler(NullLogger<SimpleExceptionHandler>.Instance);

        return new CommandService(nullLogger, exceptionHandler);
    }

    private static Robot CreateRobotInMiddleOf5x5RoomWithDirection(Direction initialDirection)
    {
        /* Below is a 5 x 5 Test Room visualization of x,y points with a middle of 2,2. 
           A complex route trace for moving from middle to north boundary, then to each corner 
           and heading back south to the middle would be: FF RFF RFFFF RFFFF RFFFF RFF RFF 

                            N                   
             ------------------------------- 
             | 0,0 | 1,0 | 2,0 | 3,0 | 4,0 |    
             | 0,1 | 1,1 | 2,1 | 3,1 | 4,1 |    
           W | 0,2 | 1,2 | 2,2 | 3,2 | 4,2 | E  
             | 0,3 | 1,3 | 2,3 | 3,3 | 4,3 |    
             | 0,4 | 1,4 | 2,4 | 3,4 | 4,4 |    
             -------------------------------
                            S        		          

        */

        var room = new Room(5, 5); // The middle of the 5x5 Room with corner of 0,0 is 2,2.
        var positionalDirection = new PositionalDirection(2, 2, initialDirection);
        var robot = new Robot(positionalDirection, room);

        return robot;
    }
    private static void AssertProcessCommandsMoveResult(int expectedX, int expectedY, Direction expectedDirection, PositionalDirection actualPositionalDirection)
    {
        Assert.Equal(expectedX, actualPositionalDirection.X);
        Assert.Equal(expectedY, actualPositionalDirection.Y);
        Assert.Equal(expectedDirection, actualPositionalDirection.Direction);
    }

    

    [Fact]
    public void RobotMove_AssignmentCase1_ShouldSucceed_WithCorrectPositionalDirection()
    {
        //5 5
        //1 2 N
        //RF RFF RF RF
        //Report: 1 3 N;

        //Arrange        
        var service = CreateCommandServiceForTests();
        var robot = CreateRobot(new Room(5, 5), new PositionalDirection(1, 2, NORTH));      

        //Act
        var result = service.ProcessCommands(robot,"RF RFF RF RF");

        //Assert        
        Assert.True(result.Success);
        AssertProcessCommandsMoveResult(1, 3, NORTH, result.UpdatedPositionalDirection!);
    }

    [Fact]
    public void RobotMove_AssignmentCase2_ShouldSucceed_WithCorrectPositionalDirection()
    {
        //5 5
        //0 0 E
        //RF LFF LRF
        //Report: 3 1 E

        //Arrange        
        var service = CreateCommandServiceForTests();
        var robot = CreateRobot(new Room(5, 5), new PositionalDirection(0, 0, EAST));

        //Act
        var result = service.ProcessCommands(robot, "RF LFF LRF");        

        //Assert
        Assert.True(result.Success);
        AssertProcessCommandsMoveResult(3, 1, EAST, result.UpdatedPositionalDirection!);
    }

    [Fact]
    public void RobotMove_AssignmentCase3_ShouldFail_WithCorrectPositionalDirection()
    {
        //3 3
        //2 2 N
        //FFLFFRF
        //ERROR: Out of bounds at 0 - 1

        //Arrange        
        var service = CreateCommandServiceForTests();
        var robot = CreateRobot(new Room(3, 3), new PositionalDirection(2, 2, NORTH));

        //Act
        var result = service.ProcessCommands(robot, "FF LFF RF");        

        //Assert        
        Assert.False(result.Success);
        AssertProcessCommandsMoveResult(0, -1, NORTH, result.UpdatedPositionalDirection!);
    }

    [Theory]
    [InlineData(NORTH, "L", WEST)]  // Turn left from North should result in West
    [InlineData(WEST,  "L", SOUTH)] // Turn left from West should result in South
    [InlineData(SOUTH, "L", EAST)]  // Turn left from South should result in East
    [InlineData(EAST,  "L", NORTH)] // Turn left from East should result in North
    [InlineData(NORTH, "R", EAST)]  // Turn right from North should result in East
    [InlineData(EAST,  "R", SOUTH)] // Turn right from East should result in South
    [InlineData(SOUTH, "R", WEST)]  // Turn right from South should result in West
    [InlineData(WEST,  "R", NORTH)] // Turn right from West should result in North
    public void RobotTurn_ShouldSucceed_WithCorrectDirection(
        Direction initialDirection, string turnCommand, Direction expectedDirection)
    {
        //TODO: Dev test that we may wanna remove, other test cases will probably cover this

        //Arrange
        var service = CreateCommandServiceForTests();
        var robot = CreateRobotInMiddleOf5x5RoomWithDirection(initialDirection);

        //Act
        var result = service.ProcessCommands(robot,turnCommand);

        //Assert
        Assert.True(result.Success);
        Assert.Equal(expectedDirection, result.UpdatedPositionalDirection!.Direction);        
    }

    [Theory]
    [InlineData(NORTH, "F", 2, 1, NORTH)] // Move forward facing North from middle (2,2) to (2,1)
    [InlineData(EAST,  "F", 3, 2, EAST)]  // Move forward facing East from middle (2,2) to (3,2)
    [InlineData(SOUTH, "F", 2, 3, SOUTH)] // Move forward facing South from middle (2,2) to (2,3)
    [InlineData(WEST,  "F", 1, 2, WEST)]  // Move forward facing West from  middle (2,2) to (1,2)
    public void RobotMove_WithinRoom_ShouldSucceed_WithCorrectPositionalDirection(
        Direction initialDirection, string commands, int expectedX, int expectedY, Direction expectedDirection)
    {
        //Arrange
        var service = CreateCommandServiceForTests();
        var robot = CreateRobotInMiddleOf5x5RoomWithDirection(initialDirection);

        //Act
        var result = service.ProcessCommands(robot,commands);

        // Assert
        Assert.True(result.Success);
        AssertProcessCommandsMoveResult(expectedX, expectedY, expectedDirection, result.UpdatedPositionalDirection!);
    }

    [Theory]
    [InlineData(NORTH, "FFF", 2, -1, NORTH)] // Out of Bounds - moving North after reaching (2,0) and trying one more move
    [InlineData(EAST,  "FFF", 5, 2, EAST)]   // Out of Bounds - moving East after reaching (4,2) and trying one more move
    [InlineData(SOUTH, "FFF", 2, 5, SOUTH)]  // Out of Bounds - moving South after reaching (2,4) and trying one more move
    [InlineData(WEST,  "FFF", -1, 2, WEST)]  // Out of Bounds - moving West after reaching (0,2) and trying one more move
    public void RobotMove_OutOfBounds_ShouldFail_WithCorrectPositionalDirection(
        Direction initialDirection, string commands, int expectedX, int expectedY, Direction expectedDirection)
    {
        //Arrange
        var service = CreateCommandServiceForTests();
        var robot = CreateRobotInMiddleOf5x5RoomWithDirection(initialDirection);

        //Act        
        var result = service.ProcessCommands(robot, commands);

        //Assert
        Assert.False(result.Success);
        AssertProcessCommandsMoveResult(expectedX, expectedY, expectedDirection, result.UpdatedPositionalDirection!);
    }
}
