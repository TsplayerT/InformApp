using System;
using System.Threading.Tasks;
using InformApp.Modelo;
using InformApp.Utilidade;
using Xamarin.Forms;

namespace InformApp.Controle.Pagina
{
    public partial class PrimeiraTela
    {
        public static BindableProperty CarregandoProperty = BindableProperty.Create(nameof(Carregando), typeof(bool), typeof(PrimeiraTela), default(bool), propertyChanged: CarregandoPropertyChanged);
        public bool Carregando
        {
            get => (bool)GetValue(CarregandoProperty);
            set => SetValue(CarregandoProperty, value);
        }

        public static BindableProperty NumeroIdentificacaoHabilitadoProperty = BindableProperty.Create(nameof(NumeroIdentificacaoHabilitado), typeof(bool), typeof(PrimeiraTela), true);
        public bool NumeroIdentificacaoHabilitado
        {
            get => (bool)GetValue(NumeroIdentificacaoHabilitadoProperty);
            set => SetValue(NumeroIdentificacaoHabilitadoProperty, value);
        }
        public static BindableProperty CodigoAutenticacaoHabilitadoProperty = BindableProperty.Create(nameof(CodigoAutenticacaoHabilitado), typeof(bool), typeof(PrimeiraTela), true);
        public bool CodigoAutenticacaoHabilitado
        {
            get => (bool)GetValue(CodigoAutenticacaoHabilitadoProperty);
            set => SetValue(CodigoAutenticacaoHabilitadoProperty, value);
        }
        public static BindableProperty BotaoSelecionarArquivoHabilitadoProperty = BindableProperty.Create(nameof(BotaoSelecionarArquivoHabilitado), typeof(bool), typeof(PrimeiraTela), true, propertyChanged: BotaoSelecionarArquivoHabilitadoPropertyChanged);
        public bool BotaoSelecionarArquivoHabilitado
        {
            get => (bool)GetValue(BotaoSelecionarArquivoHabilitadoProperty);
            set => SetValue(BotaoSelecionarArquivoHabilitadoProperty, value);
        }
        public static BindableProperty BotaoVerificarHabilitadoProperty = BindableProperty.Create(nameof(BotaoVerificarHabilitado), typeof(bool), typeof(PrimeiraTela), default(bool), propertyChanged: BotaoVerificarHabilitadoPropertyChanged);
        public bool BotaoVerificarHabilitado
        {
            get => (bool)GetValue(BotaoVerificarHabilitadoProperty);
            set => SetValue(BotaoVerificarHabilitadoProperty, value);
        }

        public static BindableProperty BotaoSelecionarArquivoFundoAtualProperty = BindableProperty.Create(nameof(BotaoSelecionarArquivoFundoAtual), typeof(Color), typeof(PrimeiraTela), Color.FromHex("#c8c8c8"));
        public Color BotaoSelecionarArquivoFundoAtual
        {
            get => (Color)GetValue(BotaoSelecionarArquivoFundoAtualProperty);
            set => SetValue(BotaoSelecionarArquivoFundoAtualProperty, value);
        }
        public static BindableProperty BotaoVerificarFundoAtualProperty = BindableProperty.Create(nameof(BotaoVerificarFundoAtual), typeof(Color), typeof(PrimeiraTela), Color.FromHex("#7d7dfa"));
        public Color BotaoVerificarFundoAtual
        {
            get => (Color)GetValue(BotaoVerificarFundoAtualProperty);
            set => SetValue(BotaoVerificarFundoAtualProperty, value);
        }

