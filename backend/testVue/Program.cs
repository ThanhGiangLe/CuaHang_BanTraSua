using Microsoft.EntityFrameworkCore;
using testVue.Configs;
using testVue.Datas;
using testVue.Handls;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký DI
builder.Services.AddScoped<IHandleOrderItem, HandleOrderItem>();
builder.Services.AddScoped<IHandleMaterials, HandleMaterials>();
builder.Services.AddScoped<MaterialConfigs>();
builder.Services.AddScoped<MaterialIdConfigs>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<UserConnectionManager>(); 

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); 
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.MapHub<UserHub>("/userhub"); // endpoint cho SignalR

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
