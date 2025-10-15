using BLL.Mappings;
using BLL.IServices;
using BLL.Services;
using DAL;
using DAL.IRepositories;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register DbContext
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AutoMapper profiles
builder.Services.AddAutoMapper(typeof(AutoMappingProfile));

// Register Repositories
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IStaffRepo, StaffRepo>();
builder.Services.AddScoped<IInventoryRepo, InventoryRepo>();
builder.Services.AddScoped<IVersionRepo, VersionRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
builder.Services.AddScoped<IColorRepo, ColorRepo>();
builder.Services.AddScoped<IModelRepo, ModelRepo>();
builder.Services.AddScoped<ITestDriveAppointmentRepo, TestDriveAppointmentRepo>();

// Register Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IVersionService, VersionService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<ITestDriveAppointmentService, TestDriveAppointmentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
