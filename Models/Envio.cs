namespace RestoApp_Api.Models
{
    public class Envio
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public double costo { get; set; }
        public Repartidor? repartidor { get; set; }
        public bool estado_envio { get; set; }
    }
}