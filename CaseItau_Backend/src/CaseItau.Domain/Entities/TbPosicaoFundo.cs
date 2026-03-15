namespace CaseItau.Domain.Entities;

public class TbPosicaoFundo
{
    public int Id { get; set; }
    public int FundoId { get; set; }
    public DateTime DataPosicao { get; set; }
    public decimal VlrPatrimonio { get; set; }
    public byte[] RowVersion { get; set; } = [];
    public TbFundo? Fundo { get; set; }
}
