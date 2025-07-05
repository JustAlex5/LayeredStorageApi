
namespace LayeredStorageApi.BackgroundServices
{
    public class FileCleanupService : BackgroundService
    {
        private readonly ILogger<FileCleanupService> _logger;
        private readonly string _directoryPath;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _fileRetention = TimeSpan.FromMinutes(30);

        public FileCleanupService(ILogger<FileCleanupService> logger, IConfiguration config)
        {
            _logger = logger;
            _directoryPath = config["FileService:BasePath"] ?? "Data";

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("[{Action}] File cleanup service started at UTC time: {Time}", nameof(FileCleanupService), DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (Directory.Exists(_directoryPath))
                    {
                        var files = Directory.GetFiles(_directoryPath, "*.json");

                        foreach (var file in files)
                        {
                            var creationTime = File.GetCreationTimeUtc(file);
                            if (DateTime.UtcNow - creationTime > _fileRetention)
                            {
                                File.Delete(file);
                                _logger.LogInformation("Deleted file: {File}", file);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "File cleanup failed.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("File cleanup service stopped.");
        }
    }
}
