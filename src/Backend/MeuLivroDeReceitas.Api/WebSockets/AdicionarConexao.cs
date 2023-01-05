using MeuLivroDeReceitas.Application.UseCases.Conexao.AceitarConexao;
using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
using MeuLivroDeReceitas.Application.UseCases.Conexao.RecusarConexao;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Api.WebSockets;

[Authorize(Policy = "UsuarioLogado")]
public class AdicionarConexao : Hub
{
    private readonly Broadcaster _broadcaster;
    private readonly IGerarQRCodeUseCase _gerarQRCodeUseCase;
    private readonly IHubContext<AdicionarConexao> _hubContext;
    private readonly IQRCodeLidoUseCase _qrCodeLidoUseCase;
    private readonly IRecusarConexaoUseCase _recusarConexaoUseCase;
    private readonly IAceitarConexaoUseCase _aceitarConexaoUseCase;

    public AdicionarConexao(IGerarQRCodeUseCase gerarQRCodeUseCase, 
        IHubContext<AdicionarConexao> hubContext, 
        IQRCodeLidoUseCase qrCodeLidoUseCase, 
        IRecusarConexaoUseCase recusarConexaoUseCase,
        IAceitarConexaoUseCase aceitarConexaoUseCase)
    {
        _broadcaster = Broadcaster.Instance;
        _gerarQRCodeUseCase = gerarQRCodeUseCase;
        _hubContext = hubContext;
        _qrCodeLidoUseCase = qrCodeLidoUseCase;
        _recusarConexaoUseCase = recusarConexaoUseCase;
        _aceitarConexaoUseCase = aceitarConexaoUseCase;
    }

     public async Task GetQRCode()
     {
         (var qrCode, var idUsuario) = await _gerarQRCodeUseCase.Executar();

         _broadcaster.InicializarConexao(_hubContext, idUsuario, Context.ConnectionId);

         await Clients.Caller.SendAsync("ResultadoQRCode", qrCode);
     }

     public async Task QRCodeLido(string codigoConexao)
     {
         try
         {
             (var usuarioParaSeConectar, var idUsuarioQueGerouQRCode) = await _qrCodeLidoUseCase.Executar(codigoConexao);

             var connectionIdUsuarioGerouQrCode = _broadcaster.GetConnectionIdByIdUsuario(idUsuarioQueGerouQRCode);
             var connectionIdUsuarioLeuQrCode = Context.ConnectionId;

             _broadcaster.ResetarTempoExpiracao(connectionIdUsuarioGerouQrCode);
             _broadcaster.SetConnectionIdUsuarioLeitorQrCode(connectionIdUsuarioGerouQrCode, connectionIdUsuarioLeuQrCode);

             await Clients.Client(connectionIdUsuarioGerouQrCode).SendAsync("ResultadoQRCodeLido", usuarioParaSeConectar);
         }
         catch(MeuLivroDeReceitasException ex)
         {
             await Clients.Caller.SendAsync("Erro", ex.Message);
         }
         catch
         {
             await Clients.Caller.SendAsync("Erro", ResourceMensagensDeErro.ERRO_DESCONHECIDO);
         }
     }

     public async Task RecusarConexao()
     {
         var connectionIdUsuarioQueGerouQRCode = Context.ConnectionId;
         var usuarioId = await _recusarConexaoUseCase.Executar();
         var connectionIdUsuarioLeuQRCode = _broadcaster.Remover(connectionIdUsuarioQueGerouQRCode, usuarioId);

         await Clients.Client(connectionIdUsuarioLeuQRCode).SendAsync("OnConexaoRecusada");
     }

     public async Task AceitarConexao(long idUsuarioParaSeConectar)
     {
         var usuarioId = await _aceitarConexaoUseCase.Executar(idUsuarioParaSeConectar);
         var connectionIdUsuarioQueGerouQRCode = Context.ConnectionId;


         var connectionIdUsuarioLeuQRCode = _broadcaster.Remover(connectionIdUsuarioQueGerouQRCode, usuarioId);
         await Clients.Client(connectionIdUsuarioLeuQRCode).SendAsync("OnConexaoAceita");
     }

    public override Task OnConnectedAsync()
    {
        var x = Context.ConnectionId;
        return base.OnConnectedAsync();
    }
}
