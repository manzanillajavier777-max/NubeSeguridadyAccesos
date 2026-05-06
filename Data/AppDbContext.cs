using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        {
            
        }
        public DbSet<AccesoRegistros> AccesoRegistros {get;set;}
        public DbSet<AsignacionEquipamiento> AsignacionEquipamientos {get;set;}
        public DbSet<DatosBiometricos> DatosBiometricos {get;set;}
        public DbSet<Dispositivo> Dispositivo {get;set;}
        public DbSet<Equipamiento> Equipamiento {get;set;}
        public DbSet<Horario> Horario {get;set;}
        public DbSet<IdentidadAcceso> IdentidadAcceso {get;set;}   
        public DbSet<Reporte> Reporte {get;set;}
        public DbSet<Rol> Rol {get;set;}
        public DbSet<RolHorario> RolHorario {get;set;}
        public DbSet<RolIdentidad> RolIdentidad {get;set;}
        
    }


}