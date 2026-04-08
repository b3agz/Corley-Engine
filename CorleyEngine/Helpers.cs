
namespace CorleyEngine;

public class Helpers {

    public static string GetEnginePath() {

        string targetRelativePath = @"CorleyEngine\";

        var drive = DriveInfo.GetDrives()
            .Where(d => d.IsReady)
            .Select(d => Path.Combine(d.RootDirectory.FullName, targetRelativePath))
            .FirstOrDefault(Directory.Exists);

        return drive;

    }
}