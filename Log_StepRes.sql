/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
SELECT TOP (1000) [PCBID]
      ,Ls.Content
	  ,[StepID]
      ,[TestResult]
      ,[ScanDate]
      ,[SNID]
  FROM [FAS].[dbo].[ATestTable_Ct_StepResult] As S
  left join SMDCOMPONETS.dbo.LazerBase as Ls On Ls.IDLaser = S.PCBID
  order by ScanDate desc



SELECT TOP (1000) [PCBID],ls.Content,[LOTID],[StepID],[TestResultID],[StepDate],[StepByID],[LineID],[ErrorCodeID]
,[Descriptions],[SNID]
  FROM [FAS].[dbo].[ATestCt_OperLog] as L
  left join SMDCOMPONETS.dbo.LazerBase as Ls On Ls.IDLaser = l.PCBID
    order by StepDate desc

use FAS SELECT ls.Content,Ss.StepName,Sr.Result,Er.ErrorCode,[Descriptions],Ln.LineName,Us.UserName
,format([StepDate],'dd.MM.yyyy HH:mm:ss') as Date
FROM [FAS].[dbo].[ATestCt_OperLog] as Lg
left join SMDCOMPONETS.dbo.LazerBase as Ls On Ls.IDLaser = lg.PCBID
left join [FAS].[dbo].[Ct_StepScan] as Ss On Ss.ID = StepID
left join [FAS].[dbo].[Ct_TestResult] as Sr On Sr.ID = TestResultID
left join [FAS].[dbo].[FAS_Users] as Us On Us.UserID = [StepByID]
left join [FAS].[dbo].[FAS_Lines] as Ln On Ln.LineID = Lg.LineID
left join [FAS].[dbo].[FAS_ErrorCode] as Er On Er.ErrorCodeID = Lg.ErrorCodeID
where lg.pcbid = 1086305
order by StepDate desc