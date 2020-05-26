/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
SELECT top 20 l.Content, [PCBID],[LOTID],[StepID],[TestResultID],[StepDate],
[StepByID],[LineID],[ErrorCodeID],[Descriptions],[SNID]
FROM [FAS].[dbo].[Ct_OperLog]
left join SMDCOMPONETS.dbo.LazerBase as L on l.IDLaser = PCBID
where stepdate > '2020-05-22'
order by  stepdate desc

--use fas delete [FAS].[dbo].[Ct_OperLog]where stepdate > '2020-05-22'

--insert into [FAS].[dbo].[Ct_OperLog] ([PCBID],[LOTID],[StepID],
--[TestResultID],[StepDate], [StepByID],[LineID],[ErrorCodeID],[Descriptions])
--values (1103833,20059,1,2,CURRENT_TIMESTAMP, 11,7,Null,Null)