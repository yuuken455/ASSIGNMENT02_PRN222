using BLL.Mappings;
using BLL.IServices;
using BLL.Services;
using DAL;
using DAL.IRepositories;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using EVDManagement.SignalR; // <-- QUAN TRỌNG: using đúng namespace chứa ModelHub

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// DbContext
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMappingProfile));

// Repositories
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IStaffRepo, StaffRepo>();
builder.Services.AddScoped<IInventoryRepo, InventoryRepo>();
builder.Services.AddScoped<IVersionRepo, VersionRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderDetailRepo, OrderDetailRepo>();
builder.Services.AddScoped<IColorRepo, ColorRepo>();
builder.Services.AddScoped<IModelRepo, ModelRepo>();
builder.Services.AddScoped<ITestDriveAppointmentRepo, TestDriveAppointmentRepo>();

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IVersionService, VersionService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<ITestDriveAppointmentService, TestDriveAppointmentService>();

// >>> SignalR (bắt buộc để DI resolve IHubContext<T>)
builder.Services.AddSignalR();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// >>> Map hub endpoint (client sẽ connect tới /hubs/models)
app.MapHub<ModelHub>("/hubs/models");

app.Run();
