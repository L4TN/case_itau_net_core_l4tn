namespace CaseItau.Domain.Entities;

public class TbMovimentacaoFundo
{
    public int Id { get; set; }
    public int FundoId { get; set; }
    public DateTime DataMovimentacao { get; set; }
    public decimal VlrMovimentacao { get; set; }
    public TbFundo? Fundo { get; set; }
}
