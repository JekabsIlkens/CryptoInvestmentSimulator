using CryptoInvestmentSimulator;

internal class Program
{
    private static readonly GlobalOperations globalOperations = new();

    /// <summary>
    /// Starts global timer on application launch and executes
    /// market data retrieval and position liquidation methods on each elapsed interval.
    /// </summary>
    static void OnStartedActions()
    {
        var timer = new System.Timers.Timer { Interval = 60000 };
        timer.Elapsed += CollectMarketDataAndLiquidatePositions;
        timer.Start();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="elapsedEvent"></param>
    static void CollectMarketDataAndLiquidatePositions(object sender, System.Timers.ElapsedEventArgs elapsedEvent)
    {
        globalOperations.InsertMarketData();
        Console.WriteLine($"New market data collected! Collection time: {DateTime.Now}");

        // globalOperations.LiquidatePositions();
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
