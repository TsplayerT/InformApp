using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using InformApp.Controle;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace InformApp.Utilidade
{
    public static class Miscelanea
    {
        public static void DefinirAtributosPai(Type tipo)
        {
            if (tipo == null)
            {
                Device.BeginInvokeOnMainThread(async () => await Principal.Mensagem($"Não foi possível definir os atributos no método {nameof(DefinirAtributosPai)} porque o parâmetro essencial do tipo {typeof(Type)} chamado {nameof(tipo)} está nulo"));

                return;
            }

            var listaAtributos = tipo.GetProperties().SelectMany(x => x.GetCustomAttributes(true)).OfType<Attribute>().ToList();
            var objetosImplementados = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(tipo.IsAssignableFrom);

            objetosImplementados.ForEach(x => TypeDescriptor.AddAttributes(x, listaAtributos.ToArray()));
        }

        public static Type[] PegarTodasClassesEmNamespace(string diretorio)
        {
            bool Funcao(Type x) => !string.IsNullOrEmpty(x.Namespace) && x.Namespace.Equals(diretorio, StringComparison.OrdinalIgnoreCase) && !x.IsEnum && !x.IsInterface;
            return Assembly.GetExecutingAssembly().GetTypes().Where(Funcao).ToArray();
        }
    }
}
