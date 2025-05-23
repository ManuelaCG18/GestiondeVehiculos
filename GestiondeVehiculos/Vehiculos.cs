﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestiondeVehiculos
{
    public abstract class Vehiculos
    {
        public string Modelo { get; private set; }
        public string Marca { get; private set; }
        public double PrecioBase { get; private set; }

        protected Vehiculos(string modelo, string marca, double precioBase)
        {
            if (string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(marca))
            {

                throw new ArgumentException("El modelo y la marca no pueden estar vacios, son obligatorios :(");
            }
            if (precioBase < 0)
            {
                throw new ArgumentException("El precio es obligatorio, y debe ser mayor a cero");
            }

            Modelo = modelo;
            Marca = marca;
            PrecioBase = precioBase;
        }

        public abstract double CalcularPrecioFinal();
        public abstract double CalcularCargoExtra();
        public abstract double CalcularImpuesto();
    }

    public class Automovil : Vehiculos
    {
        public Automovil(string modelo, string marca, double precioBase) : base(modelo, marca, precioBase) { }

        public override double CalcularPrecioFinal()
        {
            return PrecioBase + CalcularImpuesto();
        }

        public override double CalcularCargoExtra()
        {
            return 0;
        }

        public override double CalcularImpuesto()
        {
            return PrecioBase > 50000000 ? PrecioBase * 0.10 : 0;
        }

    }

    public class Motocicleta : Vehiculos
    {
        public Motocicleta(string modelo, string marca, double precioBase) : base(modelo, marca, precioBase) { }

        public override double CalcularPrecioFinal()
        {
            return PrecioBase + CalcularCargoExtra();
        }

        public override double CalcularCargoExtra()
        {
            return PrecioBase > 15000000 ? 500000 : 0;
        }

        public override double CalcularImpuesto()
        {
            return 0;
        }
    }

    // factory para la creacion de vehiculos
    public class VehiculosFactory
    {
        public static Vehiculos CrearVehiculo(string tipo, string modelo, string marca, double precioBase)
        {
            switch (tipo.ToLower())
            {
                case "automovil":
                    return new Automovil(modelo, marca, precioBase);
                case "motocicleta":
                    return new Motocicleta(modelo, marca, precioBase);
                default:
                    throw new ArgumentException("Tipo de vehiculo no valido :( ");
            }
        }
    }

    // singleton para manejar lista de vehiculos
    public class GestionVehiculos
    {
        private static GestionVehiculos _instancia;
        private static readonly object _lock = new object();
        
        public List<Vehiculos> Vehiculos { get; private set; }

        private GestionVehiculos()
        {
            Vehiculos = new List<Vehiculos>();
        }

        public static GestionVehiculos ObtenerInstancia()
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new GestionVehiculos();
                    }
                }
            }
            return _instancia;
        }

        public void AgregarVehiculo(Vehiculos vehiculo)
        {
            Vehiculos.Add(vehiculo);
        }
    }
}
