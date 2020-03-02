using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using InformApp.Controle.Pagina;
using Xamarin.Forms;

namespace InformApp.Utilidade
{
    public static class Constantes
    {
        public static string AppId => "065fc6dd-75b5-4b07-84e6-98ad139aec9b";
        public static ImageSource LogoImagemSource => ImageSource.FromResource("InformApp.Recurso.Imagem.logo.png", typeof(Constantes).GetTypeInfo().Assembly);

        public static Color CorDesabilitado => Color.FromHex("#afafaf");

        public enum TipoPagina
        {
            Nenhum,
            TelaInicial,
            PrimeiraTela,
            HistoricoAvaliativo
        }

        public enum TipoAcao
        {
            Nenhum,
            AtualizarLista
        }

        public static Dictionary<TipoPagina, Page> Paginas => new Dictionary<TipoPagina, Page>
        {
            { TipoPagina.TelaInicial, new Lazy<TelaInicial>(LazyThreadSafetyMode.ExecutionAndPublication).Value },
            { TipoPagina.PrimeiraTela, new Lazy<PrimeiraTela>(LazyThreadSafetyMode.ExecutionAndPublication).Value },
            { TipoPagina.HistoricoAvaliativo, new Lazy<HistoricoAvaliativo>(LazyThreadSafetyMode.ExecutionAndPublication).Value }
        };
    }
}
