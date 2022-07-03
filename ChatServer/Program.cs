using ChatServer.Hubs;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    var userPolicy = options.GetPolicy(options.DefaultPolicyName);


    var corsBuilder = new CorsPolicyBuilder();
    corsBuilder.AllowAnyHeader();
    corsBuilder.AllowAnyMethod();

    // Sets Allow all origins to true for SignalR
    // https://stackoverflow.com/a/53790841

    System.Console.WriteLine($"Service has registered hubs, allowing all origins");
    corsBuilder.SetIsOriginAllowed(_ => true);
    corsBuilder.AllowCredentials();


    options.AddDefaultPolicy(corsBuilder.Build());

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();


app.MapHub<ChatHub>("/chatHub");
app.Run();

