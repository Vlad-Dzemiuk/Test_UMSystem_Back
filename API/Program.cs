using Application.Sections.Commands;
using Infrastructure;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<MongoDbService>();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSectionCommand).Assembly));
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}