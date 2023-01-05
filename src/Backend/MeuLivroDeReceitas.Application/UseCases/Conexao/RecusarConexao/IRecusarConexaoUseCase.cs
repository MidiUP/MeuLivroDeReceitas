namespace MeuLivroDeReceitas.Application.UseCases.Conexao.RecusarConexao;

public interface IRecusarConexaoUseCase
{
    Task<long> Executar();
}
