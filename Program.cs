using Microsoft.EntityFrameworkCore;
using RepoPatt_Api;
using RepoPatt_Core.Interfaces;
using RepoPatt_EF;
using RepoPatt_EF.Repos;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(
    x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn"),
    y => y.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName))); //determine ApplicationDBContext assembly 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

//builder.Services.AddTransient(typeof(IBaseRepository<>),typeof(BaseRepository<>)); //using Repo
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>(); //using unit of work
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x=>x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
