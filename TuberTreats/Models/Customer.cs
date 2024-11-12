namespace TuberTreats.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<TuberOrder> TuberOrders { get; set; }
        
        public Customer()
        {
            TuberOrders = new List<TuberOrder>();
        }

        public Customer(int id, string name, string address, List<TuberOrder> tuberOrders)
        {
            Id = id;
            Name = name;
            Address = address;
            TuberOrders = tuberOrders ?? new List<TuberOrder>();
        }
    }
}
