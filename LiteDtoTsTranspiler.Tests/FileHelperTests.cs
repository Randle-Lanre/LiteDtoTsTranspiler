using LiteDtoTsTranspiler.Helpers;

namespace LiteDtoTsTranspiler.Test;

public class FileHelperTests
{
    [Theory]
    [InlineData("TestDto", true)]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("Invalid*Name", false)]
    public void FilePathHelper_ShouldCreateFile(string dtoName, bool expectedCreated)
    {
        // Arrange
        var outputLocation = Path.GetTempPath();
        var filePath = Path.Combine(outputLocation, $"{dtoName}.ts");

        // Act
        var (created, actualFilePath) = FileHelper.FilePathHelper(dtoName, outputLocation);
        // actualFilePath = actualFilePath.Replace(@"\\", @"\");

        // Assert
        Assert.Equal(expectedCreated, created);
        if (created)
        {
            using (var file = File.Open(filePath, FileMode.Open))
            {
                Assert.True(File.Exists(filePath));
                Assert.Equal(filePath, actualFilePath);
            }
        }
        else
        {
            Assert.False(File.Exists(filePath));
            Assert.True(string.IsNullOrWhiteSpace(actualFilePath));
        }
    }
}