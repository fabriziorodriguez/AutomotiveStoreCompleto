namespace AutomotiveStore.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public bool IsActive { get; set; }
        public int IdArticulesTypes { get; set; }

    }
}
