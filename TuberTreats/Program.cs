using TuberTreats.Models;
List<Customer> customers = new List<Customer>
{
    new Customer()
    {
        Id = 1,
        Name = "John",
        Address = "123 Big Rd",
        TuberOrders = new List<TuberOrder>()
    },

    new Customer()
    {
        Id = 2,
        Name = "Elisa",
        Address = "535 Big Lake Dr",
        TuberOrders = new List<TuberOrder>()
    }
};

List<TuberOrder> tuberOrders = new List<TuberOrder>
{
    new TuberOrder()
    {
        Id = 1, 
        OrderPlaceOnDate = new DateTime (2024, 11, 11),
        CustomerId = 1,
        TuberDriverId = 1,
        DeliveredOnDate = new DateTime (2024, 11, 12),
        Toppings = new List<Topping>()
    },

    new TuberOrder()
    {
        Id = 2, 
        OrderPlaceOnDate = new DateTime (2024, 11, 11),
        CustomerId = 2,
        TuberDriverId = 1,
        DeliveredOnDate = new DateTime (2024, 11, 12),
        Toppings = new List<Topping>()
    }
};

List<TuberDriver> tuberDrivers = new List<TuberDriver>
{
    new TuberDriver()
    {
        Id = 1,
        Name = "Sam",
        TuberDeliveries = new List<TuberOrder>()
    },
    new TuberDriver()
    {
        Id = 2,
        Name = "Jane",
        TuberDeliveries = new List<TuberOrder>()
    }
};

List<Topping> toppings = new List<Topping>
{
   new Topping()
   {
    Id = 1,
    Name = "Cheese",
   },
   new Topping()
   {
    Id = 2, 
    Name = "Sour Cream"
   },
   new Topping() 
   {
    Id = 3,
    Name = "Chives"
   },
   new Topping()
   {
    Id = 4,
    Name = "Bacon Bits"
   }
};

List<TuberTopping> tuberToppings = new List<TuberTopping>
{
    new TuberTopping()
    {
        Id = 1,
        TuberOrderId = 1,
        ToppingId = 1
    },
    new TuberTopping()
    {
        Id = 2,
        TuberOrderId = 1,
        ToppingId = 2
    },
    new TuberTopping()
    {
        Id = 3,
        TuberOrderId = 2,
        ToppingId = 3
    },
    new TuberTopping()
    {
        Id = 4,
        TuberOrderId = 2,
        ToppingId = 4
    }
};


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

//add endpoints here

app.Run();
//don't touch or move this!
public partial class Program { }