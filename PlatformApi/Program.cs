using PlatformApi.Extensions;
using PlatformApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

//adds all the controller classes as services
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication(); //Reading Identity cookie

app.UseAuthorization(); //Validates access permissions of the user

app.MapControllers(); //Execute the filter pipeline (action + filters)

app.Run();
