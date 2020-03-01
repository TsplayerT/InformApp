using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using InformApp.Modelo;
using InformApp.Servico;
using InformApp.Utilidade;
using Xamarin.Forms;

namespace InformApp.Controle.Pagina
{
    public partial class HistoricoAvaliativo
    {
        public static BindableProperty ListaNotificacaoProperty = BindableProperty.Create(nameof(ListaNotificacao), typeof(ObservableCollection<Notificacao>), typeof(HistoricoAvaliativo), default(ObservableCollection<Notificacao>));
        public ObservableCollection<Notificacao> ListaNotificacao
        {
            get => (ObservableCollection<Notificacao>)GetValue(ListaNotificacaoProperty);
            set => SetValue(ListaNotificacaoProperty, value);
        }
        public static BindableProperty CarregandoProperty = BindableProperty.Create(nameof(Carregando), typeof(bool), typeof(HistoricoAvaliativo), true);
        public bool Carregando
        {
            get => (bool)GetValue(CarregandoProperty);
            set => SetValue(CarregandoProperty, value);
        }
        public static BindableProperty NenhumItemProperty = BindableProperty.Create(nameof(NenhumItem), typeof(bool), typeof(HistoricoAvaliativo), default(bool));
        public bool NenhumItem
        {
            get => (bool)GetValue(NenhumItemProperty);
            set => SetValue(NenhumItemProperty, value);
        }
        public static BindableProperty AcaoAtualizarListaProperty = BindableProperty.Create(nameof(AcaoAtualizarLista), typeof(Action), typeof(HistoricoAvaliativo), default(Action), defaultValueCreator: AcaoAtualizarListaDefaultValueCreator);
        public Action AcaoAtualizarLista
        {
            get => (Action)GetValue(AcaoAtualizarListaProperty);
            set => SetValue(AcaoAtualizarListaProperty, value);
        }

        public HistoricoAvaliativo()
        {
            InitializeComponent();

            Appearing += delegate
            {
                AcaoAtualizarLista.Invoke();
            };

            BindingContext = this;
        }

        private async void BotaoGostei_OnClicked(object sender, EventArgs e)
        {
            if (sender is Button botao && botao.BindingContext is Notificacao notificacao)
            {
                var index = ListaNotificacao.IndexOf(notificacao);

                notificacao.Resposta = true;

                await Principal.Repositorio.AdicionarOuAtualizarAsync(notificacao);

                if (index >= 0)
                {
                    ListaNotificacao[index] = notificacao;
                }
            }
        }

        private async void BotaoNaoGostei_OnClicked(object sender, EventArgs e)
        {
            if (sender is Button botao && botao.BindingContext is Notificacao notificacao)
            {
                var index = ListaNotificacao.IndexOf(notificacao);

                notificacao.Resposta = false;

                await Principal.Repositorio.AdicionarOuAtualizarAsync(notificacao);

                if (index >= 0)
                {
                    ListaNotificacao[index] = notificacao;
                }
            }
        }

        private static object AcaoAtualizarListaDefaultValueCreator(BindableObject bindable)
        {
            if (bindable is HistoricoAvaliativo objeto)
            {
                var acao = new Action(async () =>
                {
                    var lista = await Principal.Repositorio.ListarAsync<Notificacao>();

                    objeto.ListaNotificacao = new ObservableCollection<Notificacao>(lista?.OrderBy(x => x.DataAgendada).ToList() ?? new List<Notificacao>());

                    objeto.Carregando = false;
                    objeto.NenhumItem = !objeto.ListaNotificacao.Any();
                });

                if (!Eventos.AcoesNotificacaoRecebida.ContainsKey(Constantes.TipoAcao.AtualizarLista))
                {
                    Eventos.AcoesNotificacaoRecebida.Add(Constantes.TipoAcao.AtualizarLista, acao);
                }

                return acao;
            }

            return default;
        }

    }
}
