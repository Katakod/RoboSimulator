using Xunit;

namespace RoboSimulator.Tests;

public class SimulationTests
{

/* Below is a 5 x 5 Test Room visualization of x,y points with a middle of 3,3. 
* A complex route trace for moving from middle to north boundary, 
* then to each corner and heading back south to the middle: FF RFF RFFFF RFFFF RFFFF RFF RFF 
         
                          N                   
           ------------------------------- 
           | 1,5 | 2,5 | 3,5 | 4,5 | 5,5 |    
           | 1,4 | 2,4 | 3,4 | 4,4 | 5,4 |    
         W | 1,3 | 2,3 | 3,3 | 4,3 | 5,3 | E  
           | 1,2 | 2,2 | 3,2 | 4,2 | 5,2 |    
           | 1,1 | 2,1 | 3,1 | 4,1 | 5,1 |    
           -------------------------------
                          S        		          
        
*/


    [Theory]
    [InlineData("N", "L", "W")] // Turn left from North should result in West
    [InlineData("W", "L", "S")] // Turn left from West should result in South
    [InlineData("S", "L", "E")] // Turn left from South should result in East
    [InlineData("E", "L", "N")] // Turn left from East should result in North
    [InlineData("N", "R", "E")] // Turn right from North should result in East
    [InlineData("E", "R", "S")] // Turn right from East should result in South
    [InlineData("S", "R", "W")] // Turn right from South should result in West
    [InlineData("W", "R", "N")] // Turn right from West should result in North
    public void RobotTurn_ShouldChangeDirectionCorrectly(string initialDirection, string turnCommand, string expectedDirection)
    {
        //Arrange

        //Act
        var actualDirection = initialDirection;

        //Assert
        Assert.Equal(expectedDirection, actualDirection);
    }

    [Theory]
    [InlineData("N", "F", 3, 4)] // Move forward facing North from (3,3) to (3,4)
    [InlineData("E", "F", 4, 3)] // Move forward facing East from (3,3) to (4,3)
    [InlineData("S", "F", 3, 2)] // Move forward facing South from (3,3) to (3,2)
    [InlineData("W", "F", 2, 3)] // Move forward facing West from (3,3) to (2,3)
    public void RobotMove_WithinRoom_ShouldUpdatePosition(string initialDirection, string commands, int expectedX, int expectedY)
    {
        //Arrange

        //Act
        int actualX = expectedX;
        int actualY = expectedY;
        bool success = false;

        // Assert
        Assert.True(success);
        Assert.Equal(expectedX, actualX);
        Assert.Equal(expectedY, actualY);
    }


    [Theory]
    [InlineData("N", "FFF", 3, 5)] // Out of Bounds - moving North after reaching (3,5) and trying one more move
    [InlineData("E", "FFF", 5, 3)] // Out of Bounds - moving East after reaching (5,3) and trying one more move
    [InlineData("S", "FFF", 3, 1)] // Out of Bounds - moving South after reaching (3,1) and trying one more move
    [InlineData("W", "FFF", 1, 3)] // Out of Bounds - moving West after reaching (1,3) and trying one more move
    public void RobotMove_OutBoundsOfRoom_ShouldFail(string initialDirection, string commands, int expectedX, int expectedY)
    {
        //Arrange

        //Act
        int actualX = -1; //TODO: Room should be limit, making x,y stop at crossing point?
        int actualY = -1;
        bool success = false;

        //Assert
        Assert.True(success);
        Assert.Equal(expectedX, actualX);
        Assert.Equal(expectedY, actualY);
    }

}
