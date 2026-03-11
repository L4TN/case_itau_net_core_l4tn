namespace CaseItau.Domain.Entities;

public class TbFeatureFlag
{
    public int Id { get; set; }
    public string Chave { get; set; } = string.Empty;
    public bool Habilitado { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? JsonConfig { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
}
