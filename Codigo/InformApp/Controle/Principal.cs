using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using InformApp.Modelo;
using InformApp.Servico;
using InformApp.Utilidade;
using Xamarin.Forms;

namespace InformApp.Controle
{
    public partial class Principal
    {
        public static Dictionary<Constantes.TipoAcao, Action> AcoesNotificacaoRecebida { get; set; }
        public static Repositorio Repositorio { get; set; }
        private static NavigationPage Navegacao { get; set; }

        public Principal()
        {
            InitializeComponent();

            Parallel.Invoke(() =>
            {
                MainPage = new ContentPage();
                AcoesNotificacaoRecebida = new Dictionary<Constantes.TipoAcao, Action>();
            },
            async () =>
            {
                Repositorio = new Repositorio();

                var configuracao = await Repositorio.PegarAsync<Configuracao>(x => x.Tipo == Configuracao.TipoConfiguracao.AppConfigurado && x.Valor == Configuracao.TipoValor.Booleano && x.ValorBruto != null);
                var paginaInicial = configuracao != null && Convert.ToBoolean(configuracao.ValorBruto) ? Constantes.TipoPagina.HistoricoAvaliativo : Constantes.TipoPagina.PrimeiraTela;

                await Device.InvokeOnMainThreadAsync(() => MainPage = Navegacao = new NavigationPage(Constantes.Paginas[paginaInicial]));
            },
            () => OneSignal.Current.StartInit(Constantes.AppId).HandleNotificationReceived(NotificacaoRecebida).EndInit());
        }

        private async void NotificacaoRecebida(OSNotification result)
        {
            var notificacao = result.payload;

            if (Repositorio != null && notificacao != null)
            {
                await Repositorio.AdicionarOuAtualizarAsync(new Notificacao
                {
                    Identificacao = notificacao.notificationID,
                    Titulo = notificacao.title,
                    Mensagem = notificacao.body,
                    DataAgendada = DateTime.Now.ToString("HH:mm:ss")
                });

                AcoesNotificacaoRecebida.Where(x => x.Value != null).OrderBy(x => x.Key).Select(x => x.Value).ToList().ForEach(x => x.Invoke());
            }
            else
            {
                await Mensagem($"Não foi possível adicionar {nameof(Notificacao)}!");
            }
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