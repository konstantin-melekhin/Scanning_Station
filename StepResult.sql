/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
SELECT TOP (1000) l.Content,[PCBID],[StepID],[TestResult],[ScanDate],[SNID]
  FROM [FAS].[dbo].[ATestTable_Ct_StepResult]
  left join SMDCOMPONETS.dbo.LazerBase as L on l.IDLaser = PCBID
  order by pcbid desc
