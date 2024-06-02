namespace RestoApp_Api.Model
{
    public class ProductoDto
    {
        public int ProductoId { get; set; }
        public string? Nombre_producto { get; set; }
        public double Precio { get; set; }
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public int Cantidad { get; set; }
    }

}

