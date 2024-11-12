namespace TuberTreats.Models
{
    public class TuberOrder
    {
        public int Id { get; set; }
        public DateTime OrderPlaceOnDate { get; set; }
        public int CustomerId { get; set; }
        public int TuberDriverId { get; set; }
        public DateTime DeliveredOnDate { get; set; }
        public List<Topping> Toppings { get; set; }

        // Constructor to initialize toppings list
        public TuberOrder()
        {
            Toppings = new List<Topping>();
        }
    }
}