        public static BindableProperty BotaoSelecionarArquivoFundoPadraoProperty = BindableProperty.Create(nameof(BotaoSelecionarArquivoFundoPadrao), typeof(Color), typeof(PrimeiraTela), Color.FromHex("#c8c8c8"), propertyChanged: BotaoSelecionarArquivoFundoPadraoPropertyChanged);
        public Color BotaoSelecionarArquivoFundoPadrao
        {
            get => (Color)GetValue(BotaoSelecionarArquivoFundoPadraoProperty);
            set => SetValue(BotaoSelecionarArquivoFundoPadraoProperty, value);
        }
        public static BindableProperty BotaoVerificarFundoPadraoProperty = BindableProperty.Create(nameof(BotaoVerificarFundoPadrao), typeof(Color), typeof(PrimeiraTela), Color.FromHex("#7d7dfa"), propertyChanged: BotaoVerificarFundoPadraoPropertyChanged);
        public Color BotaoVerificarFundoPadrao
        {
            get => (Color)GetValue(BotaoVerificarFundoPadraoProperty);
            set => SetValue(BotaoVerificarFundoPadraoProperty, value);
        }

        public static BindableProperty NumeroIdentificacaoProperty = BindableProperty.Create(nameof(NumeroIdentificacao), typeof(string), typeof(PrimeiraTela), string.Empty, propertyChanged: CampoPropertyChanged);
        public string NumeroIdentificacao
        {
            get => (string)GetValue(NumeroIdentificacaoProperty);
            set => SetValue(NumeroIdentificacaoProperty, value);
        }
        public static BindableProperty CodigoAutenticacaoProperty = BindableProperty.Create(nameof(CodigoAutenticacao), typeof(string), typeof(PrimeiraTela), string.Empty, propertyChanged: CampoPropertyChanged);
        public string CodigoAutenticacao
        {
            get => (string)GetValue(CodigoAutenticacaoProperty);
            set => SetValue(CodigoAutenticacaoProperty, value);
        }

        public PrimeiraTela()
        {
            InitializeComponent();

            BindingContext = this;
        }

        private async void BotaoSelecionarArquivo_OnClicked(object sender, EventArgs e)
        {
            Carregando = true;
            await Task.Delay(2000);
            Carregando = false;
        }

        private async void BotaoVerificar_OnClicked(object sender, EventArgs e)
        {
            await Principal.Repositorio.AdicionarOuAtualizarAsync(new Configuracao
            {
                Tipo = Configuracao.TipoConfiguracao.AppConfigurado,
                Valor = Configuracao.TipoValor.Booleano,
                ValorBruto = true
            });
            await Principal.MudarPagina(Constantes.TipoPagina.HistoricoAvaliativo);
        }

        private static void CampoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is PrimeiraTela objeto)
            {
                objeto.BotaoVerificarHabilitado = !(string.IsNullOrEmpty(objeto.CodigoAutenticacao) || string.IsNullOrEmpty(objeto.NumeroIdentificacao));
            }
        }
        private static void CarregandoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is PrimeiraTela objeto && newvalue is bool carregando)
            {
                var habilitado = !carregando;

                objeto.BotaoVerificarHabilitado = habilitado;
                objeto.BotaoSelecionarArquivoHabilitado = habilitado;
                objeto.NumeroIdentificacaoHabilitado = habilitado;
                objeto.CodigoAutenticacaoHabilitado = habilitado;
            }
        }
        private static void BotaoVerificarHabilitadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is PrimeiraTela objeto && newvalue is bool habilitado)
            {
                objeto.BotaoVerificarFundoAtual = habilitado ? objeto.BotaoVerificarFundoPadrao : Constantes.CorDesabilitado;
            }
        }
        private static void BotaoSelecionarArquivoHabilitadoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is PrimeiraTela objeto && newvalue is bool habilitado)
            {
                objeto.BotaoSelecionarArquivoFundoAtual = habilitado ? objeto.BotaoSelecionarArquivoFundoPadrao : Constantes.CorDesabilitado;
            }
        }
        private static void BotaoSelecionarArquivoFundoPadraoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is PrimeiraTela objeto && newvalue is Color fundoPadrao)
            {
                objeto.BotaoSelecionarArquivoFundoAtual = fundoPadrao;
            }
        }
        private static void BotaoVerificarFundoPadraoPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is PrimeiraTela objeto && newvalue is Color fundoPadrao)
            {
                objeto.BotaoVerificarFundoAtual = fundoPadrao;
            }
        }
    }
}
