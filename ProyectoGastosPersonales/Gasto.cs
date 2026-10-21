using System;

namespace ProyectoGastosPersonales
{
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

        public override void MostrarDetalle()
        {
            Console.WriteLine($"ID: {Id} | Gasto: {Descripcion} | Monto: ${Monto}");
        }
    }
}