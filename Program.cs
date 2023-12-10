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
         string conexionABDDADMIN = "Data Source=DESKTOP-PPMBHAK\\SQLEXPRESS;Initial Catalog=master;User ID=sa;Password=;";

        creacionInicial(conexionABDDADMIN);
        crearLogin(conexionABDDADMIN);


        }
        public static void creacionInicial(string con)
        {
            using (SqlConnection conexion = new SqlConnection(con))
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
                    "Portada varbinary(max)"  +
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

            public static void crearLogin(string con)
            {
            //LOGIN
            //Añadir capa de seguridad
            /*  SecureString theSecureString = new NetworkCredential("sa", "").SecurePassword;
              theSecureString.MakeReadOnly(); //Necesario
              SqlCredential credencial = new SqlCredential("", theSecureString);*/

            //Conexion siendo SA porque SA puede crear usuarios
           // string connectionString = "Data Source=DESKTOP-PPMBHAK\\SQLEXPRESS;Initial Catalog=master;User ID=sa;Password=;";

                using (SqlConnection conexion = new SqlConnection(con))
                {
                    Console.WriteLine("Inserte nombre");
                    string nombre = Console.ReadLine();
                    Console.WriteLine("Inserte psswd");
                    string pass = Console.ReadLine();
                Console.WriteLine("Inserte código de administrador"); ;
                    string codigoSecreto = Console.ReadLine();

                
                //AQUI PROBAR CON HACER EL USER EN OTRA CONSULTA, O SEA, EJECUTAR EL USING COPYPASTE POR SEGUNDA VEZ A VER SI FUNCIONA

                string[] consultas = new string[3];
                consultas[0] = "use ProyectoFinalTBDD";
                consultas[1] = $"CREATE LOGIN {nombre} WITH PASSWORD = '{pass}'";
                consultas[2]= $"create user {nombre} for login {nombre} with default_schema = proyecto";
                
                    try
                    {
                        // Abrir la conexión
                        conexion.Open();

                        // Realizar operaciones en la base de datos aquí

                     Console.WriteLine("-----Conexión exitosa para crear login");
                    for(int i=0; i<consultas.Length; i++)
                    {

                        using (SqlCommand command = new SqlCommand(consultas[i], conexion))
                        {
                            // Ejecutar la consulta
                            command.ExecuteNonQuery();
                            Console.WriteLine($"Creado exitosamente {i}");
                        }

                    }


                    string[] permisosConsulta = new string[3];
                    permisosConsulta[0] = "use ProyectoFinalTBDD";
                    //Dar permisos

                    Console.WriteLine("-----dAR PERMISOS");
                    if (codigoSecreto == "administrador")
                    {
                        //Si es admin puede hacer todo incluso crear backup y restaurar
                        permisosConsulta[1] = $"GRANT EXECUTE, INSERT, UPDATE, DELETE, SELECT, ALTER TO {nombre}";
                        permisosConsulta[2] = $"EXEC sp_addrolemember 'db_backupoperator', {nombre}";

                      

                    }
                    else
                    {
                        //Si no lo es, entonces solo puede ver
                        permisosConsulta[1] = $"GRANT SELECT TO {nombre}";
                        permisosConsulta[2] = $"DENY SELECT ON proyecto.Auditoria TO {nombre}";
                    }
                    for (int i = 0; i < permisosConsulta.Length; i++)
                    {
                        using (SqlCommand command = new SqlCommand(permisosConsulta[i], conexion))
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





        }
    }

