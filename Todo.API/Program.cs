/***************************************************************************************
*                                                                                      *
*    Posso todas as coisas em Cristo que me fortalece                                  *
*                                                                                      *
*    Filipenses 4:13                                                                   *
*                                                                                      *
****************************************************************************************/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddInfrastructureApi(builder.Configuration)
    .AddInfrastructureJWT()
    .AddInfrastructureSwagger()
    .AddInfrastructureCORS(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo.WebAPI v1"));
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(builder.Configuration.GetSection("Cors:PolicyName").Value!);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
