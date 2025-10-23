using BLL.IServices;
using BLL.Mappings;
using BLL.Services;
using DAL;
using DAL.IRepositories;
using DAL.Repositories;
using EVDManagement.SignalR;
using Microsoft.EntityFrameworkCore;
using DAL.IRepository;
using DAL.Repository; // <— thêm

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddSession();

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

// >>> SignalR
builder.Services.AddSignalR(); // <— thêm dòng này

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<CustomerHub>("/customerHub");

// >>> Map SignalR hubs
app.MapHub<ModelHub>("/hubs/models"); // <— thêm endpoint (đổi tên/đường dẫn tùy bạn)

app.Run();
