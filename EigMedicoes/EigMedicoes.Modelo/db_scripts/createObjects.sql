USE [EIG]
GO
/****** Object:  Table [dbo].[EIGMedicao_Import_Stage]    Script Date: 11/30/2016 11:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EIGMedicao_Import_Stage](
	[Agente] [varchar](50) NULL,
	[Ponto   Grupo] [varchar](50) NULL,
	[Data] [varchar](50) NULL,
	[Hora] [varchar](50) NULL,
	[Energia_Ativa_De_Consumo] [varchar](50) NULL,
	[Energia_Ativa_De_Geração] [varchar](50) NULL,
	[Qualidade] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EIGMedicao_Medicoes_Manuais]    Script Date: 11/30/2016 11:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EIGMedicao_Medicoes_Manuais](
	[Ponto] [varchar](50) NOT NULL,
	[Periodo] [smalldatetime] NOT NULL,
	[Consumo] [decimal](12, 3) NOT NULL,
	[Geracao] [decimal](12, 3) NOT NULL,
 CONSTRAINT [PK_EIGMedicao_Medicoes_Manuais] PRIMARY KEY CLUSTERED 
(
	[Ponto] ASC,
	[Periodo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EIGMedicao_Medicoes]    Script Date: 11/30/2016 11:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EIGMedicao_Medicoes](
	[Ponto] [varchar](50) NOT NULL,
	[Periodo] [smalldatetime] NOT NULL,
	[Consumo] [decimal](12, 3) NOT NULL,
	[Geracao] [decimal](12, 3) NOT NULL,
	[Consistido] [bit] NOT NULL,
 CONSTRAINT [PK_EIGMedicao_Medicoes] PRIMARY KEY CLUSTERED 
(
	[Ponto] ASC,
	[Periodo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EIGMedicao_MedicaoClientes]    Script Date: 11/30/2016 11:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EIGMedicao_MedicaoClientes](
	[ID_Unidade] [int] NOT NULL,
	[ID_Cadastro] [int] NOT NULL,
	[Inicio] [datetime] NOT NULL,
	[Fim] [datetime] NOT NULL,
	[MWh] [decimal](18, 8) NULL,
	[HorasFaltantes] [int] NOT NULL,
	[PossuiProjecao] [bit] NOT NULL,
	[SiglaAgente] [varchar](150) NULL,
	[CD_AGEN_SCDD] [varchar](150) NULL,
 CONSTRAINT [PK_EIGMedicao_MedicaoClientes] PRIMARY KEY CLUSTERED 
(
	[ID_Unidade] ASC,
	[Inicio] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[EIGMedicao_View_Medicoes]    Script Date: 11/30/2016 11:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EIGMedicao_View_Medicoes] AS

SELECT M.*,  cast( 0 as bit) as [Manual] FROM EIGMedicao_Medicoes M WHERE 
M.Consistido = 1 OR (
	M.Consistido = 0 AND 
	NOT EXISTS (SELECT 1 FROM EIGMedicao_Medicoes_Manuais MM WHERE MM.Ponto = M.Ponto and MM.Periodo = M.Periodo )
	)

UNION 

SELECT MM.*, cast( 1 as bit) as [Consistido], cast( 1 as bit)as [Manual] FROM EIGMedicao_Medicoes_Manuais MM
WHERE NOT EXISTS (SELECT 1 FROM EIGMedicao_Medicoes M WHERE 
	M.Consistido = 1 AND M.Ponto = MM.Ponto AND M.Periodo = MM.Periodo
	)
GO
/****** Object:  View [dbo].[EIGMedicao_View_Contratos2]    Script Date: 11/30/2016 11:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EIGMedicao_View_Contratos2]
AS

WITH EIG_PRINCIPAL AS (
  
  SELECT [ID_Agente_Vendedor] as ID_AGENTE, 'VENDA' as TIPO, * FROM
  [EIG].[dbo].[EIG_Boleta_Principal] 
  UNION 
  SELECT ID_Agente_Comprador as ID_AGENTE, 'COMPRA' as TIPO, * FROM
  [EIG].[dbo].[EIG_Boleta_Principal] 
  
  ) , EIG_BOLETA AS (
SELECT 
	EBP.Identificador,
	EBP.ID_AGENTE,
	Agente.Agente,
	EBP.TIPO,
	EBP.ID_Agente_Vendedor,
	EBP.ID_Agente_Comprador,
	EBP.Flex_Min,
	EBP.Flex_Max,
	EBP.Proinfa,	
	EBP.Prioridade_Proinfa,
	
	EBC.Inicio,
	EBC.Fim,	
	EBC.Montante,
	
	EPC.ID_Cadastro,
	EPC.Representatividade_Porcentagem,
	EPC.Representatividade_MWh,	
	EPC.Montante_Exercido,
	DATEDIFF( HOUR, EBC.Inicio, EBC.Fim ) + 
		CASE DATEPART(MONTH, EBC.Inicio)
		WHEN 10 THEN 23
		WHEN 2 THEN 25
		ELSE 24 END AS HORAS

	
FROM EIG_PRINCIPAL AS EBP
JOIN EIG_Boleta_Complemento AS EBC ON EBP.Identificador = EBC.Identificador
LEFT JOIN EIG_Particionamento_ContratosEnergia AS EPC ON EBP.Identificador = EPC.Identificador AND EBC.Inicio = EPC.Inicio
JOIN PRD_KALIMDOR.dbo.Agentes AS Agente ON EBP.ID_AGENTE = Agente.ID
), EIG_BOLETA2 AS  (
SELECT  
Identificador,
ID_Cadastro,
ID_AGENTE,
Agente,
TIPO,

ID_Agente_Vendedor,
ID_Agente_Comprador,

Proinfa,	
Prioridade_Proinfa,
Inicio,
	
ISNULL(
	ISNULL(Montante_Exercido * HORAS , Representatividade_MWh) 
	, Montante * ISNULL(Representatividade_Porcentagem, 1.0) * HORAS )	
	AS MONTANTE 
	
 FROM EIG_BOLETA)
 
 SELECT [ID_AGENTE]
      ,[Agente]
      ,[TIPO]
      ,[Inicio]
      ,SUM( case when [Proinfa] = 0 then MONTANTE else 0 end) as MONTANTE
      ,SUM( case when [Proinfa] = 1 AND Prioridade_Proinfa <> 1 then MONTANTE else 0 end) as MONTANTE_proinfa
      ,SUM( case when [Proinfa] = 1 AND Prioridade_Proinfa = 1 then MONTANTE else 0 end) as MONTANTE_proinfaPrioridade
      ,SUM(montante) as Total

      --,[Proinfa]
      --,[Prioridade_Proinfa]      
      --,[MONTANTE]
      
  FROM EIG_BOLETA2
  GROUP BY [ID_AGENTE]
      ,[Agente]
      ,[TIPO]
      ,[Inicio]
GO
/****** Object:  View [dbo].[EIGMedicao_View_Contratos]    Script Date: 11/30/2016 11:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EIGMedicao_View_Contratos]
AS



SELECT EBP.ID_AGENTE
      , Agente.Agente
      ,EBP.TIPO
	  ,EBC.Inicio
      ,EBC.Fim
      ,SUM(isnull(EPCE.Montante, EBC.Montante)) AS Montante_MWm
      /*,EBP.[Sazo_Min]
      ,EBP.[Sazo_Max]
      ,EBP.[Flex_Min]
      ,EBP.[Flex_Max]
      ,EBP.[Mod_Min]
      ,EBP.[Mod_Max]
      ,EBP.[Flex_Anual_Min]
      ,EBP.[Flex_Anual_Max]*/
  FROM (
  
  SELECT [ID_Agente_Vendedor] as ID_AGENTE, 'VENDA' as TIPO, * FROM
  [EIG].[dbo].[EIG_Boleta_Principal] 
  UNION 
  SELECT ID_Agente_Comprador as ID_AGENTE, 'COMPRA' as TIPO, * FROM
  [EIG].[dbo].[EIG_Boleta_Principal] 
  
  ) AS EBP
  LEFT JOIN EIG_Boleta_Complemento AS EBC ON EBP.Identificador = EBC.Identificador
  LEFT JOIN ( SELECT [Identificador]
				  ,[Inicio]
				  ,[Fim]
				  ,sum([Montante_Exercido]) MONTANTE
			  FROM [EIG].[dbo].[EIG_Particionamento_ContratosEnergia]
			  group BY [Identificador]
				  ,[Inicio]
				  ,[Fim] 
      ) AS EPCE ON EBC.Identificador = EPCE.Identificador AND EBC.Inicio = EPCE.Inicio
  LEFT JOIN PRD_KALIMDOR.dbo.Agentes AS Agente ON EBP.ID_AGENTE = Agente.ID
  GROUP BY EBP.ID_AGENTE
      ,Agente.Agente
      ,EBP.TIPO
	  ,EBC.Inicio
      ,EBC.Fim
