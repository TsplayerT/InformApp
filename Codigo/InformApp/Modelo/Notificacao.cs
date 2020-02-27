using SQLite;

namespace InformApp.Modelo
{
    public class Notificacao : _ModeloBase
    {
        public string Identificacao { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string DataAgendada { get; set; }
        public bool? Resposta { get; set; }

        [Ignore]
        public bool RespostaVisivel => Resposta != null;
        [Ignore]
        public bool EscolherRespostaVisivel => Resposta == null;
        [Ignore]
        public string RespostaTratada
        {
            get
            {
                switch (Resposta)
                {
                    case true:
                        return "Positivo";
                    case false:
                        return "Negativo";
                    default:
                        return null;
                }
            }
        }
    }
}
