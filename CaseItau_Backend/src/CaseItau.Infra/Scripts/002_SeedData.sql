-- =============================================
-- Script 002: Seed data inicial
-- Case Itaú Asset Management
-- =============================================

-- Tipos de Fundo
IF NOT EXISTS (SELECT 1 FROM [Tb_Tipo_Fundo])
BEGIN
    INSERT INTO [Tb_Tipo_Fundo] ([Cd_Tipo_Fundo], [Nm_Tipo_Fundo]) VALUES (1, 'RENDA FIXA');
    INSERT INTO [Tb_Tipo_Fundo] ([Cd_Tipo_Fundo], [Nm_Tipo_Fundo]) VALUES (2, 'ACOES');
    INSERT INTO [Tb_Tipo_Fundo] ([Cd_Tipo_Fundo], [Nm_Tipo_Fundo]) VALUES (3, 'MULTI MERCARDO');
    PRINT 'Seed de Tb_Tipo_Fundo inserido com sucesso.';
END
GO

-- Fundos
IF NOT EXISTS (SELECT 1 FROM [Tb_Fundo])
BEGIN
    INSERT INTO [Tb_Fundo] ([Cd_Fundo], [Nm_Fundo], [Nr_Cnpj], [Cd_Tipo_Fundo], [Vlr_Patrimonio])
    VALUES ('ITAURF123', 'ITAU JUROS RF +', '11222333444455', 1, 5498731.54);

    INSERT INTO [Tb_Fundo] ([Cd_Fundo], [Nm_Fundo], [Nr_Cnpj], [Cd_Tipo_Fundo], [Vlr_Patrimonio])
    VALUES ('ITAUMM999', 'ITAU TREND MM', '12222333444455', 3, 5.00);

    INSERT INTO [Tb_Fundo] ([Cd_Fundo], [Nm_Fundo], [Nr_Cnpj], [Cd_Tipo_Fundo], [Vlr_Patrimonio])
    VALUES ('ITAURF321', 'ITAU LONGO PRAZO RF +', '13222333444455', 1, 7875421.58);

    INSERT INTO [Tb_Fundo] ([Cd_Fundo], [Nm_Fundo], [Nr_Cnpj], [Cd_Tipo_Fundo], [Vlr_Patrimonio])
    VALUES ('ITAUAC546', 'ITAU ACOES DIVIDENDO', '14222333444455', 2, 66421254.83);

    PRINT 'Seed de Tb_Fundo inserido com sucesso.';
END
GO
