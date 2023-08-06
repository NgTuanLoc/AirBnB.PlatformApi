using PlatformApi.Extensions;
using PlatformApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

//adds all the controller classes as services
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
app.UseHsts();

app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication(); //Reading Identity cookie

app.UseAuthorization(); //Validates access permissions of the user

app.MapControllers(); //Execute the filter pipeline (action + filters)

app.Run();
