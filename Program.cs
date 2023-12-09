using System.Data.SqlClient;
using System.Net;
using System.Security;

namespace ConsoleApp1
{
    internal class Program
    {
        private string cadenaConexion;




        static void Main(string[] args)
        {
            creacionInicial();
            


        }

        public static void creacionInicial()
        {
            string conexionABDD = "Data Source=DESKTOP-PPMBHAK\\SQLEXPRESS;Initial Catalog=master;User ID=sa;Password=;";

            using (SqlConnection conexion = new SqlConnection(conexionABDD))
            {

                string[] consultas = new string[7];
                consultas[0] = "CREATE DATABASE ProyectoFinalTBDD;";
                consultas[1] = "use ProyectoFinalTBDD;";
                consultas[2] = "CREATE SCHEMA proyecto;";
                consultas[3] = "CREATE TABLE proyecto.Tienda(" +
                    "IdJuego int primary key NOT NULL," +
                    "Nombre varchar(50) NOT NULL," +
                    "Precio money NOT NULL," +
                    "Descripción varchar(200)" +
                    ")";
                consultas[4] = "CREATE TABLE proyecto.Usuario(" +
                    "IdUsuario int primary key IDENTITY(1,1)," +
                    "Nombre varchar(50) not null," +
                   "apellido_usuario varchar(50)," +
                   "email_usuario varchar(50) not null," +
                   "Dinero MONEY not null);";

                consultas[5] = "CREATE TABLE proyecto.Libreria(" +
                    "IdLibreria int primary key Identity(1,1)," +
                    "IdUsuario int not null," +
                    "constraint IdUsuario FOREIGN KEY (IdUsuario) REFERENCES proyecto.Usuario (IdUsuario)," +
                    "IdJuego INT NOT NULL," +
                    "constraint IdJuego FOREIGN KEY (IdJuego) REFERENCES proyecto.Tienda (IdJuego)" +
                    ")";
                consultas[6] = "CREATE TABLE proyecto.Auditoria(" +
                    "id_evento int IDENTITY (1,1) NOT NULL PRIMARY KEY," +
                    "tipo_evento char(10) not null," +
                    "fecha datetime not null," +
                    "descripcion nvarchar(200) null," +
                    "usuario nvarchar(100) null," +
                    "terminal nvarchar (30) null," +
                    "aplicacion nvarchar(500) not null)";



                try
                {
                    conexion.Open();
                    Console.WriteLine("Conexión exitosa");

                    for (int i = 0; i <consultas.Length; i++)
                    {

                        using (SqlCommand command = new SqlCommand(consultas[i], conexion))
                        {
                            // Ejecutar la consulta
                            command.ExecuteNonQuery();
                            Console.WriteLine($"Creado exitosamente {i}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }



            }
        }

            public static void crearLogin()
            {
            //LOGIN
            //Añadir capa de seguridad
            /*  SecureString theSecureString = new NetworkCredential("sa", "").SecurePassword;
              theSecureString.MakeReadOnly(); //Necesario
              SqlCredential credencial = new SqlCredential("", theSecureString);*/

            //Conexion siendo SA porque SA puede crear usuarios
            string connectionString = "Data Source=DESKTOP-PPMBHAK\\SQLEXPRESS;" +
                    "Initial Catalog=master;" +
                    "User ID=sa;Password=;";

                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    Console.WriteLine("Inserte nombre");
                    string nombre = Console.ReadLine();
                    Console.WriteLine("Inserte psswd");
                    string pass = Console.ReadLine();
                    string consultaCrearUsuario = $"CREATE LOGIN {nombre} WITH PASSWORD = '{pass}'";

                    try
                    {
                        // Abrir la conexión
                        conexion.Open();

                        // Realizar operaciones en la base de datos aquí

                        Console.WriteLine("Conexión exitosa");

                        using (SqlCommand command = new SqlCommand(consultaCrearUsuario, conexion))
                        {
                            // Ejecutar la consulta
                            command.ExecuteNonQuery();
                            Console.WriteLine("Usuario creado exitosamente");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }





        }
    }

