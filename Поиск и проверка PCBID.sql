/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
--SELECT TOP (1000) [PCBID],[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID],[ErrorCodeID],[Descriptions],[SNID]
--FROM [FAS].[dbo].[Ct_OperLog]
--where PCBID =  1074838

declare @IDLaser as int


SELECT TOP (1000) [PCBserial],[AOIpass],[AOIverify],[PCBScanTime],[PCID],[LaserStatus],[PCBResult],[IDstartTHT]
  FROM [SMDCOMPONETS].[dbo].[THTStart] where [PCBserial]= 'AQ00027'




select @IDLaser = (SELECT [IDLaser] FROM [SMDCOMPONETS].[dbo].[LazerBase] where Content = 'AQ00027')
If @IDLaser is null
select 'Fail'
else
SELECT  [PCBID],[StepID],[TestResult],[ScanDate],[SNID]
FROM [FAS].[dbo].[Ct_StepResult]
where PCBID =  @IDLaser


