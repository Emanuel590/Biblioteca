
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
        //public DbSet<Productos> Productos { get; set; }

        //Y aca agregamos todas las que necesitemos
        //Usuarios, roles, categorias, facturas
    }
}
