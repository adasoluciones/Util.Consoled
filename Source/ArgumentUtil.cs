using Ada.Framework.Util.Consoled.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Ada.Framework.Util.Consoled
{
    public static class ArgumentUtil
    {
        public static string ObtenerValor(string texto, out int stopIndex, int startIndex = 0, params char[] separadores)
        {
            string retorno = null;
            bool saltarEspacios = false;
            char caracterSaltoEspacio = (char)0;
            stopIndex = startIndex;

            for (int i = startIndex; i < texto.Length; i++)
            {
                char caracter = texto[i];

                if (caracterSaltoEspacio == 0 && caracter == '\'' || caracter == '\"') caracterSaltoEspacio = caracter;

                if (caracter == caracterSaltoEspacio)
                {
                    saltarEspacios = !saltarEspacios;
                    if (!saltarEspacios) caracterSaltoEspacio = (char)0;
                    continue;
                }
                                
                if (!saltarEspacios && separadores.Contains(caracter))
                {
                    stopIndex = i;
                    break;
                }

                if (!saltarEspacios && caracter == ' ')
                {
                    retorno = null;
                    stopIndex = startIndex;
                    break;
                }
                
                retorno += caracter;
            }

            return retorno;
        }

        public static KeyValuePair<string, string>? ObtenerPar(string texto, out int stopIndex, int startIndex = 0)
        {
            string key = ObtenerValor(texto, out stopIndex, startIndex, '=');
            if (key == null) return null;
            string value = ObtenerValor(texto, out stopIndex, stopIndex + 1, ',', ' ');
            return new KeyValuePair<string, string>(key, value);
        }

        public static IDictionary<string, string> ObtenerPropiedades(string texto, out int stopIndex, int startIndex = 0, params char[] separadores)
        {
            IDictionary<string, string> retorno = new Dictionary<string, string>();
            stopIndex = startIndex;

            KeyValuePair<string, string>? par = new KeyValuePair<string, string>();

            while (true)
            {
                par = ObtenerPar(texto, out stopIndex, (stopIndex == startIndex ? stopIndex : stopIndex + 1));

                if (par != null && par.HasValue)
                {
                    retorno.Add(par.Value);
                }
                else break;
            }
            
            return retorno;
        }

        public static IDictionary<string, IDictionary<string, string>> ObtenerArgumentos(string argumentos, out int stopIndex, int startIndex = 0)
        {
            IDictionary<string, IDictionary<string, string>> retorno = new Dictionary<string, IDictionary<string, string>>();
            
            string argumento = string.Empty;

            bool finalizarParametro = false;
            bool saltarEspacios = false;
            bool argumentoEncontrado = false;

            stopIndex = startIndex;

            for (int i = startIndex; i < argumentos.Length; i++)
            {
                char caracter = argumentos[i];

                if (!argumentoEncontrado && caracter != '-')
                {
                    break;
                }

                if (!argumentoEncontrado && caracter == '-')
                {
                    argumentoEncontrado = true;
                    continue;
                }

                if (!retorno.ContainsKey(argumento) && caracter == ':')
                {
                    retorno.Add(argumento, new Dictionary<string, string>());
                }

                if (caracter == ':' || (!saltarEspacios && caracter == ' '))
                {
                    finalizarParametro = true;
                }

                if (caracter == '\'' || caracter == '\"')
                {
                    if (!saltarEspacios)
                    {
                        saltarEspacios = true;
                    }
                    continue;
                }

                if (finalizarParametro)
                {
                    retorno[argumento] = ObtenerPropiedades(argumentos, out stopIndex,i + 1, ',');

                    if (caracter == ':' && retorno[argumento].Count == 0)
                    {
                        string valor = ObtenerValor(argumentos, out stopIndex, i + 1, ' ');
                        retorno[argumento].Add("value", valor);
                        stopIndex += 1;
                    }

                    argumento = string.Empty;
                    i = stopIndex - 1;

                    saltarEspacios = false;
                    finalizarParametro = false;
                    argumentoEncontrado = false;
                }
                else
                {
                    argumento += caracter;
                }
            }

            return retorno;
        }

        public static IList<ComandoTO> ObtenerComandos(string argumentosCLI, out int stopIndex, int startIndex = 0)
        {
            IList<ComandoTO> retorno = new List<ComandoTO>();
            stopIndex = startIndex;
            
            string nombreComando = null;

            for (int i = startIndex; i < argumentosCLI.Length; i++)
            {
                nombreComando = ObtenerValor(argumentosCLI, out stopIndex, i, ' ');
                
                ComandoTO comando = new ComandoTO();
                comando.Nombre = nombreComando;
                
                comando.Propiedades = ObtenerPropiedades(argumentosCLI, out stopIndex, stopIndex + 1);
                comando.Argumentos = ArgumentoTO.ToArgumentos(ObtenerArgumentos(argumentosCLI, out stopIndex, stopIndex));
                
                comando.Argumentos.Concat(ConsoledProgram.ArgumentosComunes);
                comando.Propiedades.Concat(ConsoledProgram.PropiedadesComunes);

                retorno.Add(comando);
                i = stopIndex - 1;
            }
            
            return retorno;
        }
    }
}
