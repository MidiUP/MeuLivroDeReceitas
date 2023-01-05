namespace MeuLivroDeReceitas.Application.UseCases.Conexao.AceitarConexao;

public interface IAceitarConexaoUseCase
{
    Task<long> Executar(long usuarioParaSeConectar);
}
