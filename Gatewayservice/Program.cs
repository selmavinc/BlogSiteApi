using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Configuration.AddJsonFile("ocelot.json");
//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//builder.Services.AddOcelot(builder.Configuration);
//builder.Configuration.AddJsonFile("Ocelot_new.json");
//builder.Services.AddOcelot().AddPolly();
//builder.Services.AddOcelot().AddPolly();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot().AddPolly();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseOcelot().Wait();

//app.MapControllers();

app.Run();
