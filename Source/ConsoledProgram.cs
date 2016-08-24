using Ada.Framework.Util.Consoled.Entities;
using System.Collections.Generic;

namespace Ada.Framework.Util.Consoled
{
    public abstract class ConsoledProgram : ConsoledBase
    {
        public static string ArgumentosCLI { get; private set; }
        public static IDictionary<string, string> PropiedadesComunes = new Dictionary<string, string>();
        public static IList<ArgumentoTO> ArgumentosComunes = new List<ArgumentoTO>();
        public static IList<ComandoTO> Comandos = new List<ComandoTO>();
        
        public static void CargarArgumentosConsola(string[] args)
        {
            ArgumentosCLI = Unir(args);
            
            int stopIndex = 0;
            ArgumentosComunes = ArgumentoTO.ToArgumentos(ArgumentUtil.ObtenerArgumentos(ArgumentosCLI, out stopIndex, 0));
            PropiedadesComunes = ArgumentUtil.ObtenerPropiedades(ArgumentosCLI, out stopIndex, stopIndex);
            Comandos = ArgumentUtil.ObtenerComandos(ArgumentosCLI, out stopIndex, stopIndex);
        }
        
    }
}
