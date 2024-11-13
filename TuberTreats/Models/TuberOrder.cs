namespace TuberTreats.Models
{
    public class TuberOrder
    {
        public int Id { get; set; }
        public DateTime OrderPlaceOnDate { get; set; }
        public int CustomerId { get; set; }
        public int? TuberDriverId { get; set; } // Nullable since no driver is assigned initially
        public DateTime? DeliveredOnDate { get; set; } // Make this nullable to handle null values
        public List<Topping> Toppings { get; set; }
    }
}
