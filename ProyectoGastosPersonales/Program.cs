using System;
using Microsoft.Data.Sqlite;

namespace ProyectoGastosPersonales
{
    class Program
    {
        private static string cadenaConexion = "Data Source=gastos.db";

        static void Main(string[] args)
        //[span_10](start_span)[span_10](end_span)
        {
            InicializarBaseDatos();

            // 1. Pantalla de Bienvenida y Personalización inicial
            Console.Clear();
            Console.WriteLine("=== BIENVENIDO AL SISTEMA DE GASTOS PERSONALES ===");
            Console.Write("¿Cuál es tu nombre?: ");
            string nombreUsuario = Console.ReadLine() ?? "Usuario";

            Console.Write("¿A qué te dedicas (Ocupación)?: ");
            string ocupacionUsuario = Console.ReadLine() ?? "No especificada";

            Console.Write("¿Cómo percibes tus gastos actualmente (Trabajo, beca, etc.)?: ");
            string fuenteIngresos = Console.ReadLine() ?? "General";

            Console.WriteLine($"\n¡Hola, {nombreUsuario} ({ocupacionUsuario})! Tus ingresos provienen de: {fuenteIngresos}.");
            Console.WriteLine("Presiona cualquier tecla para ir al menú principal...");
            Console.ReadKey();

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine($"=== CONTROL DE GASTOS: {nombreUsuario.ToUpper()} ===");
                Console.WriteLine("1. Registrar nuevo gasto (Crear)");
                Console.WriteLine("2. Ver lista de gastos (Leer)");
                Console.WriteLine("3. Ver total gastado");
                Console.WriteLine("4. Actualizar un gasto (Actualizar)");
                Console.WriteLine("5. Eliminar un gasto (Borrar)");
                Console.WriteLine("6. Salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine() ?? string.Empty;

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            RegistrarGasto();
                            break;
                        case "2":
                            ListarGastos();
                            break;
                        case "3":
                            VerTotal();
                            break;
                        case "4":
                            ActualizarGasto();
                            break;
                        case "5":
                            EliminarGasto();
                            break;
                        case "6":
                            salir = true;
                            Console.WriteLine("\n¡Saliendo del programa!");
                            break;
                        default:
                            Console.WriteLine("\nOpción no válida. Intente nuevamente.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[Error Detectado]: {ex.Message}");
                    Console.ResetColor();
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static void InicializarBaseDatos()
        {
            using (var conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string comandoSql = @"
                    CREATE TABLE IF NOT EXISTS Gastos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Descripcion TEXT NOT NULL,
                        Monto REAL NOT NULL
                    );";
                using (var comando = new SqliteCommand(comandoSql, conexion))
                {
                    comando.ExecuteNonQuery();
                }
            }
        }

        private static void RegistrarGasto()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR NUEVO GASTO ---");
            Console.Write("¿En qué gastaste? (Ej. Comida, Ropa): ");
            string descripcion = Console.ReadLine() ?? string.Empty;

            Console.Write("Ingrese el monto: ");
            string entradaMonto = Console.ReadLine() ?? string.Empty;

            if (!decimal.TryParse(entradaMonto, out decimal monto))
            {
                throw new FormatException("Debe ingresar un número válido para el monto.");
            }

            // Validamos mediante la clase asegurando POO
            Gasto nuevoGasto = new Gasto
            {
                Descripcion = descripcion,
                Monto = monto
            };

            using (var conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string query = "INSERT INTO Gastos (Descripcion, Monto) VALUES (@desc, @monto)";
                using (var cmd = new SqliteCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@desc", nuevoGasto.Descripcion);
                    cmd.Parameters.AddWithValue("@monto", nuevoGasto.Monto);
                    cmd.ExecuteNonQuery();
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n¡Gasto registrado con éxito en la base de datos!");
            Console.ResetColor();
        }

        private static void ListarGastos()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE GASTOS ---");

            bool hayRegistros = false;
            using (var conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string query = "SELECT Id, Descripcion, Monto FROM Gastos";
                using (var cmd = new SqliteCommand(query, conexion))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            hayRegistros = true;
                            Gasto gasto = new Gasto
                            {
                                Id = reader.GetInt32(0),
                                Descripcion = reader.GetString(1),
                                Monto = reader.GetDecimal(2)
                            };
                            gasto.MostrarDetalle(); // Polimorfismo en acción
                        }
                    }
                }
            }

            if (!hayRegistros)
            {
                Console.WriteLine("No hay gastos registrados todavía.");
            }
        }

        private static void VerTotal()
        {
            Console.Clear();
            Console.WriteLine("--- TOTAL GASTADO ---");
            decimal total = 0;

            using (var conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string query = "SELECT SUM(Monto) FROM Gastos";
                using (var cmd = new SqliteCommand(query, conexion))
                {
                    var resultado = cmd.ExecuteScalar();
                    if (resultado != DBNull.Value && resultado != null)
                    {
                        total = Convert.ToDecimal(resultado);
                    }
                }
            }

            Console.WriteLine($"Llevas gastando un total de: ${total}");
        }

        private static void ActualizarGasto()
        {
            Console.Clear();
            Console.WriteLine("--- ACTUALIZAR GASTO ---");
            ListarGastos();

            Console.Write("\nIngrese el ID del gasto que desea modificar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                throw new FormatException("ID inválido.");
            }

            Console.Write("Nueva descripción: ");
            string nuevaDesc = Console.ReadLine() ?? string.Empty;

            Console.Write("Nuevo monto: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal nuevoMonto))
            {
                throw new FormatException("Monto inválido.");
            }

            // Aplicamos validación de POO
            Gasto gastoTemp = new Gasto { Descripcion = nuevaDesc, Monto = nuevoMonto };

            using (var conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string query = "UPDATE Gastos SET Descripcion = @desc, Monto = @monto WHERE Id = @id";
                using (var cmd = new SqliteCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@desc", gastoTemp.Descripcion);
                    cmd.Parameters.AddWithValue("@monto", gastoTemp.Monto);
                    cmd.Parameters.AddWithValue("@id", id);
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n¡Gasto actualizado correctamente!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("\nNo se encontró un gasto con ese ID.");
                    }
                }
            }
        }

        private static void EliminarGasto()
        {
            Console.Clear();
            Console.WriteLine("--- ELIMINAR GASTO ---");
            ListarGastos();

            Console.Write("\nIngrese el ID del gasto que desea eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                throw new FormatException("ID inválido.");
            }

            using (var conexion = new SqliteConnection(cadenaConexion))
            {
                conexion.Open();
                string query = "DELETE FROM Gastos WHERE Id = @id";
                using (var cmd = new SqliteCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n¡Gasto eliminado correctamente!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("\nNo se encontró un gasto con ese ID.");
                    }
                }
            }
        }
    }
}