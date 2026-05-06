using DTOS;
using Entities;

namespace Mapeador
{
    public static class ReporteMapeador
    {
        public static ReporteDTO toReporteDTO(this Reporte reporte)
        {
            return new ReporteDTO
            {
                codigoReporte = reporte.codigoReporte,
                TipoReporte = reporte.TipoReporte,
                Descripcion = reporte.Descripcion,
                FechaReporte = reporte.FechaReporte
            };
        }
    }
}