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
        TuberDriverId = null,
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

app.MapGet("/tuberorders", () =>
{
    List<TuberOrderDTO> orders = tuberOrders.Select(order => new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlaceOnDate = order.OrderPlaceOnDate,
        DeliveredOnDate = order.DeliveredOnDate,
        CustomerId = order.CustomerId,
        Customer = customers.FirstOrDefault(c => c.Id == order.CustomerId) != null
            ? new CustomerDTO
            {
                Id = order.CustomerId,
                Name = customers.First(c => c.Id == order.CustomerId).Name,
                Address = customers.First(c => c.Id == order.CustomerId).Address
            }
            : null,
        TuberDriverId = order.TuberDriverId.HasValue ? order.TuberDriverId.Value : (int?)null,
        TuberDriver = order.TuberDriverId.HasValue && tuberDrivers.FirstOrDefault(d => d.Id == order.TuberDriverId) != null
                ? new TuberDriverDTO
                {
                    Id = order.TuberDriverId.Value,
                    Name = tuberDrivers.First(d => d.Id == order.TuberDriverId).Name
                }
                : null,
        Toppings = tuberToppings
                .Where(tt => tt.TuberOrderId == order.Id)
                .Select(tt => new ToppingDTO
                {
                    Id = tt.ToppingId,
                    Name = toppings.First(t => t.Id == tt.ToppingId).Name
                }).ToList()

    }).ToList();

    return Results.Ok(orders);
});



app.MapGet("/tuberorders/{id}", (int id) =>
{
    TuberOrder order = tuberOrders.FirstOrDefault(o => o.Id == id);
    if (order == null)
    {
        return Results.NotFound("Please enter a valid Id.");
    }

    // Find the related customer
    Customer customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);

    // Find the related driver (if any)
    TuberDriver driver = tuberDrivers.FirstOrDefault(d => d.Id == order.TuberDriverId);

    // Find related toppings
    List<ToppingDTO> toppingDTOs = tuberToppings
        .Where(tt => tt.TuberOrderId == order.Id)
        .Select(tt => new ToppingDTO
        {
            Id = tt.ToppingId,
            Name = toppings.First(t => t.Id == tt.ToppingId).Name
        }).ToList();

    // Create and return TuberOrderDTO
    TuberOrderDTO orderDTO = new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlaceOnDate = order.OrderPlaceOnDate,
        DeliveredOnDate = order.DeliveredOnDate,
        CustomerId = order.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        TuberDriverId = order.TuberDriverId,
        TuberDriver = driver == null ? null : new TuberDriverDTO
        {
            Id = driver.Id,
            Name = driver.Name
        },
        Toppings = toppingDTOs
    };

    return Results.Ok(orderDTO);
});

app.MapPost("/tuberorders", (TuberOrder newOrder) =>
{
    newOrder.Id = tuberOrders.Count > 0 ? tuberOrders.Max(o => o.Id) + 1 : 1;

    newOrder.OrderPlaceOnDate = DateTime.Now;

    if (newOrder.Toppings == null)
    {
        newOrder.Toppings = new List<Topping>();
    }

    tuberOrders.Add(newOrder);

    foreach (var topping in newOrder.Toppings)
    {
        tuberToppings.Add(new TuberTopping
        {
            Id = tuberToppings.Count > 0 ? tuberToppings.Max(tt => tt.Id) + 1 : 1,
            TuberOrderId = newOrder.Id,
            ToppingId = topping.Id
        });
    }

   
    TuberOrderDTO newOrderDTO = new TuberOrderDTO
    {
        Id = newOrder.Id,
        OrderPlaceOnDate = newOrder.OrderPlaceOnDate,
        DeliveredOnDate = newOrder.DeliveredOnDate,
        CustomerId = newOrder.CustomerId,
        Customer = customers.FirstOrDefault(c => c.Id == newOrder.CustomerId) != null
                    ? new CustomerDTO
                    {
                        Id = newOrder.CustomerId,
                        Name = customers.First(c => c.Id == newOrder.CustomerId).Name,
                        Address = customers.First(c => c.Id == newOrder.CustomerId).Address
                    }
                    : null,
        TuberDriverId = null, 
        TuberDriver = null,  
        Toppings = newOrder.Toppings.Select(t => new ToppingDTO
        {
            Id = t.Id,
            Name = t.Name
        }).ToList()
    };

    return Results.Created($"/tuberorders/{newOrderDTO.Id}", newOrderDTO);
});

