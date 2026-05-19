using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using UnityEditor.Android;

/// <summary>
/// Workaround for Meta XR SDK namespace collision with Android Gradle Plugin 8.x+.
/// InteractionSdk.aar, SDKTelemetry.aar, and OVRPlugin.aar all declare
/// package="com.oculus.Integration" in their AndroidManifest.xml.
/// AGP requires unique namespaces per library, so this script patches each AAR
/// to use a distinct package name before Gradle runs.
/// </summary>
public class FixAndroidNamespaceConflict : IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 1;

    public void OnPostGenerateGradleAndroidProject(string unityLibraryPath)
    {
        string libsPath = Path.Combine(unityLibraryPath, "libs");

        PatchAarNamespace(libsPath, "InteractionSdk.aar", "com.oculus.interaction.sdk.fix");
        PatchAarNamespace(libsPath, "SDKTelemetry.aar",   "com.oculus.sdk.telemetry.fix");
        PatchAarNamespace(libsPath, "OVRPlugin.aar",      "com.oculus.ovrplugin.fix");
    }

    private static void PatchAarNamespace(string libsPath, string aarFileName, string newPackage)
    {
        string aarPath = Path.Combine(libsPath, aarFileName);
        if (!File.Exists(aarPath))
            return;

        string tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);

        try
        {
            ZipFile.ExtractToDirectory(aarPath, tempDir);

            string manifestPath = Path.Combine(tempDir, "AndroidManifest.xml");
            if (File.Exists(manifestPath))
            {
                string content = File.ReadAllText(manifestPath);
                content = Regex.Replace(content, @"package=""[^""]*""", $"package=\"{newPackage}\"");
                File.WriteAllText(manifestPath, content);
            }

            File.Delete(aarPath);

            using (var zip = ZipFile.Open(aarPath, ZipArchiveMode.Create))
            {
                foreach (string filePath in Directory.GetFiles(tempDir, "*", SearchOption.AllDirectories))
                {
                    string entryName = filePath.Substring(tempDir.Length + 1).Replace('\\', '/');
                    zip.CreateEntryFromFile(filePath, entryName);
                }
            }
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }
}
