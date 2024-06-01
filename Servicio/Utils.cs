using System;

namespace RestoApp_Api.Servicio
{
    static public class Utils

    {
        public static bool Fecha1MayorFecha2(string fecha1, string fecha2)
        {
            DateTime date1 = DateTime.ParseExact(fecha1, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            DateTime date2 = DateTime.ParseExact(fecha2, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            return date1 > date2;
        }

        public static int? CompararFecha(string fechaInicio, string? fechaFin = null, bool abs = false)
        {

            DateTime fechaFinDate;
            if (string.IsNullOrEmpty(fechaInicio))
            {
                return null;
            }
            DateTime fechaInicioDate = DateTime.ParseExact(fechaInicio, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            if (string.IsNullOrEmpty(fechaFin))
            {
                fechaFinDate = DateTime.Today;
            }
            else
            {
                fechaFinDate = DateTime.ParseExact(fechaFin, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            }
            TimeSpan diferencia = fechaInicioDate - fechaFinDate;
            int diasDiferencia = (abs) ? Math.Abs(diferencia.Days) : diferencia.Days;
            return diasDiferencia;
        }

    }
}
