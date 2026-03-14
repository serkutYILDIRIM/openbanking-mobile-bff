using openbanking_mobile_bff.Common.Middleware;
using openbanking_mobile_bff.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBffServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<OhvpsHeaderValidationMiddleware>();
app.UseMiddleware<JwtBearerMiddleware>();
app.UseMiddleware<JwsValidationMiddleware>();

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseBffSwagger();
app.MapBffHealthChecks();

app.Run();