app.MapPut("/tuberorders/{id}/driver/{driverId}", (int id, int driverId) =>
{
    TuberOrder order = tuberOrders.FirstOrDefault(o => o.Id == id);
    if (order == null)
    {
        return Results.NotFound("Order not found. Please provide a valid order Id.");
    }

    TuberDriver driver = tuberDrivers.FirstOrDefault(d => d.Id == driverId);
    if (driver == null)
    {
        return Results.NotFound("Driver not found. Please provide a valid driver Id.");
    }

    order.TuberDriverId = driverId;

    TuberOrderDTO updatedOrderDTO = new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlaceOnDate = order.OrderPlaceOnDate,
        DeliveredOnDate = order.DeliveredOnDate,
        CustomerId = order.CustomerId,
        Customer = customers.FirstOrDefault(c => c.Id == order.CustomerId) != null
                    ? new CustomerDTO
                    {
                        Id = order.CustomerId,
                        Name = customers.First(c => c.Id == order.CustomerId).Name,
                        Address = customers.First(c => c.Id == order.CustomerId).Address
                    }
                    : null,
        TuberDriverId = driverId,
        TuberDriver = new TuberDriverDTO
        {
            Id = driverId,
            Name = driver.Name
        },
        Toppings = tuberToppings
                    .Where(tt => tt.TuberOrderId == order.Id)
                    .Select(tt => new ToppingDTO
                    {
                        Id = tt.ToppingId,
                        Name = toppings.First(t => t.Id == tt.ToppingId).Name
                    }).ToList()
    };

    return Results.Ok(updatedOrderDTO);
});

app.MapPost("/tuberorders/{id}/complete", (int id) =>
{
    TuberOrder order = tuberOrders.FirstOrDefault(o => o.Id == id);
    if (order == null)
    {
        return Results.NotFound("Order not found. Please enter a valid ID");
    }

    order.DeliveredOnDate = DateTime.Now;
    TuberOrderDTO updatedOrderDTO = new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlaceOnDate = order.OrderPlaceOnDate,
        DeliveredOnDate = order.DeliveredOnDate,
        CustomerId = order.CustomerId,
        Customer = customers.FirstOrDefault(c => c.Id == order.CustomerId) != null
                ? new CustomerDTO
                    {
                        Id = order.CustomerId,
                        Name = customers.First(c => c.Id == order.CustomerId).Name,
                        Address = customers.First(c => c.Id == order.CustomerId).Address
                    }
                    : null,
        TuberDriverId = order.TuberDriverId,
        TuberDriver = order.TuberDriverId.HasValue
                    ? new TuberDriverDTO
                    {
                        Id = order.TuberDriverId.Value,
                        Name = tuberDrivers.First(d => d.Id == order.TuberDriverId.Value).Name
                    }
                    : null,
        Toppings = tuberToppings
                    .Where(tt => tt.TuberOrderId == order.Id)
                    .Select(tt => new ToppingDTO
                    {
                        Id = tt.ToppingId,
                        Name = toppings.First(t => t.Id == tt.ToppingId).Name
                    }).ToList()
    };

    return Results.Ok(updatedOrderDTO);
});

// topping endpoints
app.MapGet("/toppings", () =>
{
    List<ToppingDTO> toppingList = toppings.Select(t => new ToppingDTO
    {
        Id = t.Id,
        Name = t.Name
    }).ToList();

    return Results.Ok(toppingList);
});

app.MapGet("/toppings/{id}", (int id) =>
{
    Topping topping = toppings.FirstOrDefault(t => t.Id == id);
    if (topping == null)
    {
        return Results.NotFound("Topping not found. Please provide a valid topping Id.");
    }

    ToppingDTO toppingDTO = new ToppingDTO
    {
        Id = topping.Id,
        Name = topping.Name
    };

    return Results.Ok(toppingDTO);
});

// tubertopping endpoints

