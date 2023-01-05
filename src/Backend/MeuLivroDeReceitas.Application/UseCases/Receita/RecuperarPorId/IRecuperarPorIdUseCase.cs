using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;

public interface IRecuperarPorIdUsecase
{
    Task<RespostaReceitaJson> Executar(long id);
}
