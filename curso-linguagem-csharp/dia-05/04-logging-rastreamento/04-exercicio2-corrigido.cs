Log.Logger = new LoggerConfiguration()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithProperty("ApplicationName", "ECommerce")
    .WriteTo.File(new CompactJsonFormatter(), "logs/ecommerce-.json",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Use @ para serialização completa
Log.Information("Carrinho criado {@Carrinho}", carrinho);