namespace F1Manager.PointsCalculation.Configuration;

public class PointsCalculationOptions
{
    public string AzureStorageAccount { get; set; } = default!;
    public string CacheConnectionString { get; set; } = default!;
}