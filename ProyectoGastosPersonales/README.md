PROYECTO FINAL INTEGRADOR: CONTROL DE GASTOS PERSONALES (C#)

1. Introducción y Propuesta
- ¿Qué problema resuelve? Permite llevar un control claro y ordenado de los gastos cotidianos (en qué se gasta el dinero y cuál es el total acumulado).
- ¿Por qué es una buena solución? Es una aplicación de consola funcional, intuitiva, estructurada con Programación Orientada a Objetos, validaciones estrictas y manejo seguro de errores sin advertencias en .NET 10.

2. Documentación del Diseño y Requerimientos

2.1 Requerimientos Funcionales
- RF-01: El sistema deberá permitir registrar nuevos gastos con descripción y monto.
- RF-02: El sistema deberá permitir consultar la lista completa de gastos registrados.
- RF-03: El sistema deberá calcular y mostrar el total gastado.
- RF-04: El sistema deberá almacenar la información de manera persistente en memoria (durante la ejecución).

2.2 Requerimientos No Funcionales
- El sistema validará los datos de entrada para evitar montos negativos o vacíos.
- El sistema manejará excepciones con bloques try-catch para evitar cierres inesperados.
- El código mantendrá una estructura organizada aplicando los pilares de la POO.

2.3 Estructura de Clases (POO)
- TransaccionBase (Clase Abstracta): Define los atributos privados encapsulados _id y _descripcion, sus propiedades públicas con validación, y un método abstracto MostrarDetalle().
- Gasto (Clase Derivada): Hereda de TransaccionBase, añade el atributo privado _monto con validación de valores positivos, y aplica polimorfismo sobrescribiendo MostrarDetalle().
- Program (Clase Principal): Contiene la lógica del menú interactivo, la lista de almacenamiento dinámico y el control global de excepciones.

2.4 Validaciones y Manejo de Errores
- Validación de texto: Evita descripciones vacías o con espacios en blanco mediante string.IsNullOrWhiteSpace().
- Validación numérica: Previene errores de conversión al ingresar montos usando decimal.TryParse().
- Control de excepciones: Envuelve las operaciones del menú en un bloque try-catch que captura cualquier error y muestra un mensaje amigable en consola.