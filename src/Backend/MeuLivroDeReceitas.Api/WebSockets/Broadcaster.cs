using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;

namespace MeuLivroDeReceitas.Api.WebSockets
{
    public class Broadcaster
    {
        private readonly static Lazy<Broadcaster> _instance = new(() => new Broadcaster());

        public static Broadcaster Instance { get { return _instance.Value; } }

        private ConcurrentDictionary<string, object> _dictionary { get; set; }

        public Broadcaster()
        {
            _dictionary = new ConcurrentDictionary<string, object>();
        }

        public void InicializarConexao(IHubContext<AdicionarConexao> hubContext, long idUsuarioQueGerouQrCode, string connectionId)
        {
            var conexao = new Conexao(hubContext, connectionId);

            _dictionary.TryAdd(connectionId, conexao);
            _dictionary.TryAdd(idUsuarioQueGerouQrCode.ToString(), connectionId);

            conexao.IniciarContagemTempo(CallbackTempoExpirado);
        }

        private void CallbackTempoExpirado(string connectionId)
        {
            _dictionary.TryRemove(connectionId, out _);
        }

        public string GetConnectionIdByIdUsuario(long idUsuario)
        {
            if(!_dictionary.TryGetValue(idUsuario.ToString(), out var connectionId))
            {
                throw new MeuLivroDeReceitasException("");
            }
            return connectionId.ToString();
        }

        public void ResetarTempoExpiracao(string connectionId)
        {
            _dictionary.TryGetValue(connectionId, out var objetoConexao);
            var conexao = objetoConexao as Conexao;
            conexao.ResetarContagemTempo();
        }

        public void SetConnectionIdUsuarioLeitorQrCode(string connectionIdGerouQrCode, string connectionIdLeuQrCode)
        {
            _dictionary.TryGetValue(connectionIdGerouQrCode, out var objetoConexao);

            var conexao = objetoConexao as Conexao;
            conexao.SetConnectionIdUsuarioLeitorQrCode(connectionIdLeuQrCode);
        }

        public string Remover(string connectionId, long usuarioId)
        {
            _dictionary.TryGetValue(connectionId, out var objetoConexao);

            var conexao = objetoConexao as Conexao;
            conexao.StopTimer();

            _dictionary.TryRemove(connectionId, out _);
            _dictionary.TryRemove(usuarioId.ToString(), out _);

            return conexao.UsuarioQueLeuQrCode();
        }

    }
}
