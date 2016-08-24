using Ada.Framework.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace Ada.Framework.Util.Consoled.Entities
{
    public class ArgumentoTO
    {
        public string Nombre { get; set; }
        public IDictionary<string, string> Propiedades { get; set; }

        public ArgumentoTO()
        {
            Propiedades = new Dictionary<string, string>();
        }
        
        public static ArgumentoTO ToArgumento(string nombre, IDictionary<string, string> propiedades)
        {
            if (nombre == null) throw new ArgumentException(ArgumentoInvalido.VACIO.ToString(), "nombre");

            ArgumentoTO retorno = new ArgumentoTO();
            retorno.Nombre = nombre;

            if (propiedades != null)
            {
                retorno.Propiedades = propiedades;
            }
            else
            {
                retorno.Propiedades.Clear();
            }

            return retorno;
        }

        public static IList<ArgumentoTO> ToArgumentos(IDictionary<string, IDictionary<string, string>> argumentos)
        {
            IList<ArgumentoTO> retorno = new List<ArgumentoTO>();

            foreach (KeyValuePair<string, IDictionary<string, string>> par in argumentos)
            {
                retorno.Add(ToArgumento(par.Key, par.Value));
            }

            return retorno;
        }
    }
}
