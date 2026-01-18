// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configurar serviços base
builder.Services.AddControllersWithViews();

// Configurar logging por ambiente
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Information);
}
else
{
    builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
}

// Configurar Kestrel por ambiente
builder.WebHost.ConfigureKestrel((context, options) =>
{
    if (context.HostingEnvironment.IsDevelopment())
    {
        // Desenvolvimento: HTTP e HTTPS
        options.ListenAnyIP(5000);
        options.ListenAnyIP(5001, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    }
    else
    {
        // Produção: apenas HTTPS
        options.ListenAnyIP(80); // Para redirecionamento
        options.ListenAnyIP(443, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    }
});

var app = builder.Build();

// Configuração do pipeline por ambiente
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();