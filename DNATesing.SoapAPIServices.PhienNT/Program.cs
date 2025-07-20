using DNATesting.Service.PhienNT;
using DNATesting.SoapAPIServices.PhienNT.SoapServices;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Service Providers
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();

// Add SOAP Services
builder.Services.AddScoped<IDnaTestsPhienNtSoapService, DnaTestsPhienNtSoapService>();
builder.Services.AddScoped<ILociPhienNtSoapService, LociPhienNtSoapService>();

// Add SoapCore services
builder.Services.AddSoapCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Configure SOAP endpoints
app.UseSoapEndpoint<IDnaTestsPhienNtSoapService>("/DnaTestsPhienNtSoapService.asmx", new SoapEncoderOptions());
app.UseSoapEndpoint<ILociPhienNtSoapService>("/LociPhienNtSoapService.asmx", new SoapEncoderOptions());

app.Run();