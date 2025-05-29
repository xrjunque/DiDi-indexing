Imports DiDi2020.DiDi20250526
Imports System.Text
Imports System.Threading

Public Class FrmDiDi
    Dim numClaves As Int32 = 10 ^ 7
    Dim minlenClaves As Int32 = 1
    Dim maxlenClaves As Int32 = 15
    Dim vNd(numClaves) As DiDi20250526.Nodo
    Dim data(numClaves)() As Byte
    Dim ObjDD As New DiDi20250526(vNd, data, 40)
    Dim v(numClaves - 1) As String
    Dim NextAvailableRegister As Int32
    Dim LastAddedRegister As Int32
    Shared LastFoundRegister As Int32
    Dim NLecturas As Int32
    Dim tiempoLecturas As Single
    Dim dlgMsg As New DlgMessage
    Shared WithEvents outputViewer As LargeTextViewer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        outputViewer = New LargeTextViewer()

        outputViewer.BorderStyle = BorderStyle.FixedSingle
        outputViewer.BackColor = Color.White
        outputViewer.Top = 50
        outputViewer.Left = 10
        outputViewer.Width = SplitContainer1.Panel2.Width - 20
        outputViewer.Height = SplitContainer1.Panel2.Height - 60
        outputViewer.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        SplitContainer1.Panel2.Controls.Add(outputViewer)

        Globalization.CultureInfo.CurrentUICulture = New Globalization.CultureInfo("en-US")
        Globalization.CultureInfo.CurrentCulture = New Globalization.CultureInfo("en-US")
        CBox_Mode.SelectedIndex = 0
        dgv.Columns.Add("Key", "Key")
        dgv.Columns.Add(" ", " ")
        outputViewer.ContextMenuStrip = ContextMenuStrip1
        AddHandler SplitContainer1.Panel1.Paint, AddressOf Panel1_Paint
        AddHandler SplitContainer1.Panel2.Paint, AddressOf Panel2_Paint
        dgv.BorderStyle = BorderStyle.None
        dgv.GridColor = Color.LightGray
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(180, 200, 220)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black

    End Sub
    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)
        Using brush As New Drawing2D.LinearGradientBrush(
        CType(sender, Panel).ClientRectangle,
        Color.White,                             ' Color inicial (arriba)
        Color.FromArgb(220, 230, 250),            ' Color final (abajo)
        Drawing2D.LinearGradientMode.Vertical)   ' Dirección vertical
            e.Graphics.FillRectangle(brush, CType(sender, Panel).ClientRectangle)
        End Using
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs)
        Using brush As New Drawing2D.LinearGradientBrush(
        CType(sender, Panel).ClientRectangle,
        Color.White,
        Color.FromArgb(220, 230, 250),
        Drawing2D.LinearGradientMode.Vertical)
            e.Graphics.FillRectangle(brush, CType(sender, Panel).ClientRectangle)
        End Using
    End Sub

    Private Async Sub BtnMode_Click(sender As Object, e As EventArgs) Handles BtnMode.Click
        Try
            EnableDisableButtons(False)
            Status(" ")
            Dim i As Int32 = 0
            Dim bIndexModified As Boolean = False
            For Each row As DataGridViewRow In dgv.Rows
                If row.IsNewRow Then Continue For
                Dim key = row.Cells(0).Value
                If key Is DBNull.Value OrElse Trim(key).Length = 0 Then Continue For
                Dim vClave() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(key)
                row.Cells(1).Value = ""
                Select Case CBox_Mode.SelectedIndex
                    Case 0
                        If Not Me.Add(key) Then
                            row.Cells(1).Value = "Already exits: reg." + LastAddedRegister.ToString
                        Else
                            bIndexModified = True
                        End If
                    Case 1
                        If Not Me.Remove(key) Then
                            row.Cells(1).Value = "Not Found"
                        Else
                            row.Cells(1).Value = "Removed: " + LastFoundRegister.ToString
                            bIndexModified = True
                        End If
                    Case 2
                        If Not Me.Find(key) Then
                            row.Cells(1).Value = "Not Found"
                        Else
                            row.Cells(1).Value = "Found: " + LastFoundRegister.ToString
                        End If
                End Select
            Next
            If Not bIndexModified Then
                Exit Try
            End If
            'tbOutput.Text = ToStringAllKeys().GetAwaiter.GetResult(1)
            ' 2
            Dim vs() As String = Await ToStringAllKeys()
            'outputViewer.SetText(vs(0))
            NLecturas = Int32.Parse(vs(1))
            tiempoLecturas = Single.Parse(vs(2)) / 100.0F

            outputViewer.strBuilder = outputViewer.strBuilder ' forzar el dibujado
            ' 3
            'Await FillOutputDisplay(vs(0))
            ProgressBar1.Value = 0
            Status($"{NLecturas} registers")
        Catch ex As Exception
            Status(ex.Message)
        Finally
            EnableDisableButtons(True)
        End Try
    End Sub
    Private Async Sub BtnGenerate_Click(sender As Object, e As EventArgs) Handles BtnGenerate.Click
        Try
            EnableDisableButtons(False)
            dlgMsg = New DlgMessage
            dlgMsg.msg1 = New Mensaje
            numClaves = NumericUpDown1.Value
            If numClaves <= 0 Then Exit Sub

            ' Inicializa la barra de progreso
            ProgressBar1.Minimum = 0
            ProgressBar1.Maximum = numClaves
            ProgressBar1.Value = 0

            ' 1
            Status("Generating arbitrary keys and adding to index...")
            Await Task.Run(Sub() GeneracionClavesYAltas())

            ' 2
            Dim vs() As String = Await ToStringAllKeys()
            'outputViewer.SetText(vs(0))
            Dim NLecturas As Int32 = Int32.Parse(vs(1))
            tiempoLecturas = Single.Parse(vs(2)) / 1000 / 100.0F

            ' 3
            'Await FillOutputDisplay(vs(0))

            ProgressBar1.Value = 0
            dlgMsg.msg1.tiempoLecturas = tiempoLecturas
            dlgMsg.msg1.numLecturas = NLecturas
            Status($"{NLecturas } registers")
            dlgMsg.Display()
            dlgMsg.Show()
            Status($"{numClaves - dlgMsg.msg1.duplicados } insertions. {dlgMsg.msg1.duplicados} duplicates.")
            ProgressBar1.Value = 0
        Catch ex As Exception
            Status(ex.Message)
        Finally
            EnableDisableButtons(True)
        End Try
    End Sub


    Sub GeneracionClavesYAltas()


        Dim rnd As New Random()
        Dim Duplicados As Int32 = 0

        For i As Int32 = 0 To numClaves - 1
            Dim t As Int64 = Now.Ticks
            Dim key As String = ""

            For j = 1 To minlenClaves
                key += Chr(rnd.Next(65, 65 + 26))
            Next
            For j = 1 To rnd.Next(0, maxlenClaves - minlenClaves)
                key += Chr(rnd.Next(65, 65 + 26))
            Next

            If ChkUnicity.Checked Then
                key += String.Format("{0:00000000}", NextAvailableRegister)
            End If

            dlgMsg.msg1.tiempoGenerar += New TimeSpan(Now.Ticks - t)

            If Not Add(key) Then
                Duplicados += 1
            End If
            If i Mod 1000 = 0 Then
                Dim i1 As Int32 = i
                StatusStrip1.Invoke(Sub()
                                        ProgressBar1.Value = Math.Min(i1, ProgressBar1.Maximum)
                                    End Sub)
            End If
        Next
        StatusStrip1.Invoke(
            Sub()
                ProgressBar1.Value = 0
            End Sub)

        dlgMsg.msg1.duplicados = Duplicados
    End Sub
    Function Add(key As String) As Boolean
        Dim vClave() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(key)
        Dim NAbis As Int32 = NextAvailableRegister
        Dim param As New SearchParams With {
        .key = vClave,
        .register = NAbis,
        .mode = Modes.Add
    }
        Dim t As Int64 = Now.Ticks
        Dim r As Boolean = ObjDD.Search(param)

        dlgMsg.msg1.tiempoAltas += New TimeSpan(Now.Ticks - t)

        LastAddedRegister = param.register
        If Not r Then Return False
        NextAvailableRegister += 1
        dlgMsg.msg1.numClaves += 1
        Return r
    End Function
    Function Remove(Key As String) As Boolean
        Dim vClave() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(Key)
        'Console.WriteLine("remove " + Key)
        Dim r As Boolean

        Dim param As New SearchParams With {
        .key = vClave,
        .register = LastAddedRegister,
        .mode = Modes.Remove
            }
        r = ObjDD.Search(param)
        LastFoundRegister = param.register
        Return r
    End Function
    Function Find(Key As String) As Boolean
        Dim vClave() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(Key)
        Dim r As Boolean

        Dim param As New SearchParams With {
        .key = vClave,
        .register = LastFoundRegister,
        .mode = Modes.SearchEqual
        }

        r = ObjDD.Search(param)
        LastFoundRegister = param.register
        Return r
    End Function
    Async Function ToStringAllKeys() As Task(Of String())

        Status("Retrieving sorted keys from index...")
        ProgressBar1.Value = 0
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = ObjDD.NumberOfKeys
        outputViewer.strBuilder = New StringBuilder

        Dim sb As StringBuilder = outputViewer.strBuilder

        Dim vKeys As String() = Await Task.Run(
            Function()
                Dim key As String = ""
                Dim vClave() As Byte = New Byte() {0}
                Dim r As Boolean
                Dim i As Int32 = 0

                Dim param As New SearchParams With {
                                .key = vClave,
                                .register = LastFoundRegister
                            }

                Dim t As Int64
                Dim t2 As Int64
                Do
                    t = Now.Ticks
                    param.mode = Modes.SearchNext
                    r = ObjDD.Search(param)
                    t2 += Now.Ticks - t
                    If r Then
                        sb.Append($"{param.register} " + Encoding.ASCII.GetString(param.key) + vbCrLf)
                        i += 1
                        If i Mod 1000 = 0 Then
                            StatusStrip1.Invoke(Sub()
                                                    ProgressBar1.Value = Math.Min(i, ProgressBar1.Maximum)
                                                End Sub)
                        End If
                    Else
                        Exit Do
                    End If

                Loop While r
                Dim ts As New TimeSpan(t2)
                Return New String() {sb.ToString, i.ToString, Math.Round(ts.TotalMilliseconds * 100)}
            End Function)

        outputViewer.strBuilder = outputViewer.strBuilder ' forzar el dibujado

        StatusStrip1.Invoke(
            Sub()
                ProgressBar1.Value = 0
            End Sub)
        Return vKeys
    End Function

    Async Function FillOutputDisplay(texto As String) As Task
        Status("Displaying keys in output window...")
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = texto.Length
        ProgressBar1.Value = 0
        Const bloque As Integer = 50000 ' caracteres por fragmento
        Dim partes = Enumerable.Range(0, CInt(Math.Ceiling(texto.Length / bloque))) _
        .Select(Function(i) texto.Substring(i * bloque, Math.Min(bloque, texto.Length - i * bloque))) _
        .ToList()

        Dim tsDisplay As TimeSpan = Await Task.Run(
            Function()
                Dim t As Int64 = Now.Ticks
                Dim i As Int32 = 0
                Dim j As Int32 = 0
                For Each parte In partes
                    outputViewer.Invoke(
                        Sub()
                            outputViewer.AppendText(parte)
                            i += parte.Length
                        End Sub)
                    Thread.Sleep(1) ' da un pequeño respiro al sistema
                    j += 1
                    If j Mod 10 = 0 Then
                        StatusStrip1.Invoke(Sub()
                                                ProgressBar1.Value = Math.Min(i, ProgressBar1.Maximum)
                                            End Sub)
                    End If
                Next
                Dim ts As New TimeSpan(Now.Ticks - t)
                Return ts
            End Function)
        dlgMsg.msg1.tiempoDisplay = tsDisplay
        Status("")
    End Function
    Sub Status(s As String)
        ToolStripStatusLabel1.Text = "Message: " + s
        StatusStrip1.Refresh()
    End Sub

    Private Async Sub BtnRemoveAll_Click(sender As Object, e As EventArgs) Handles BtnRemoveAll.Click
        Try
            EnableDisableButtons(False)
            Dim nRemoved As Int32 = Await RemoveAll()
            Await ToStringAllKeys()
            Status($"Removed {nRemoved} keys – {ObjDD.NumberOfKeys} remaining.")
        Catch ex As Exception
        Finally
            EnableDisableButtons(True)
        End Try
    End Sub

    Private Async Function RemoveAll() As Task(Of Int32)
        Status("Deleting keys...")
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = ObjDD.NumberOfKeys
        ProgressBar1.Value = 0
        Dim nRemoved As Int32 = Await Task.Run(
            Function()
                Dim key As String = ""
                Dim vClave() As Byte = New Byte() {0}
                Dim r As Boolean

                Dim param As New SearchParams With {
                                .key = vClave,
                                .register = LastFoundRegister
                            }
                Dim i As Int32 = 0
                Do
                    param.mode = Modes.SearchNext
                    r = ObjDD.Search(param)
                    If Not r Then
                        Exit Do
                    End If
                    param.mode = Modes.Remove
                    r = ObjDD.Search(param)
                    i += 1
                    If i Mod 1000 = 0 Then
                        StatusStrip1.Invoke(Sub()
                                                ProgressBar1.Value = Math.Max(i, ProgressBar1.Maximum)
                                            End Sub)
                    End If
                Loop While r
                Return i
            End Function)
        Return nRemoved
        'Status($"Removed {nRemoved} keys – {ObjDD.NumberOfKeys} remaining.")

        ' Este DisplayAll sí se puede ejecutar aquí (ya estamos de vuelta en UI)

    End Function

    Private Sub tbOutput_MouseUp(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            ContextMenuStrip1.Show(outputViewer, e.Location)
        End If
    End Sub

    'Private Sub ToolStripCpySelected_Click(sender As Object, e As EventArgs) Handles ToolStripCpySelected.Click
    '   Clipboard.SetText(outputViewer.SelectedText)
    'End Sub

    Private Sub ToolStripCpyToList_Click(sender As Object, e As EventArgs) Handles ToolStripCpyToList.Click
        Try

            Dim s As String = Clipboard.GetText
            Dim vs() As String = s.Split(vbCrLf) ' outputViewer.SelectedText.Split(vbCrLf)
            Dim pos0 As Int32 = 0 ' outputViewer.SelectionStart
            If pos0 > 0 Then
                If outputViewer.Text(pos0 - 1) <> vbLf Then
                    vs(0) = ""
                End If
            End If
            Dim pos1 As Int32 = s.Length ' outputViewer.SelectionStart + outputViewer.SelectionLength
            If pos1 < outputViewer.Text.Length Then
                If outputViewer.Text(pos1) <> vbCr Then
                    vs(vs.Length - 1) = ""
                End If
            End If

            For i As Int32 = 0 To vs.Length - 1
                Dim vs2() As String = vs(i).Split(" ")
                If vs2.Length > 1 AndAlso vs2(1).Trim.Length > 0 Then
                    dgv.Rows.Add(vs2(1), "")
                End If
            Next

        Catch ex As Exception

        End Try
    End Sub

    Private Sub BtnClearList_Click(sender As Object, e As EventArgs) Handles BtnClearList.Click
        dgv.Rows.Clear()
    End Sub
    Sub EnableDisableButtons(enable As Boolean)
        BtnGenerate.Enabled = enable
        BtnMode.Enabled = enable
        BtnRemoveAll.Enabled = enable
        BtnClearList.Enabled = enable
    End Sub
End Class
