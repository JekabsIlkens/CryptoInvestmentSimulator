using CryptoInvestmentSimulator;
using CryptoInvestmentSimulator.Controllers;

internal class Program
{
    private static readonly MarketController marketController = new();

    /// <summary>
    /// Starts global timer on application launch and executes
    /// market data retrieval method on each elapsed interval.
    /// </summary>
    static void OnStartedActions()
    {
        var timer = new System.Timers.Timer { Interval = 60000 };
        timer.Elapsed += GetNewMarketData;
        timer.Start();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="elapsedEvent"></param>
    static void GetNewMarketData(object sender, System.Timers.ElapsedEventArgs elapsedEvent)
    {
        marketController.InsertMarketData(marketController.GetNewMarketData());
        Console.WriteLine("New market data collected! Collection time: " + DateTime.Now.ToString());
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

        ///* UNCOMMENT TO TURN ON MARKET DATA COLLECTION *///
        // app.Lifetime.ApplicationStarted.Register(OnStartedActions);

        app.Run();
    }
}
