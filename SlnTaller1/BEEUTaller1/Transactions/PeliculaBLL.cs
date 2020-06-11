using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEEUTaller1.Transactions
{
    public class PeliculaBLL
    {
        public static void Create(Pelicula p)
        {
            //Empezamos la transaccion
            //Siempre que yo CREAR, ACTUALIZA, ELIMINAR debo utilizar una transaccion
            using (Entities db = new Entities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Peliculas.Add(p);
                        db.SaveChanges();
                        transaction.Commit();//Comprometer cambios
                    }
                    catch (Exception ex)
                    {
                        //Rollback -> para evitar datos inconsistentes
                        transaction.Rollback();//Todo se elimina
                        throw ex;
                    }
                }
            }
        }

        //Para que el alumno este dentro del mismo contexto se hace lo siguiente: (PARA UPDATE)
        //GET-CONSULTAR
        public static Pelicula Get(int? id)
        {
            Entities db = new Entities();
            return db.Peliculas.Find(id);
        }

        public static void Update(Pelicula pelicula)
        {
            using (Entities db = new Entities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Peliculas.Attach(pelicula);
                        db.Entry(pelicula).State = System.Data.Entity.EntityState.Modified; //Attach en lugar de SAaveChanges es otra opcion
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public static void Delete(int? id)
        {
            using (Entities db = new Entities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Pelicula alumno = db.Peliculas.Find(id);
                        db.Entry(alumno).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public static List<Pelicula> List()
        {
            Entities db = new Entities(); //Instancia del contexto
            
            return db.Peliculas.ToList();
        }

    }
}
