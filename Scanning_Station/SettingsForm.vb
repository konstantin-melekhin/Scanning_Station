Imports Library3


Public Class SettingsForm
    Private StationName As String

    'Public Property StationName1 As String
    '    Get
    '        Return StationName
    '    End Get
    '    Set(value As String)
    '        StationName = value
    '    End Set
    'End Property

    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim PCInfo As New ArrayList() 'PCInfo = (App_ID, App_Caption, lineID, LineName, StationName,CT_ScanStep)
        PCInfo = GetPCInfo(26)
        If PCInfo.Count = 0 Then
            PrintLabel(L_Result, "Дальнейшая работа невозможна, " & vbCrLf & "укажите рабочую линию!", Color.Red)
        Else
            TextBox1.Text = "App_ID = " & PCInfo(0) & vbCrLf &
                        "App_Caption = " & PCInfo(1) & vbCrLf &
                        "lineID = " & PCInfo(2) & vbCrLf &
                        "LineName = " & PCInfo(3) & vbCrLf &
                        "StationName = " & PCInfo(4) & vbCrLf &
                        "CT_ScanStep = " & PCInfo(5) & vbCrLf
        End If
        'GetStationID(Label_StationName.Text)
    End Sub
End Class
