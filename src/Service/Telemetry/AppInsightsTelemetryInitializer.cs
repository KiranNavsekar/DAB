using System;
using System.Collections.Generic;
using Azure.DataApiBuilder.Product;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

public class AppInsightsTelemetryInitializer : ITelemetryInitializer
{
    public static readonly IReadOnlyDictionary<string, string> GlobalProperties = new Dictionary<string, string>
    {
        { "ProductName", "Data API builder"},
        { "UserAgent", $"{ProductInfo.GetDataApiBuilderUserAgent()}" }
        // Add more custom properties here
    };

    /// <summary>
    /// Initializes the telemetry context.
    /// </summary>
    /// <param name="telemetry">The telemetry object to</param>
    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = ProductInfo.GetDataApiBuilderUserAgent();
        telemetry.Context.Session.Id = Guid.NewGuid().ToString();
        telemetry.Context.Component.Version = ProductInfo.GetProductVersion();

        foreach (KeyValuePair<string, string> property in GlobalProperties)
        {
            telemetry.Context.GlobalProperties.Add(property.Key, property.Value);
        }
    }
}
