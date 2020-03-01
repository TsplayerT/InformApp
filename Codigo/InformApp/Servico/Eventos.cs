using System;
using System.Collections.Generic;
using System.Linq;
using Com.OneSignal.Abstractions;
using InformApp.Controle;
using InformApp.Modelo;
using InformApp.Utilidade;

namespace InformApp.Servico
{
    public static class Eventos
    {
        static Eventos()
        {
            AcoesNotificacaoRecebida = new Dictionary<Constantes.TipoAcao, Action>();
        }

        public static Dictionary<Constantes.TipoAcao, Action> AcoesNotificacaoRecebida { get; }

        public static async void NotificacaoRecebida(OSNotification result)
        {
            var notificacao = result.payload;

            if (Principal.Repositorio != null && notificacao != null)
            {
                await Principal.Repositorio.AdicionarOuAtualizarAsync(new Notificacao
                {
                    Identificacao = notificacao.notificationID,
                    Titulo = notificacao.title,
                    Mensagem = notificacao.body,
                    DataAgendada = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                });

                AcoesNotificacaoRecebida.Where(x => x.Value != null).OrderBy(x => x.Key).Select(x => x.Value).ToList().ForEach(x => x.Invoke());
            }
            else
            {
                await Principal.Mensagem($"Não foi possível adicionar {nameof(Notificacao)}!");
            }
        }

    }
}
