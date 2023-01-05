using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Recuperar;

public interface IRecuperarPorUsuarioUseCase
{
    Task<List<RespostaReceitaJson>> Executar();
}
