public class TuberToppingDTO
{
    public int Id { get; set; }
    public int TuberOrderId { get; set; }
    public int ToppingId { get; set; }
    public ToppingDTO Topping { get; set; }
}
