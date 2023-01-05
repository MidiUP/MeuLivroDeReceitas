using Microsoft.AspNetCore.SignalR;
using System;
using System.Timers;

namespace MeuLivroDeReceitas.Api.WebSockets
{
    public class Conexao
    {
        private readonly IHubContext<AdicionarConexao> _hubContext;
        private readonly string UsuarioQRCodeId;
        private Action<string> _callbackTempoExpirado;
        private string _connectionIdUsuarioLeitorQRCode;

        public Conexao(IHubContext<AdicionarConexao> hubContext, string usuarioQRCodeId) 
        {
            _hubContext = hubContext;
            UsuarioQRCodeId = usuarioQRCodeId;
        }
        private short tempoRestanteSegundos { get; set; }
        private Timer _timer { get; set; }

        public void IniciarContagemTempo(Action<string> callbackTempoExpirado)
        {
            _callbackTempoExpirado = callbackTempoExpirado;
            StartTimer();
        }

        public void ResetarContagemTempo()
        {
            StopTimer();
            StartTimer();
        }

        public void StopTimer()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }

        private async void ElapsetTimer(object sender, ElapsedEventArgs e)
        {
            if (tempoRestanteSegundos >= 0)
                await _hubContext.Clients.Client(UsuarioQRCodeId).SendAsync("SetTempoRestante", tempoRestanteSegundos--);
            else
            { 
                StopTimer();
                _callbackTempoExpirado(UsuarioQRCodeId);
            }
        }

        public void SetConnectionIdUsuarioLeitorQrCode(string connectionId)
        {
            _connectionIdUsuarioLeitorQRCode = connectionId;
        }

        public string UsuarioQueLeuQrCode()
        {
            return _connectionIdUsuarioLeitorQRCode;
        }

        private void StartTimer()
        {
            tempoRestanteSegundos = 60;
            _timer = new Timer(1000)
            {
                Enabled = true,
            };
            _timer.Elapsed += ElapsetTimer;
            _timer.Enabled = true;
        }
    }
}
