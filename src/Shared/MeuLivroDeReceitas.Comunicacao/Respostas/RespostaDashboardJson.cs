﻿using System.Collections.Generic;

namespace MeuLivroDeReceitas.Comunicacao.Respostas;

public class RespostaDashboardJson
{
    public List<RespostaReceitaDashboardJson> Receitas { get; set; }
}
