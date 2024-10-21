using RoboSimulator.Core.Model;
using Xunit;

namespace RoboSimulator.ConsoleApp.Tests;

public class InputValidatorTests
{    
    [Theory]
    [InlineData("5 5", 5, 5)]
    public void ValidateRoomInput_ValidInput_ShouldReturnCorrectDimensions(string input, int expectedWidth, int expectedHeight)
    {
        //Act
        var (actualDimensions, _) = InputValidator.ValidateRoomDimensionsInput(input);

        //Assert
        Assert.NotNull(actualDimensions);
        Assert.Equal(expectedWidth, actualDimensions.Width);
        Assert.Equal(expectedHeight, actualDimensions.Depth);

    }

    [Theory]
    [InlineData("invalid input")]
    [InlineData("0 0")]
    public void ValidateRoomInput_InvalidInput_ShouldReturnNullWithErrorMessage(string input)
    {
        //Act
        var (actualDimensions, validationMessage) = InputValidator.ValidateRoomDimensionsInput(input);

        //Assert
        Assert.Null(actualDimensions);
        Assert.Contains("invalid", (validationMessage ?? "").ToLower());
    }   

    [Theory]
    [InlineData("1 2 N", 1, 2, Direction.N)]
    [InlineData("0 0 E", 0, 0, Direction.E)]
    public void ValidateRobotInput_ValidInput_ShouldReturnCorrectPositionalDirection(string input, int expectedX, int expectedY, Direction expectedDirection)
    {
        //Arrange
        var testRoom = new Room(5, 5);

        //Act
        var (actualStartPosition, _) = InputValidator.ValidateRobotPositionInput(input, testRoom);

        //Assert
        Assert.NotNull(actualStartPosition);
        Assert.Equal(expectedX, actualStartPosition.X);
        Assert.Equal(expectedY, actualStartPosition.Y);
        Assert.Equal(expectedDirection, actualStartPosition.Direction);
    }

    [Theory]
    [InlineData("1 2 InvalidDirection")]
    [InlineData("o 0 N")]
    [InlineData("-1 0 N")]
    public void ValidateRobotInput_InvalidSartPositionInRoom5x5_ShouldReturnNullWithErrorMessage(string input)
    {
        //Arrange
        var testRoom = new Room(5, 5);

        //Act
        var (actualStartPosition, validationMessage) = InputValidator.ValidateRobotPositionInput(input, testRoom);               

        //Assert
        Assert.Null(actualStartPosition);
        Assert.Contains("invalid", (validationMessage ?? "").ToLower());
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
