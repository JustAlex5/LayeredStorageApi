using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Common.Utils
{
    public static class WriteToFile
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
    }
}
