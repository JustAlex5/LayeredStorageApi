using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Common.Utils
{
    public static class FileUtils
    {
        public static async Task<bool> SaveDataAsync<T>(T data, string directoryPath, string fileName)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var fullPath = Path.Combine(directoryPath, fileName);

                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(fullPath, json);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

        }

        public static async Task<T?> ReadDataAsync<T>(string directoryPath, string fileName)
        {
            try
            {
                var fullPath = Path.Combine(directoryPath, fileName);

                if (!File.Exists(fullPath))
                    return default;

                var json = await File.ReadAllTextAsync(fullPath);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public static async Task<bool> DeleteFileAsync(string directoryPath, string fileName)
        {
            try
            {
                var fullPath = Path.Combine(directoryPath, fileName);

                if (!File.Exists(fullPath))
                    return false;

                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
