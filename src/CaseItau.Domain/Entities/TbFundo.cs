namespace CaseItau.Domain.Entities;

public class TbFundo
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public int TipoFundoId { get; set; }
    public TbTipoFundo? TipoFundo { get; set; }
    public ICollection<TbPosicaoFundo> Posicoes { get; set; } = new List<TbPosicaoFundo>();
    public ICollection<TbMovimentacaoFundo> Movimentacoes { get; set; } = new List<TbMovimentacaoFundo>();
}
