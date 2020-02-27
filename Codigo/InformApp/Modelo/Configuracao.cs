using Newtonsoft.Json;
using SQLite;

namespace InformApp.Modelo
{
    public class Configuracao : _ModeloBase
    {
        public int TipoConfiguracaoId { get; set; }
        public int TipoValorId { get; set; }

        [Ignore]
        public object ValorBruto
        {
            get => JsonConvert.DeserializeObject(ValorBrutoSerializado ?? string.Empty);
            set => ValorBrutoSerializado = JsonConvert.SerializeObject(value ?? string.Empty);
        }
        public string ValorBrutoSerializado { get; set; }

        [Ignore]
        public TipoConfiguracao Tipo
        {
            get => (TipoConfiguracao)TipoConfiguracaoId;
            set => TipoConfiguracaoId = (int)value;
        }
        [Ignore]
        public TipoValor Valor
        {
            get => (TipoValor)TipoValorId;
            set => TipoValorId = (int)value;
        }

        public enum TipoConfiguracao
        {
            Nenhum = 0,
            AppConfigurado = 1
        }

        public enum TipoValor
        {
            Nenhum = 0,
            Booleano = 1,
            Inteiro = 2,
            Texto = 3
        }
    }
}
