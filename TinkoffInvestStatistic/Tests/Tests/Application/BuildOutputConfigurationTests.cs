using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Tests.Application;

[TestClass]
public class BuildOutputConfigurationTests
{
    [TestMethod]
    public void DirectoryBuildProps_EnablesCentralizedArtifactsOutput()
    {
        var propsPath = Path.Combine(GetRepositoryRoot(), "Directory.Build.props");

        Assert.IsTrue(File.Exists(propsPath), $"MSBuild configuration was not found: {propsPath}");

        var document = XDocument.Load(propsPath);
        var useArtifactsOutput = document.Root?
            .Elements("PropertyGroup")
            .Elements("UseArtifactsOutput")
            .SingleOrDefault();

        Assert.IsNotNull(useArtifactsOutput);
        Assert.AreEqual("true", useArtifactsOutput.Value.Trim(), ignoreCase: true);
    }

    private static string GetRepositoryRoot([CallerFilePath] string sourceFilePath = "")
    {
        return Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(sourceFilePath)!,
            "..",
            "..",
            "..",
            ".."));
    }
}
