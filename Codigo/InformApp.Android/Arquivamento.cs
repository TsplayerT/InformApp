using InformApp.Droid;
using InformApp.Servico;

[assembly: Xamarin.Forms.Dependency(typeof(Arquivamento))]
namespace InformApp.Droid
{
    public class Arquivamento : IArquivamento
    {
        public string PegarDiretorio() => Android.App.Application.Context.GetExternalFilesDir(string.Empty).Path;
    }
}