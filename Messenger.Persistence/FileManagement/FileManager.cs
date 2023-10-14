namespace MessengerX.Persistence.Extensions;

public static class FileManagerExtension
{
    // private const string path = "../MessengerX.Persistence/Uploads";

    public static async Task<string?> WriteToFileAsync(this byte[]? file, string path,  string fileName)
    {
        string fullName = $"{Guid.NewGuid()}.{fileName}.{DateTime.Now:ddMMyyyy.HHmm}";
        string fullPath = $"{path}/{fullName}";

        if (file == null || file.Length <= 0)
            return null;

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
            
        await File.WriteAllBytesAsync(fullPath, file);

        return fullPath;
    }

    public static async Task<byte[]?> ReadToBytesAsync(this string? path)
    {
        return path != null ? await File.ReadAllBytesAsync(path) : null;
    }

    public static byte[]? ReadToBytes(this string? path)
    {
        return path != null ? File.ReadAllBytes(path) : null;
    }
}