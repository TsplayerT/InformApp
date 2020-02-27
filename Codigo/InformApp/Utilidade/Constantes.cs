using System.Collections.Generic;
using System.Reflection;
using InformApp.Controle.Pagina;
using Xamarin.Forms;

namespace InformApp.Utilidade
{
    public static class Constantes
    {
        public static string AppId => "065fc6dd-75b5-4b07-84e6-98ad139aec9b";
        public static ImageSource LogoImagemSource => ImageSource.FromResource("InformApp.Recurso.Imagem.logo.png", typeof(Constantes).GetTypeInfo().Assembly);

        /*
         
        <Image
            Margin="0, -27, 0, 0"
            Scale="0.5"
            Source="{x:Static utilidade:Constantes.LogoImagemSource}"/>

         */

        public static Color CorDesabilitado => Color.FromHex("#afafaf");

        public enum TipoPagina
        {
            Nenhum,
            PrimeiraTela,
            HistoricoAvaliativo
        }

        public enum TipoAcao
        {
            Nenhum,
            AtualizarLista
        }

        public static Dictionary<TipoPagina, Page> Paginas = new Dictionary<TipoPagina, Page>
        {
            { TipoPagina.PrimeiraTela, new PrimeiraTela() },
            { TipoPagina.HistoricoAvaliativo, new HistoricoAvaliativo() },
        };
    }
}
