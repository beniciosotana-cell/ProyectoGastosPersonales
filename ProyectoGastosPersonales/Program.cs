using System;
using System.Collections.Generic;

namespace ProyectoGastosPersonales
{
    // ==========================================
    // 1. CLASE BASE (Abstracción y Encapsulación)
    // ==========================================
    public abstract class TransaccionBase
    {
        private int _id;
        private string _descripcion = string.Empty; // Soluciona CS8618 inicializándolo por defecto

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La descripción no puede estar vacía.");
                _descripcion = value;
            }
        }

        public abstract void MostrarDetalle();
    }

    // ==========================================
    // 2. CLASE DERIVADA (Herencia)
    // ==========================================
    public class Gasto : TransaccionBase
    {
        private decimal _monto;

        public decimal Monto
        {
            get { return _monto; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El monto debe ser mayor a cero.");
                _monto = value;
            }
        }

        // Polimorfismo: Sobrescribiendo el método abstracto
        public override void MostrarDetalle()
        {
            Console.WriteLine($"ID: {Id} | Gasto: {Descripcion} | Monto: ${Monto}");
        }
    }

    // ==========================================
    // 3. CLASE PRINCIPAL Y LÓGICA DEL SISTEMA
    // ==========================================
    class Program
    {
        static List<Gasto> listaGastos = new List<Gasto>();
        static int contadorId = 1;

        static void Main(string[] args)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== CONTROL DE GASTOS PERSONALES ===");
                Console.WriteLine("1. Registrar nuevo gasto");
                Console.WriteLine("2. Ver lista de gastos");
                Console.WriteLine("3. Ver total gastado");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");

                // Soluciona CS8600 y CS8601 asegurando que si es nulo use un texto vacío
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
                            salir = true;
                            Console.WriteLine("¡Saliendo del programa. Mucho éxito con tu presentación!");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente nuevamente.");
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

        static void RegistrarGasto()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR NUEVO GASTO ---");

            Console.Write("¿En qué gastaste? (Ej. Comida, Salida, Ropa): ");
            // Soluciona CS8600 para la entrada por consola
            string descripcion = Console.ReadLine() ?? string.Empty;

            Console.Write("Ingrese el monto: ");
            string entradaMonto = Console.ReadLine() ?? string.Empty;

            if (!decimal.TryParse(entradaMonto, out decimal monto))
            {
                throw new FormatException("Debe ingresar un número válido para el monto.");
            }

            Gasto nuevoGasto = new Gasto();
            nuevoGasto.Id = contadorId++;
            nuevoGasto.Descripcion = descripcion;
            nuevoGasto.Monto = monto;

            listaGastos.Add(nuevoGasto);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n¡Gasto registrado con éxito!");
            Console.ResetColor();
        }

        static void ListarGastos()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE GASTOS ---");

            if (listaGastos.Count == 0)
            {
                Console.WriteLine("No hay gastos registrados todavía.");
                return;
            }

            foreach (var gasto in listaGastos)
            {
                gasto.MostrarDetalle();
            }
        }

        static void VerTotal()
        {
            Console.Clear();
            Console.WriteLine("--- TOTAL GASTADO ---");

            decimal total = 0;
            foreach (var gasto in listaGastos)
            {
                total += gasto.Monto;
            }

            Console.WriteLine($"Llevas gastando un total de: ${total}");
        }
    }
}