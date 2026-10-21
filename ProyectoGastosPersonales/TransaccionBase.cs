using System;

namespace ProyectoGastosPersonales
{
    public abstract class TransaccionBase
    {
        private int _id;
        private string _descripcion = string.Empty;

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
}