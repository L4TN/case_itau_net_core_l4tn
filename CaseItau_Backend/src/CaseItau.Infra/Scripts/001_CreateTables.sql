-- =============================================
-- Script 001: Criação das tabelas
-- Case Itaú Asset Management
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tb_Tipo_Fundo')
BEGIN
    CREATE TABLE [Tb_Tipo_Fundo] (
        [Cd_Tipo_Fundo]   INT            NOT NULL,
        [Nm_Tipo_Fundo]   VARCHAR(20)    NOT NULL,
        CONSTRAINT [PK_Tb_Tipo_Fundo] PRIMARY KEY ([Cd_Tipo_Fundo])
    );
    PRINT 'Tabela Tb_Tipo_Fundo criada com sucesso.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tb_Fundo')
BEGIN
    CREATE TABLE [Tb_Fundo] (
        [Cd_Fundo]        VARCHAR(20)    NOT NULL,
        [Nm_Fundo]        VARCHAR(100)   NOT NULL,
        [Nr_Cnpj]         VARCHAR(14)    NOT NULL,
        [Cd_Tipo_Fundo]   INT            NOT NULL,
        [Vlr_Patrimonio]  DECIMAL(18,2)  NULL,
        CONSTRAINT [PK_Tb_Fundo] PRIMARY KEY ([Cd_Fundo]),
        CONSTRAINT [FK_Tb_Fundo_Tb_Tipo_Fundo] FOREIGN KEY ([Cd_Tipo_Fundo])
            REFERENCES [Tb_Tipo_Fundo] ([Cd_Tipo_Fundo])
            ON DELETE NO ACTION
    );
    PRINT 'Tabela Tb_Fundo criada com sucesso.';
END
GO

-- Índice único no CNPJ
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Tb_Fundo_Nr_Cnpj')
BEGIN
    CREATE UNIQUE INDEX [IX_Tb_Fundo_Nr_Cnpj] ON [Tb_Fundo] ([Nr_Cnpj]);
    PRINT 'Índice único IX_Tb_Fundo_Nr_Cnpj criado.';
END
GO
