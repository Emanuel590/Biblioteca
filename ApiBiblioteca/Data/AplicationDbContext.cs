
using System.Data;
using ApiBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Data
{
    //Permite la interciconexion entre el String y la aplicacion para conectarse a BD
    public class AplicationDbContext : DbContext
    {
        //Constructor vacio me permite inicializar cualquier clase.
        //En nuestro caso el constructor inicializa los datos de interconexion
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {

        }

        //Necesitamos que van hacer los set y gets.
        //La definicion de entidades y modelos

       public DbSet<Roles> BIBLIOTECA_ROLE_TB {  get; set; }
        public DbSet<Usuarios> BIBLIOTECA_USUARIOS_TB {  get; set; }
        public DbSet<Generos> BIBLIOTECA_GENERO_TB { get; set; }
        //public DbSet<Productos> BIBLIOTECA_PRODUCTOS_TB { get; set; }
        //public DbSet<Pagos> BIBLIOTECA_METODO_PAGO_TB { get; set; }
        public DbSet<Resenas> BIBLIOTECA_RESENA_TB { get; set; }
        public DbSet<Reclamos> BIBLIOTECA_RECLAMOS_TB { get; set; }
        //public DbSet<Reservas> BIBLIOTECA_RESERVAS_TB { get; set; }
        public DbSet<Facturas> BIBLIOTECA_FACTURAS_TB { get; set; }


        public DbSet<Libros> BIBLIOTECA_LIBROS_TB { get; set; }
        public DbSet<Estados> BIBLIOTECA_ESTADO_TB { get; set; }
        public DbSet<Autores> BIBLIOTECA_AUTOR_TB { get; set; }
        public DbSet<Categorias> BIBLIOTECA_CATEGORIA_TB { get; set; }

        //Y aca agregamos todas las que necesitemos
        //Usuarios, roles, categorias, facturas
    }
}


// COMMIT ANDONY