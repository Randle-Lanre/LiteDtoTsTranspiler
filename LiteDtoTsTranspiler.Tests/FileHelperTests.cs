using LiteDtoTsTranspiler.Helpers;

namespace LiteDtoTsTranspiler.Test;

public class FileHelperTests
{
    [Fact]
    public void FilePathHelper_ShouldCreateFile()
    {
        // Arrange
        var dtoName = "TestDto";

        // Act
        var (created, filePath) = FileHelper.FilePathHelper(dtoName, "");

        // Assert
        Assert.True(created);
        Assert.True(File.Exists(filePath));

        // Cleanup
        File.Delete(filePath);
    }
}