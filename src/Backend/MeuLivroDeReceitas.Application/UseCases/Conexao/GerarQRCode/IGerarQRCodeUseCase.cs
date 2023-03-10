namespace MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;

public interface IGerarQRCodeUseCase
{
    Task<(string qrCode, long idUsuario)> Executar();
}
