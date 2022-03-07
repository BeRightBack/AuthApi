

using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: "logs\\log-.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information
                ).CreateLogger();


try
{
    Log.Information("Application is Starting");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddCors(o =>
    {
        o.AddPolicy("All", builder =>
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("All");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{

    Log.Fatal(ex, "Application failed to Start");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}