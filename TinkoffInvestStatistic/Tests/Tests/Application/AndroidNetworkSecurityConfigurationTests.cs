using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Tests.Application;

[TestClass]
public class AndroidNetworkSecurityConfigurationTests
{
    private const string AndroidNamespace = "http://schemas.android.com/apk/res/android";

    [TestMethod]
    public void Manifest_UsesNetworkSecurityConfiguration()
    {
        var manifest = XDocument.Load(GetAndroidFile("AndroidManifest.xml"));
        var application = manifest.Root?.Element("application");

        Assert.IsNotNull(application);
        Assert.AreEqual(
            "@xml/network_security_config",
            application.Attribute(XName.Get("networkSecurityConfig", AndroidNamespace))?.Value);
    }

    [TestMethod]
    public void NetworkSecurityConfiguration_TrustsSystemAndRussianCertificatesForTBankApi()
    {
        var path = GetAndroidFile("Resources", "xml", "network_security_config.xml");
        Assert.IsTrue(File.Exists(path), $"Android network security config was not found: {path}");

        var config = XDocument.Load(path);
        var domainConfig = config.Root?
            .Elements("domain-config")
            .SingleOrDefault(element => element
                .Elements("domain")
                .Any(domain => domain.Value == "invest-public-api.tbank.ru"));

        Assert.IsNotNull(domainConfig);

        var certificateSources = domainConfig
            .Element("trust-anchors")?
            .Elements("certificates")
            .Select(element => element.Attribute("src")?.Value)
            .ToArray();

        CollectionAssert.AreEquivalent(
            new[]
            {
                "system",
                "@raw/russian_trusted_root_ca",
                "@raw/russian_trusted_sub_ca",
            },
            certificateSources);
    }

    [TestMethod]
    [DataRow("russian_trusted_root_ca.cer", "8FF915CCAB7BC16F8C5C8099D53E0E115B3AEC2F")]
    [DataRow("russian_trusted_sub_ca.cer", "335D43F53451B781535FF3882DF713D3C14F8A01")]
    public void BundledRussianCertificate_HasExpectedThumbprint(string fileName, string expectedThumbprint)
    {
        var path = GetAndroidFile("Resources", "raw", fileName);
        Assert.IsTrue(File.Exists(path), $"Bundled certificate was not found: {path}");

        var certificate = X509CertificateLoader.LoadCertificateFromFile(path);

        Assert.AreEqual(expectedThumbprint, certificate.Thumbprint);
    }

    private static string GetAndroidFile(params string[] relativePath)
    {
        return Path.Combine(GetRepositoryRoot(),
            "TinkoffInvestStatistic",
            "TinkoffInvestStatistic",
            "Platforms",
            "Android",
            Path.Combine(relativePath));
    }

    private static string GetRepositoryRoot([CallerFilePath] string sourceFilePath = "")
    {
        return Path.GetFullPath(Path.Combine(
            Path.GetDirectoryName(sourceFilePath)!,
            "..",
            "..",
            ".."));
    }
}
