namespace MeuLivroDeReceitas.Domain.Repositorios.Codigo
{
    public interface ICodigoReadOnlyRepository
    {
        Task<Entidades.Codigos> RecuperarEntidadeCodigo(string codigo);
    }
}
