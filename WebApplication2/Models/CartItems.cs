namespace WebApplication2.Models
{
    public class CartItems
    {
        public int Id { get; set; }

 

        //make name on the card the foreign key
        public string? OrderInformationNameonCard { get; set; }

        public string ? item { get; set; } = null!;

        public string? price { get; set; } = null!;

     //   public virtual OrderInformation OrderInformation { get; set; }
    }
}
