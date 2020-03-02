using System;
using System.Threading.Tasks;
using Com.OneSignal;
using InformApp.Modelo;
using InformApp.Servico;
using InformApp.Utilidade;
using Xamarin.Forms;

namespace InformApp.Controle
{
    public partial class Principal
    {
        public static Repositorio Repositorio { get; set; }
        private static NavigationPage Navegacao { get; set; }

        public Principal()
        {
            InitializeComponent();

            Device.BeginInvokeOnMainThread(async () =>
            {
                MainPage = Constantes.Paginas[Constantes.TipoPagina.TelaInicial];

                DependencyService.Register<IArquivamento>();
                OneSignal.Current.StartInit(Constantes.AppId).HandleNotificationReceived(Eventos.NotificacaoRecebida).EndInit();

                Repositorio = new Repositorio();
                await Repositorio.Inicializar();
                await Task.Delay(100);

                var configuracao = await Repositorio.PegarAsync<Configuracao>(x => x.Tipo == Configuracao.TipoConfiguracao.AppConfigurado && x.Valor == Configuracao.TipoValor.Booleano && x.ValorBruto != null);
                var paginaInicial = configuracao != null && Convert.ToBoolean(configuracao.ValorBruto) ? Constantes.TipoPagina.HistoricoAvaliativo : Constantes.TipoPagina.PrimeiraTela;

                MainPage = Navegacao = new NavigationPage(Constantes.Paginas[paginaInicial]);
            });
        }

        public static async Task<bool> Mensagem(string titulo)
        {
            if (Current?.MainPage != null)
            {
                await Current.MainPage.DisplayAlert(string.Empty, titulo, "OK").ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public static async Task<bool> MudarPagina(Constantes.TipoPagina tipoPagina)
        {
            if (Navegacao != null)
            {
                var pagina = Constantes.Paginas[tipoPagina];
                var paginaAnterior = Navegacao.CurrentPage;

                NavigationPage.SetHasBackButton(pagina, false);
                await Navegacao.PushAsync(pagina);
                Navegacao.Navigation.RemovePage(paginaAnterior);
                return true;
            }

            return false;
        }

        protected override void OnStart()
        {
            OneSignal.Current.RegisterForPushNotifications();
            //OneSignal.Current.IdsAvailable(async (id, token) => await Mensagem($"ID: {id}\n\nToken: {token}"));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}