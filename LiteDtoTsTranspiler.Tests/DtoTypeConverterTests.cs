using LiteDtoTsTranspiler.Helpers;

namespace LiteDtoTsTranspiler.Test;

public class DtoTypeConverterTests
{
    [Theory]
    [InlineData("System.Int32", "number")]
    [InlineData("System.String", "string")]
    [InlineData("System.DateTime", "Date")]
    [InlineData("System.Double", "number")]
    [InlineData("System.Int64", "bigint")]
    [InlineData("System.Boolean", "boolean")]
    public void ConvertCsTypeToTsType_ShouldReturnCorrectType(string csType, string expectedTsType)
    {
        // Arrange
        var converter = new DtoTypeConverter();

        // Act
        var result = converter.ConvertCsTypeToTsType(csType);

        // Assert
        Assert.Equal(expectedTsType, result);
    }
}