using CryptoInvestmentSimulator;
using CryptoInvestmentSimulator.Database;

internal class Program
{
    public IConfiguration Configuration { get; }
    private static readonly GlobalOperations globalOperations = new();

    public Program(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Starts global timer on application launch
    /// and executes all global operations every 2 minutes.
    /// </summary>
    static void OnStartedActions()
    {
        var timer = new System.Timers.Timer { Interval = 120000 };
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

        globalOperations.LiquidateBadPositions();
        Console.WriteLine($"Poor positions liquidated! Liquidation time: {DateTime.Now}");
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.RegisterServices();

        // Registers database context with access key from appsettings.json.
        var configuration = builder.Configuration;
        var value = configuration.GetValue<string>("ConnectionStrings:defaultConnection");
        builder.Services.Add(new ServiceDescriptor(typeof(DatabaseContext), new DatabaseContext(value)));

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
