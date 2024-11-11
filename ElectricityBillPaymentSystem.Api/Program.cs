using ElectricityBillPaymentSystem.Api.Extensions;
using ElectricityBillPaymentSystem.Api.Middlewares;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();

app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();

app.MapControllers();


app.Run();