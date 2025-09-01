using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using MyOfficeService.Domain.Errors;
using MyOfficeService.Domain.Extations;
using MyOfficeService.Presentation.DependencyInjection;
using MyOfficeService.Presentation.Extations;
using MyOfficeService.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<DatabaseInitializerHostedService>();

builder.Services.ConfigurationService(builder.Configuration);

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = (context) =>
    {
        var errors = context.ModelState
            .Where(e => e.Value != null && e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors.Select(e =>
                GeneralErrors.Validation.InvalidField(e.ErrorMessage)))
            .ToErrorList();

        var result = UnitResult.Failure<ErrorList>(errors);

        return new BadRequestObjectResult(result);
    };
});

var app = builder.Build();

app.UseGlobalExceptionsHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v/swagger.json", "OfficeService API v");
        options.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

app.Run();