GO
/****** Object:  View [dbo].[EIGMedicao_View_Cadastro]    Script Date: 11/30/2016 11:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EIGMedicao_View_Cadastro]
AS

SELECT TOP 1000 C.[Codigo de Cadastro]
  ,C.[Nome] as [Grupo]
  ,ISNULL(C.[Identificacao],C.[Razão Social]) as [Nome]
  ,CASE C.[Tipo] WHEN 1 THEN 'G' WHEN 3 THEN 'C' END [Tipo]
FROM [PRD_KALIMDOR].[dbo].[Cadastro] C
----
	join dbo.EIG_PortfolioClientes port on C.[Codigo de Cadastro] = port.ID_Cadastro
----
	
	
WHERE C.[Status] = 'A'
AND C.[Tipo] IN (1, 3)
GO
/****** Object:  View [dbo].[EIGMedicao_View_Agente]    Script Date: 11/30/2016 11:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[EIGMedicao_View_Agente]
AS

SELECT DISTINCT  --port.[ID_Unidade]
  port.[ID_Unidade]
, port.[Identificacao_Unidade] AS Unidade
, port.ID_Cadastro
, ag.[CD_AGEN_SCDD]
, ag.[SG_AGEN_SCDD]
, ISNULL(C.[Razão Social] , C.[Identificacao]) AS [Agente]
, CASE C.[Tipo] WHEN 1 THEN 'G' WHEN 3 THEN 'C' END [Tipo]
      
      
  FROM [EIG].[dbo].[EIG_PortfolioClientes] port
  
  JOIN [EIG_PortfolioClientes_PerfAgentes] perfAg ON port.[ID_Unidade] = perfAg.[ID_Unidade]
  JOIN [PRD_KALIMDOR].[dbo].[Agentes] Ag ON perfAg.[CD_PERF_AGEN] = Ag.[ID]
  JOIN [PRD_KALIMDOR].[dbo].[Cadastro] C ON port.ID_Cadastro = C.[Codigo de Cadastro]
  
  WHERE port.[Unidade_Ativa] = 1
  AND C.[Tipo] IN (1, 3)
GO
/****** Object:  StoredProcedure [dbo].[EIGMedicao_PROCESSAR_DADOS_IMPORTADOS]    Script Date: 11/30/2016 11:07:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EIGMedicao_PROCESSAR_DADOS_IMPORTADOS]		
	@RowCount INT OUTPUT, @FaltantesCount INT OUTPUT
AS
BEGIN
	
	SET NOCOUNT ON;

    
    
	WITH 
	S1 AS(
	SELECT * FROM [EIGMedicao_Import_Stage] WHERE 
	Hora not like '%[^0-9]%' 
	and SUBSTRING([Data], 1, 2) not like '%[^0-9]%'
	and SUBSTRING([Data], 4, 2) not like '%[^0-9]%'
	and SUBSTRING([Data], 7, 4) not like '%[^0-9]%'
	and LEN([Data]) = 10
	),
	STAGE AS (
	SELECT REPLACE([Agente], '"', '') AGENTE
		  ,LEFT(REPLACE([Ponto   Grupo], '"', ''), 13) PONTO
		  ,SUBSTRING([Data], 1, 2) DIA
		  ,SUBSTRING([Data], 4, 2) MES
		  ,SUBSTRING([Data], 7, 4) ANO
		  ,CAST([Hora] - 1 AS VARCHAR(2)) HORA
		  ,REPLACE(REPLACE([Energia_Ativa_De_Consumo], '.', ''), ',', '.') ENERGIA_ATIVA_CONSUMO
		  ,REPLACE(REPLACE([Energia_Ativa_De_Geração], '.', ''), ',', '.') ENERGIA_ATIVA_GERACAO 
		  ,REPLACE(REPLACE([Qualidade], '"', ''), ';', '') QUALIDADE
	FROM S1	
	)

	SELECT * INTO #STAGE FROM STAGE 
	
	;
	    
	
	SELECT DISTINCT
		S.PONTO AS Ponto,
		CAST( S.ANO + '-' + S.MES + '-' + S.DIA + ' ' + S.HORA + ':00:00' AS SMALLDATETIME) AS Periodo,
		ISNULL(ENERGIA_ATIVA_CONSUMO, 0) AS Consumo,
		ISNULL(ENERGIA_ATIVA_GERACAO, 0) AS Geracao,
		CASE 
			WHEN QUALIDADE = 'Completo' or CHARINDEX('Consistido',QUALIDADE  , 1) > 0 THEN 1 
			ELSE 0 END AS Consistido
		INTO #TREATED
	FROM #STAGE S 
	
	--select Ponto, Periodo, COUNT(1) from #treated
	--group by Ponto, Periodo
	--having COUNT(1) > 1
	
	--drop table #TREATED
		
		
		
	BEGIN TRANSACTION;
	
	
	PRINT 'APAGANDO DADOS DE MEDICAO'
	DELETE m FROM dbo.EIGMedicao_Medicoes m
	WHERE EXISTS ( SELECT 1 FROM #TREATED T 
		WHERE T.Periodo = m.Periodo
		AND T.Ponto = m.Ponto )
	
	;
	
	DELETE m 
	FROM dbo.EIGMedicao_Medicoes_Manuais m
	WHERE EXISTS ( SELECT 1 FROM #TREATED T 
		WHERE T.Periodo = m.Periodo
		AND T.Ponto = m.Ponto 
		AND T.Consistido = 1 )	
	;
			
	PRINT 'INSERINDO NOVOS DADOS'
	INSERT INTO dbo.EIGMedicao_Medicoes SELECT * FROM #TREATED;
	
	
	if(@@ERROR = 0)
	begin
	PRINT 'REALIZANDO COMMIT'
	COMMIT TRANSACTION
    
    --TO DO INSERIR LOG COM RESULTADO DO PROCESSAMENTO
    SELECT @RowCount = COUNT(1) FROM #TREATED WHERE Consistido = 1;
    SELECT @FaltantesCount = COUNT(1) FROM #TREATED WHERE Consistido = 0;  
    end
    else
    begin
	PRINT 'REALIZANDO ROLLBACK'
	ROLLBACK TRANSACTION	
	end
    
END
GO
/****** Object:  StoredProcedure [dbo].[EIGMedicao_IMPORTAR_ARQUIVO2_PARA_STAGE]    Script Date: 11/30/2016 11:07:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EIGMedicao_IMPORTAR_ARQUIVO2_PARA_STAGE] 
	-- Add the parameters for the stored procedure here
	@Arquivo nvarchar(256),
	@RowCount INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	truncate table [dbo].[EIGMedicao_Import_Stage]
	
	DECLARE @bulk_cmd varchar(1000);
	SET @bulk_cmd = '
	create table #tmp (
		Ponto varchar(50),
		Data  varchar(50),
		Hora  varchar(50),
		Tipo  varchar(50),
		Energia_Ativa_De_Geração  varchar(50),
		Energia_Ativa_De_Consumo  varchar(50),
		Energia_Reativa_De_Geração  varchar(50),
		Energia_Reativa_De_Consumo  varchar(50),
		Intervalos_Faltantes   varchar(50),
		Situação   varchar(50),
		Motivo_Da_Situação  varchar(50)
	)
	
	bulk insert #tmp
		FROM ''' + @Arquivo + ''' 
		with ( FIRSTROW = 4, FIELDTERMINATOR='';'', ROWTERMINATOR='''+CHAR(10)+''' )
		
	select * from #tmp';
	
	
	declare @Temp table (
		Ponto varchar(50),
		Data  varchar(50),
		Hora  varchar(50),
		Tipo  varchar(50),
		Energia_Ativa_De_Geração  varchar(50),
		Energia_Ativa_De_Consumo  varchar(50),
		Energia_Reativa_De_Geração  varchar(50),
		Energia_Reativa_De_Consumo  varchar(50),
		Intervalos_Faltantes   varchar(50),
		Situação   varchar(50),
		Motivo_Da_Situação  varchar(50)
	);
	
	INSERT INTO @Temp
	EXEC(@bulk_cmd);
	
	INSERT INTO [dbo].[EIGMedicao_Import_Stage]
	SELECT '', Ponto, Data, Hora, Energia_Ativa_De_Consumo, Energia_Ativa_De_Geração, Motivo_Da_Situação FROM @Temp
	
	--TO DO: INSERIR LOG COM RESULTADO DA IMPORTACAO
  
	SELECT @RowCount = COUNT(1) FROM [dbo].[EIGMedicao_Import_Stage];

END
GO
/****** Object:  StoredProcedure [dbo].[EIGMedicao_IMPORTAR_ARQUIVO_PARA_STAGE]    Script Date: 11/30/2016 11:07:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EIGMedicao_IMPORTAR_ARQUIVO_PARA_STAGE] 
	-- Add the parameters for the stored procedure here
	@Arquivo nvarchar(256),
	@RowCount INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	truncate table [dbo].[EIGMedicao_Import_Stage]
	
	DECLARE @bulk_cmd varchar(1000);
	SET @bulk_cmd = 'bulk insert [dbo].[EIGMedicao_Import_Stage]
		FROM ''' + @Arquivo + ''' 
		with ( FIRSTROW = 2, FIELDTERMINATOR='';'' )';
	EXEC(@bulk_cmd);
	
	--TO DO: INSERIR LOG COM RESULTADO DA IMPORTACAO
  
	SELECT @RowCount = COUNT(1) FROM [dbo].[EIGMedicao_Import_Stage];

END
GO
/****** Object:  Default [DF_EIGMedicao_MedicaoClientes_PossuiProjecao]    Script Date: 11/30/2016 11:07:40 ******/
ALTER TABLE [dbo].[EIGMedicao_MedicaoClientes] ADD  CONSTRAINT [DF_EIGMedicao_MedicaoClientes_PossuiProjecao]  DEFAULT ((0)) FOR [PossuiProjecao]
GO
