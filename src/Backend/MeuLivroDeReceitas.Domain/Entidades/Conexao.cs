using System.ComponentModel.DataAnnotations.Schema;

namespace MeuLivroDeReceitas.Domain.Entidades;

[Table("conexao")]
public class Conexao : EntidadeBase
{
    public long UsuarioId { get; set; }
    public long ConectadoComUsuarioId { get; set; }

}

