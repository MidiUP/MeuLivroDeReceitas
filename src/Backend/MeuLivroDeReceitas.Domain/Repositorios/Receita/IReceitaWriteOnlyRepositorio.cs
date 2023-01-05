namespace MeuLivroDeReceitas.Domain.Repositorios.Receita;

public interface IReceitaWriteOnlyRepositorio
{
    Task<Entidades.Receita> Adicionar(Entidades.Receita receita);
    Task Deletar(long idReceita);
}
