using Ada.Framework.Data.Notifications;
using Ada.Framework.Data.Notifications.TO;
using Ada.Framework.Development.Log4Me;
using Ada.Framework.Development.Log4Me.Entities;
using System;

namespace Ada.Framework.Util.Consoled
{
    public class ConsoledBase
    {
#if TEST
        public static Notificacion Notificacion = new Notificacion();
        public static Data.Notifications.TO.MensajeTO UltimoMensaje { get { return Notificacion.Mensajes[Notificacion.Mensajes.Count - 1]; } }
#endif

        public static void Debug(string mensaje, params string[] args)
        {
            Print(mensaje, Nivel.Debug, args);
        }

        public static void Alert(string mensaje, params string[] args)
        {
            Print(mensaje, Nivel.Alert, args);
        }

        public static void Error(string mensaje, params string[] args)
        {
            Print(mensaje, Nivel.Error, args);
        }

        public static void Fatal(string mensaje, params string[] args)
        {
            Print(mensaje, Nivel.Fatal, args);
        }

        public static void Info(string mensaje, params string[] args)
        {
            Print(mensaje, Nivel.Info, args);
        }

        public static void Success(string mensaje, params string[] args)
        {
            Print(mensaje, Nivel.Success, args);
        }

        public static void Show(Notificacion notificacion)
        {
            foreach (Data.Notifications.TO.MensajeTO mensaje in notificacion.Mensajes)
            {
                Show(mensaje);
            }
        }

        public static void Show(Data.Notifications.TO.MensajeTO mensaje)
        {
            if (mensaje.Tipo == Severidad.Alert)
            {
                Alert(mensaje.Glosa);
            }
            else if (mensaje.Tipo == Severidad.Error)
            {
                Error(mensaje.Glosa);
            }
            else if (mensaje.Tipo == Severidad.Info)
            {
                Info(mensaje.Glosa);
            }
            else if (mensaje.Tipo == Severidad.Success)
            {
                Success(mensaje.Glosa);
            }
            else
            {
                Print(mensaje.Glosa + "\n", Nivel.Debug);
            }
        }

        public static void Exit(bool error = false)
        {
            Exit(error ? 255 : 0);
        }

        public static void Exit(int exitCode)
        {
#if !TEST
            Info("Presione enter para salir...");
            Console.ReadKey();
            Environment.Exit(exitCode);
#endif
        }
        
        public static void Print(string mensaje, Nivel nivel, params object[] args)
        {
            Log4MeManager.CurrentInstance.Mensaje(string.Format(mensaje, args), nivel);
        }

        public static string Unir(string[] args, int startIndex = 0)
        {
            string retorno = string.Empty;

            for (int i = startIndex; i < args.Length; i++)
            {
                retorno += args[i] + " ";
            }

            return retorno;
        }
    }
}