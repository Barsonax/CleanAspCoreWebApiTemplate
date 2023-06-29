using System.Text;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;

namespace CleanAspCore.Api.Tests.Helpers;

public static class FileProviderMockExtensions
{
    public static Mock<IFileProvider> SetupJsonFileMock(this Mock<IFileProvider> mock, string path, object obj)
    {
        mock.SetupFileMock(path, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj)));
        return mock;
    }
    
    public static Mock<IFileProvider> SetupFileMock(this Mock<IFileProvider> mock, string path, byte[] content)
    {
        var fileInfoMock = new Mock<IFileInfo>();
        fileInfoMock.Setup(x => x.CreateReadStream()).Returns(new MemoryStream(content));

        mock.Setup(x => x.GetFileInfo(path)).Returns(fileInfoMock.Object);
        return mock;
    }
}