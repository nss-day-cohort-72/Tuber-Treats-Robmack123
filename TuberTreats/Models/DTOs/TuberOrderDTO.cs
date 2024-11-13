public class TuberOrderDTO
{
    public int Id { get; set; }
    public DateTime OrderPlaceOnDate { get; set; }
    public DateTime? DeliveredOnDate { get; set; } // Make this nullable to handle undelivered orders
    public int CustomerId { get; set; }
    public CustomerDTO Customer { get; set; }
    public int? TuberDriverId { get; set; } // Driver is nullable initially since no driver is assigned
    public TuberDriverDTO TuberDriver { get; set; }
    public List<ToppingDTO> Toppings { get; set; }
}
