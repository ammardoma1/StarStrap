using System;
using System.IO;
using System.Text.Json;
using StarStrap;

public static class StarStrapRobloxSettingsManager // lowk didnt know what tf to name this file
{
    public class StarStrapRobloxSettings
    {
        public int MemoryCleanerIntervalSeconds { get; set; }
    }

    private static readonly string FolderPath = Paths.Base;

    private static readonly string FilePath =
        Path.Combine(FolderPath, "StarStrapRobloxSaves.json");

    public static StarStrapRobloxSettings Load()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new StarStrapRobloxSettings();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<StarStrapRobloxSettings>(json)
                   ?? new StarStrapRobloxSettings();
        }
        catch
        {
            return new StarStrapRobloxSettings();
        }
    }

    public static void Save(StarStrapRobloxSettings settings)
    {
        try
        {
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(FilePath, json);
        }
        catch
        {
        }
    }
}
