namespace ToDoList.Test;

public class CalculatorTest
{
    [Fact]
    public void Calculator_Add_ShouldReturnCorrectResult()
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        var result = calculator.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }
}

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}
