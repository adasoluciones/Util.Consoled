using System.Collections.Generic;

namespace Ada.Framework.Util.Consoled.Entities
{
    public class ComandoTO
    {
        public string Nombre { get; set; }
        public IDictionary<string, string> Propiedades { get; set; }
        public IList<ArgumentoTO> Argumentos { get; set; }
        public IList<ComandoTO> Dependencias { get; set; }

        public ComandoTO()
        {
            Argumentos = new List<ArgumentoTO>();
            Dependencias = new List<ComandoTO>();
            Propiedades = new Dictionary<string, string>();
        }

        public void Cargar(ComandoTO comando)
        {
            Nombre = comando.Nombre;
            Propiedades = comando.Propiedades;
            Argumentos = comando.Argumentos;
            Dependencias = comando.Dependencias;
        }
    }
}
