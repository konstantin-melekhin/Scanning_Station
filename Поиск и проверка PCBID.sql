/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
--SELECT TOP (1000) [PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID],[ErrorCodeID],[Descriptions],[SNID]
--FROM [FAS].[dbo].[Ct_OperLog]
--where PCBID =  1074838

declare @IDLaser as int --=1074838
Declare @PCBSN as nvarchar (50) = '2129904BTH56100010019'


select @IDLaser = (SELECT [IDLaser] FROM [SMDCOMPONETS].[dbo].[LazerBase] where Content = @PCBSN)
If @IDLaser is null
select 'Fail'
else 
SELECT top 1 [PCBserial] FROM [SMDCOMPONETS].[dbo].[THTStart] as THT
where PCBserial = @PCBSN and PCBResult = 1


SELECT LB.Content,[PCBID],[StepID],[TestResult],[ScanDate],[SNID]
FROM [FAS].[dbo].[Ct_StepResult]
left join [SMDCOMPONETS].[dbo].[LazerBase] as LB On LB.IDLaser = [PCBID]
where LB.Content =  @PCBSN



