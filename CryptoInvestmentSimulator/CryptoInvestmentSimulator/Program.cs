using CryptoInvestmentSimulator;

internal class Program
{
    private static readonly GlobalOperations globalOperations = new();

    /// <summary>
    /// Starts global timer on application launch
    /// and executes all global operations every 2 minutes.
    /// </summary>
    static void OnStartedActions()
    {
        var timer = new System.Timers.Timer { Interval = 60000 };
        timer.Elapsed += ExecuteGlobalOperations;
        timer.Start();
    }

    /// <summary>
    /// Executes latest market data collection.
    /// Executes bad position liquidation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="elapsedEvent"></param>
    static void ExecuteGlobalOperations(object sender, System.Timers.ElapsedEventArgs elapsedEvent)
    {
        globalOperations.CollectLatestMarketData();
        Console.WriteLine($"New market data collected! Collection time: {DateTime.Now}");

        // globalOperations.LiquidateBadPositions();
        Console.WriteLine($"Poor positions liquidated! Liquidation time: {DateTime.Now}");
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.RegisterServices();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Landing/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(name: "default", pattern: "{controller=Landing}/{action=Index}/{id?}");

        app.Lifetime.ApplicationStarted.Register(OnStartedActions);

        app.Run();
    }
}
