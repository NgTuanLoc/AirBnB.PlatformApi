using PlatformApi.Extensions;
using PlatformApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration, builder.Environment);
builder.Services.ConfigureExternalServices(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSwagger();

var app = builder.Build();

//adds all the controller classes as services
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
app.UseHsts();

app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Platform API 1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Platform API 2.0");
});

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication(); //Reading Identity cookie

app.UseAuthorization(); //Validates access permissions of the user

app.MapControllers(); //Execute the filter pipeline (action + filters)

app.Run();
