var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContextFactory<LibraryDbContext>(options => {
    // pomelo requires a MySqlServerVersion object, for whatever reason
    options.UseMySql("server=...;database=...;user=...;password=...",
        new MySqlServerVersion(new Version(8, 0, 28)), o => o.MigrationsAssembly("Library"));
});
// register IRepo / Repo as transient so we will 
// have multiple instances of them (fro different types)
builder.Services.AddTransient(typeof(IRepo<>), typeof(Repo<>));

// register uow and logic as scoped so that db context
// will live for just one request
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBooksLogic, BooksLogic>();
builder.Services.AddScoped<IUsersLogic, UsersLogic>();

// swagger to test
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();