app.MapGet("/tubertoppings", () =>
{
    List<TuberToppingDTO> tuberToppingList = tuberToppings.Select(tt => new TuberToppingDTO
    {
        Id = tt.Id,
        TuberOrderId = tt.TuberOrderId,
        ToppingId = tt.ToppingId,
        Topping = toppings.FirstOrDefault(t => t.Id == tt.ToppingId) != null
                ? new ToppingDTO
                {
                    Id = tt.ToppingId,
                    Name = toppings.First(t => t.Id ==tt.ToppingId).Name
                }
                : null
    }).ToList();


    return Results.Ok(tuberToppingList);
});

app.MapPost("/tubertoppings", (TuberTopping newTuberTopping) =>
{
    TuberOrder order = tuberOrders.FirstOrDefault(o => o.Id == newTuberTopping.TuberOrderId);
    if (order == null)
    {
        return Results.NotFound("Order not found. Please provide a valid order Id.");
    }

    Topping topping = toppings.FirstOrDefault(t => t.Id == newTuberTopping.ToppingId);
    if (topping == null)
    {
        return Results.NotFound("Topping not found. Please provide a valid topping Id.");
    }

    newTuberTopping.Id = tuberToppings.Count > 0 ? tuberToppings.Max(tt => tt.Id) + 1 : 1;

    tuberToppings.Add(newTuberTopping);

    TuberToppingDTO newTuberToppingDTO = new TuberToppingDTO
    {
        Id = newTuberTopping.Id,
        TuberOrderId = newTuberTopping.TuberOrderId,
        ToppingId = newTuberTopping.ToppingId,
        Topping = new ToppingDTO
        {
            Id = topping.Id,
            Name = topping.Name
        }
    };

    return Results.Created($"/tubertoppings/{newTuberTopping.Id}", newTuberToppingDTO);
});

app.MapDelete("/tubertoppings/{id}", (int id) =>
{
    TuberTopping tuberTopping = tuberToppings.FirstOrDefault(tt => tt.Id == id);
    if (tuberTopping == null)
    {
        return Results.NotFound("TuberTopping not found. Please provide a valid Id.");
    }

    tuberToppings.Remove(tuberTopping);

    return Results.NoContent();
});

// customers endpoints

app.MapGet("/customers", () =>
{
    List<CustomerDTO> customerList = customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    }).ToList();

    return Results.Ok(customerList);
});

app.MapGet("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound("Customer not found. Please provide a valid customer Id.");
    };

    List<TuberOrderDTO> customerOrders = tuberOrders
        .Where(o => o.CustomerId == id)
        .Select(o => new TuberOrderDTO
        {
            Id = o.Id,
            OrderPlaceOnDate = o.OrderPlaceOnDate,
            DeliveredOnDate = o.DeliveredOnDate,
            CustomerId = o.CustomerId,
            TuberDriverId = o.TuberDriverId,
            TuberDriver = o.TuberDriverId.HasValue
                        ? new TuberDriverDTO
                        {
                            Id = o.TuberDriverId.Value,
                            Name = tuberDrivers.First(d => d.Id == o.TuberDriverId.Value).Name
                        }
                        : null,
            Toppings = tuberToppings
                        .Where(tt => tt.TuberOrderId == o.Id)
                        .Select(tt => new ToppingDTO
                        {
                            Id = tt.ToppingId,
                            Name = toppings.First(t => t.Id == tt.ToppingId).Name
                        }).ToList()
        }).ToList();

        CustomerDTO customerDTO = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address,
            TuberOrders = customerOrders
        };

        return Results.Ok(customerDTO);
});

app.MapPost("/customers", (Customer newCustomer) =>
{
    newCustomer.Id = customers.Count > 0 ? customers.Max(c => c.Id) + 1 : 1;
    customers.Add(newCustomer);

    CustomerDTO newCustomerDTO = new CustomerDTO
    {
        Id = newCustomer.Id,
        Name = newCustomer.Name,
        Address = newCustomer.Address
    };

    return Results.Created($"/customers/{newCustomerDTO.Id}", newCustomerDTO);
});

app.MapDelete("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound("Customer not found. Please provide a valid customer Id.");
    }

    customers.Remove(customer);

    tuberOrders.RemoveAll(o => o.CustomerId == id);

    return Results.NoContent();
});



app.Run();
//don't touch or move this!
public partial class Program { }