Imports System.Windows.Forms

Public Class DlgMessage
    Public msg1 As New Mensaje

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Public Sub Display()
        Dim s As String = ""
        s += $"Total keys......:  {msg1.numLecturas:N0}" + vbCrLf
        s += $"Total keys added:  {msg1.numClaves:N0}" + vbCrLf
        s += $"Duplicates......:  {msg1.duplicados:N0}" + vbCrLf
        s += $"Generation time.:  {msg1.tiempoGenerar.TotalSeconds:N2} s" + vbCrLf
        s += $"Insert time.....:  {msg1.tiempoAltas.TotalSeconds:N2} s" + vbCrLf
        s += $"Read time.......:  {msg1.tiempoLecturas:N2} s" + vbCrLf
        's += $"Display time....:  {msg1.tiempoDisplay.TotalSeconds:N2} s" + vbCrLf
        s += vbCrLf
        If msg1.numClaves > 0 Then
            s += $"Insert average..:  {msg1.tiempoAltas.TotalMilliseconds / msg1.numClaves:N4} ms/key" + vbCrLf
        Else
            s += $"Insert average..:  N/A" + vbCrLf
        End If
        If msg1.numLecturas > 0 Then
            s += $"Read average....:  {msg1.tiempoLecturas * 1000 / msg1.numLecturas:N4} ms/key" + vbCrLf
        Else
            s += $"Read average....:  N/A" + vbCrLf
        End If
        's += $"Display average.:  {msg1.tiempoDisplay.TotalMilliseconds / msg1.numLecturas:N4} ms/key" + vbCrLf
        tbMessage.Text = s
    End Sub
End Class
Public Class Mensaje
    Public numClaves As Int32
    Public duplicados As Int32
    Public numLecturas As Int32
    Public tiempoGenerar As TimeSpan
    Public tiempoAltas As TimeSpan
    Public tiempoLecturas As Single
    Public tiempoDisplay As TimeSpan
End Class
