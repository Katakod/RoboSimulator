using Xunit;

namespace RoboSimulator.ConsoleApp.Tests;

public class InputValidatorTests
{    
    [Theory]
    [InlineData("5 5", 5, 5)]
    public void ValidateRoomInput_ValidInput_ShouldReturnDimensions(string input, int expectedWidth, int expectedHeight)
    {
        //Arrange

        //Act
        int actualWidth = 0;
        int actualHeight = 0;

        //Assert
        Assert.Equal(expectedWidth, actualWidth);
        Assert.Equal(expectedHeight, actualHeight);

    }

    [Theory]
    [InlineData("invalid input")]    
    public void ValidateRoomInput_InvalidInput_ShouldReturnNull(string input)
    {
        //Arrange

        //Act
        var actual = input;

        //Assert
        Assert.Null(actual);
    }

    [Theory]
    [InlineData("1 2 N", 1, 2, "N")]    
    public void ValidateRobotInput_ValidInput_ShouldReturnPositionAndDirection(string input, int expectedX, int expectedY, string expectedDirection)
    {
        //Arrange

        //Act
        int actualX = 0;
        int actualY = 0;
        string actualDirection = null;

        //Assert
        Assert.Equal(expectedX, actualX);
        Assert.Equal(expectedY, actualY);
        Assert.Equal(expectedDirection, actualDirection);
    }

    [Theory]
    [InlineData("1 2 InvalidDirection")]
    public void ValidateRobotInput_InvalidInput_ShouldReturnNull(string input)
    {
        //Arrange

        //Act
        var actual = input;

        //Assert
        Assert.Null(actual);
    }

    [Theory]
    [InlineData("FFLR")]
    public void ValidateCommands_ValidInput_ShouldReturnTrue(string input)
    {
        //Arrange

        //Act
        var success = false;

        //Assert
        Assert.True(success);
    }

    [Theory]
    [InlineData("FFLRX")]    
    public void ValidateCommands_InvalidInput_ShouldReturnFalse(string input)
    {
        //Arrange

        //Act

        //Assert
        var success = true;

        //Assert
        Assert.False(success);
    }
}
