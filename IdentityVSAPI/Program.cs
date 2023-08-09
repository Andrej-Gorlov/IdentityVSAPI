using IdentityVSAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddJwtAuthentication();
builder.Services.AddControllers();
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

app.UseAuthentication();
app.UseAuthorization();

//using (var scope = app.Services.CreateScope())
//{
//    var service = scope.ServiceProvider;
//    IDbBaseUserInitializer dbInitializer = service.GetRequiredService<IDbBaseUserInitializer>();
//    dbInitializer.Initialize();
//}

app.MapControllers();

app.Run();
