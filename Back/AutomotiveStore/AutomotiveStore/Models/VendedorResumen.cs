namespace AutomotiveStore.Models
{
    public class VendedorResumen
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int CantidadArticulosVendidos { get; set; }
        public decimal TotalVentas { get; set; }

    }
}

