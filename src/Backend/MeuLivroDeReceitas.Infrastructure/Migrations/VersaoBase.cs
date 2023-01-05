using FluentMigrator.Builders.Create.Table;

namespace MeuLivroDeReceitas.Infrastructure.Migrations;

public static class VersaoBase
{
    public static ICreateTableWithColumnOrSchemaOrDescriptionSyntax InserirColunasPadrao(ICreateTableWithColumnOrSchemaOrDescriptionSyntax tabela)
    {
        tabela
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("DataCriacao").AsDateTime().NotNullable();
        return tabela;
    }
}
