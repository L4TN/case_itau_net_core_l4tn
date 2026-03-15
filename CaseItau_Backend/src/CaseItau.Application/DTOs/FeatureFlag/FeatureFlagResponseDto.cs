namespace CaseItau.Application.DTOs.FeatureFlag;

public class FeatureFlagResponseDto
{
    public int Id { get; set; }
    public string Chave { get; set; } = string.Empty;
    public bool Habilitado { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? JsonConfig { get; set; }
}
