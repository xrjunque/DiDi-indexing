Imports System.Text
Imports System.Windows.Forms

'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated(), System.ComponentModel.DesignerCategory("Code")>
Public Class LargeTextViewer
    Inherits UserControl

    Private _textContent As New StringBuilder()
    Private _lines As New List(Of String)()
    Private _lineHeight As Integer
    Private _vScrollBar As New VScrollBar
    Private _scrollOffset As Integer

    ' Para la selección de texto
    Private _selecting As Boolean = False
    Private _selectionStartLine As Integer = -1
    Private _selectionEndLine As Integer = -1

    Public Sub New()
        InitializeComponent()
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.Font = New Font("Consolas", 10)
        InitScrollBar()
    End Sub

    Private Sub InitScrollBar()
        _vScrollBar = New VScrollBar()
        _vScrollBar.Dock = DockStyle.Right
        AddHandler _vScrollBar.Scroll, AddressOf ScrollBar_Scroll
        Me.Controls.Add(_vScrollBar)
    End Sub

    Private Sub ScrollBar_Scroll(sender As Object, e As ScrollEventArgs)
        _scrollOffset = e.NewValue
        Invalidate()
    End Sub
    Public Property strBuilder As StringBuilder
        Get
            Return _textContent
        End Get
        Set(value As StringBuilder)
            If value IsNot Nothing Then
                _textContent = value
                _lines = _textContent.ToString().Split({vbCrLf}, StringSplitOptions.None).ToList()
                UpdateScrollBar()
                Invalidate()
            End If
        End Set
    End Property

    Public Sub SetText(content As String)
        _textContent.Clear()
        _textContent.Append(content)
        _lines = _textContent.ToString().Split({vbCrLf}, StringSplitOptions.None).ToList()
        CalculateLineHeight()
        UpdateScrollBar()
        Invalidate()
    End Sub

    Public Sub AppendText(content As String)
        If _textContent.Length > 0 Then _textContent.Append(vbCrLf)
        _textContent.Append(content)
        _lines = _textContent.ToString().Split({vbCrLf}, StringSplitOptions.None).ToList()
        UpdateScrollBar()
        Invalidate()
    End Sub

    Private Sub CalculateLineHeight()

        Using g = Me.CreateGraphics()
            Dim sz = g.MeasureString("A", Me.Font)
            _lineHeight = CInt(sz.Height)
        End Using
    End Sub

    Private Sub UpdateScrollBar()
        If _lineHeight = 0 Then CalculateLineHeight()
        Dim totalHeight = _lines.Count * _lineHeight
        _vScrollBar.Minimum = 0
        _vScrollBar.Maximum = Math.Max(0, totalHeight - 1)
        _vScrollBar.LargeChange = Me.ClientSize.Height
        _vScrollBar.SmallChange = _lineHeight
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If _lineHeight = 0 Then CalculateLineHeight()
        UpdateScrollBar()
        Invalidate()
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        e.Graphics.Clear(Me.BackColor)


        Dim firstVisibleLine = Math.Max(0, _scrollOffset \ _lineHeight)
        Dim linesVisible = (Me.ClientSize.Height \ _lineHeight) + 1
        Dim lastVisibleLine = Math.Min(_lines.Count - 1, firstVisibleLine + linesVisible)

        Dim y As Integer = -(_scrollOffset Mod _lineHeight)

        For i As Integer = firstVisibleLine To lastVisibleLine
            If IsLineSelected(i) Then
                e.Graphics.FillRectangle(Brushes.LightBlue, New Rectangle(0, y, Me.ClientSize.Width - _vScrollBar.Width, _lineHeight))
            End If
            e.Graphics.DrawString(_lines(i), Me.Font, Brushes.Black, New PointF(0, y))
            y += _lineHeight
        Next
    End Sub
    ' Scroll al principio
    Public Sub ScrollToTop()
        If _vScrollBar IsNot Nothing Then
            _vScrollBar.Value = _vScrollBar.Minimum
        End If
    End Sub

    ' Scroll al final
    Public Sub ScrollToBottom()
        If _vScrollBar IsNot Nothing Then
            _vScrollBar.Value = Math.Max(_vScrollBar.Maximum - _vScrollBar.LargeChange + 1, _vScrollBar.Minimum)
        End If
    End Sub
    Private Function IsLineSelected(index As Integer) As Boolean
        If _selectionStartLine = -1 OrElse _selectionEndLine = -1 Then Return False
        Dim minLine = Math.Min(_selectionStartLine, _selectionEndLine)
        Dim maxLine = Math.Max(_selectionStartLine, _selectionEndLine)
        Return index >= minLine AndAlso index <= maxLine
    End Function

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)

        If e.Button = MouseButtons.Left Then
            _selecting = True
            _selectionStartLine = (_scrollOffset + e.Y) \ _lineHeight
            _selectionEndLine = _selectionStartLine
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        If _selecting Then
            _selectionEndLine = (_scrollOffset + e.Y) \ _lineHeight
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If e.Button = MouseButtons.Left Then
            CopySelectedText()
            _selecting = False
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.Control AndAlso e.KeyCode = Keys.C Then
            CopySelectedText()
        End If
    End Sub

    Private Sub CopySelectedText()
        If _selectionStartLine = -1 OrElse _selectionEndLine = -1 Then Return

        Dim minLine = Math.Min(_selectionStartLine, _selectionEndLine)
        Dim maxLine = Math.Max(_selectionStartLine, _selectionEndLine)

        If minLine >= 0 AndAlso maxLine < _lines.Count Then
            Dim selectedText = String.Join(vbCrLf, _lines.GetRange(minLine, maxLine - minLine + 1))
            Clipboard.SetText(selectedText)
        End If
    End Sub

    Public ReadOnly Property TextContent As String
        Get
            Return _textContent.ToString()
        End Get
    End Property
End Class
