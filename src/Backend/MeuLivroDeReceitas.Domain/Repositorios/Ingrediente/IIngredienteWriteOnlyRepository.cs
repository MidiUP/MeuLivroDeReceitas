namespace MeuLivroDeReceitas.Domain.Repositorios.Ingrediente;

public interface IIngredienteWriteOnlyRepository
{
    Task AdicionarIngredientes(List<Entidades.Ingrediente> ingredientes);
}
