Public Class TestForm1
    Private Sub TestForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox2.Text = CInt("&H" & TextBox1.Text)
        TextBox3.Text = Convert.ToInt32(TextBox1.Text, 16)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim Format As String = "00001DPH110Sffffff"
        Dim Coordinats As Integer() = New Integer(2) {}
        For i = 0 To 5 Step 2
            Dim J As Integer
            Coordinats(J) = Convert.ToInt32(Mid(Mid(Format, Len(Format) - 5), i + 1, 2), 16)
            J += 1
        Next
        TextBox1.Text = ("X = " & Coordinats(0) & "; Y = " & Coordinats(1) & "; Z = " & Coordinats(2))
        TextBox3.Text = (Coordinats(0) + Coordinats(1) + Coordinats(2))
    End Sub
End Class