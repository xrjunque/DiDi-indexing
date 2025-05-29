Imports System
Imports System.IO
Imports System.Threading
Imports System.Text
Imports System.Runtime.InteropServices
Public Class didi20250424
    Private I As Int32, N1 As Int32
    <StructLayout(LayoutKind.Explicit)>
    Public Structure Nodo
        <FieldOffset(0)> Public sigRegClau As Int32
        <FieldOffset(4)> Public nivel As Int32
        <FieldOffset(8)> Public Der As Int32
        <FieldOffset(12)> Public Izq As Int32
        <FieldOffset(16)> Public reg As Int32
        <FieldOffset(20)> Public lenClau As Int32
        Public Const lenNodo = 24
        Public Overrides Function ToString() As String
            Return "sigRegclau:" + sigRegClau.ToString + " nivel=" + nivel.ToString +
                    " der=" + Der.ToString + " izq=" + Izq.ToString + " reg=" + reg.ToString + " lenClau=" + lenClau.ToString
        End Function
    End Structure
    Private N As Nodo, NV As Nodo, NEq As Nodo
    Private NReg As Int32, NEqReg As Int32
    Private B1 As Int32, D As Int32
    Private maxLenClau As Int32
    Private longitud As Int32 = maxLenClau * 9
    Private J() As Nodo
    Private K As Int32, KeyNearest As Int32
    Private Pila() As Int32
    Private n2S As Byte()
    Private AD() As Nodo
    Private TheFile(-1)() As Byte
    Private vNA() As Int32
    Public Mensaje As String
    Private NAdd As Int32

    Public Shared Function ver() As String
        Return "2025/05/26" ' 
    End Function
    Public ReadOnly Property NumberOfKeys() As Int32
        Get
            Return AD(0).lenClau
        End Get
    End Property

    Public Sub New(ByVal directori() As Nodo,
        ByVal data()() As Byte, maxLenClaves As Int32) ' comentado 2020/06: , ByVal lenData As Int32)
        AD = directori
        Me.TheFile = data
        ReDim vNA(AD.Length)
        'Me.lenData = lenData comentado 2020
        longitud = maxLenClaves * 8 + 1
        If longitud = 0 Then longitud = 128 * 9
    End Sub
    Private Function AddLeadingAndTrailingBytes(ByVal bytes As Byte()) As Byte()
        Dim b() As Byte = Nothing
        If bytes Is Nothing Then
            b = New Byte() {128}
            Return b
        End If
        ReDim b(bytes.Length + 1)
        b(0) = 128
        Array.Copy(bytes, 0, b, 1, bytes.Length)
        b(b.Length - 1) = 1
        Return b
    End Function
    Private Function RemoveLeadingAndTrailingBytes(ByVal bytes As Byte()) As Byte()
        Dim b() As Byte = Nothing  ', i As Int32
        If bytes Is Nothing Then Return Nothing
        ReDim b(bytes.Length - 3) ' remove leading 128 and trailing 1
        Array.Copy(bytes, 1, b, 0, b.Length)
        Return b
    End Function
    Public Class SearchParams
        Public mode As Modes
        Public key() As Byte
        Public register As Int32
    End Class
    Public Enum Modes
        Add = 1
        Remove = 2
        SearchEqual = 4
        SearchNext = 8
        SearchPrevious = 16
    End Enum
    Public Function Search(param As SearchParams) As Boolean
        ' Available Modes:
        ' add
        ' remove
        ' =  -> search for a key
        ' >  ->   "     "  next key
        ' <  ->   "     "  previous key
        ' => or >= -> " "  a key equal or greater
        ' =< or <= "    "  " "   less or equal

        Dim t As Int64 = Now.Ticks
        Dim mode As Modes = param.mode
        If (mode = Modes.Add OrElse mode = Modes.Remove) AndAlso param.key Is Nothing Then
            Mensaje = "The key is empty."
            Return False
        End If
        Dim clave = AddLeadingAndTrailingBytes(param.key)
        Dim naTemp = param.register
        Try
            Dim oClosest As New Closest With {
                .key = clave,
                .mode = param.mode,
                .register = naTemp
            }

            oClosest = SearchClosestKey(oClosest)

            clave = RemoveLeadingAndTrailingBytes(oClosest.key)
            If clave Is Nothing Then
                param.register = -1
                Return False
            End If

            param.key = clave
            param.register = oClosest.register ' o algún valor propio de oClosest

            Return oClosest.result

        Catch ex As Exception
            Throw New Exception("ad3.bas " & param.mode.ToString & " " & ex.ToString() & " NA=" & naTemp, ex)
        End Try
    End Function
    Class Closest
        Friend mode As Modes
        Friend register As Int32
        Friend key() As Byte
        Friend result As Boolean = True
    End Class
    Private Function SearchClosestKey(oClosest As Closest) As Closest
        Dim newRegister As Int32 = oClosest.register
        Dim modo As Modes = oClosest.mode
        Dim key() As Byte = oClosest.key
        Dim K1 As Int32, V1S As Byte()
        N1 = newRegister
        V1S = key
        ReDim J((V1S.Length + 1) * 8), Pila((V1S.Length + 1) * 8)
        NReg = 0 ' nodo raíz árbol derecho
        N = ReadDirectory(NReg) : J(0) = N : Pila(0) = NReg
        If N.Der = 0 Then
            ' DiDi is empty
            If modo <> Modes.Add Then
                NAdd = -1
                GoTo retFalse
            End If
            ' Si el árbol está vacío, se crea el primer nodo hijo a la derecha del nodo raíz (registro 0)
            NV.Der = 0 : NV.Izq = 0
            For K1 = 1 To longitud
                If IsBitSet(V1S, V1S.Length, K1) = True Then NV.nivel = K1 : Exit For
            Next
            NV.reg = NAdd '
            Dim nnodo As Int32 = GetNode()
            J(0).nivel = 0
            J(0).Der = nnodo
            J(0).lenClau += 1
            Call WriteDirectory(NV, nnodo) ' añadido 2020/06 (nnodo se supone=1 )
            WriteKeyAndDataRegisters(key, nnodo)
            Call WriteDirectory(J(0), 0)
            GoTo retTrue
        Else
            NReg = N.Der : D = 1 : N = ReadDirectory(NReg)
            NEq.nivel = 0
            NV.Der = 0 : NV.Izq = 0 : NV.nivel = 0 : NV.reg = 0
            K = 0 : KeyNearest = 0
            NEq = N : NEqReg = 1 : KeyNearest = 1
            Do
                K = K + 1 : J(K) = N : Pila(K) = NReg : Pila(K + 1) = 0 : J(K + 1).nivel = 0
                If IsBitSet(V1S, V1S.Length, N.nivel) = True Then
                    D = 1 : NEq = N : NEqReg = NReg : KeyNearest = K
                    If N.Der = 0 Then Exit Do
                    NReg = N.Der : N = ReadDirectory(NReg)
                Else
                    D = 0
                    If N.Izq = 0 Then Exit Do
                    NReg = N.Izq : N = ReadDirectory(NReg)
                End If
            Loop Until N.nivel = 0

            ' Found closest key at node NEqReg
            If Not (ReadKeyAndDataRegisters(NEqReg)) Then
                GoTo retFalse
            End If
            Dim rc As Int32 = CompareTwoKeys(V1S, n2S)
            If rc <> 0 Then
                ' If keys differ, find differing bit B1:
                FindDifferingBit_B1(V1S, rc)
            End If
            'If InStr(mode, "=") < 1 AndAlso
            '(InStr(mode, ">") > 0 OrElse InStr(mode, "<") > 0) Then
            If modo = Modes.SearchNext OrElse modo = Modes.SearchPrevious Then
                Dim b As Boolean = NextOrPreviousKey(modo, key)
                If b Then GoTo retTrue
                GoTo retFalse
            End If
            If rc = 0 Then
                'If InStr(mode, "=") > 0 Then
                If (modo And Modes.SearchEqual) > 0 Then
                    NAdd = NEqReg
                    GoTo retTrue
                ElseIf modo = Modes.Remove Then
                    NAdd = NEqReg : I = NEqReg
                    If remove(key) Then
                        GoTo retTrue
                    Else
                        GoTo retFalse
                    End If
                ElseIf modo = Modes.Add Then
                    GoTo retFalse
                Else ' remove
                    Throw New Exception("Unexpected error removing.")
                End If
            Else
                If modo = Modes.Remove Then
                    GoTo retFalse
                ElseIf modo = Modes.Add Then

                    Dim b As Boolean = AddKeyToTree(V1S, key, n2S)
                    If b = False Then
                        WriteDirectory(J(0), 0)
                        GoTo retFalse
                    End If
                    GoTo retTrue
                Else
                    Mensaje = "Unknown mode"
                    GoTo retFalse
                End If
            End If
        End If
retFalse:
        oClosest.key = key
        oClosest.result = False
        oClosest.register = NAdd
        Return oClosest
retTrue:
        oClosest.key = key
        oClosest.result = True
        oClosest.register = NAdd
        Return oClosest
    End Function

    ' Función que obtiene un nuevo nodo libre, ya sea del final o reutilizando un nodo dado de baja
    Private Function GetNode() As Int32
        Dim nnodo As Int32
        If J(0).reg = 0 Then
            ' No hay registros dados de baja, se toma uno nuevo
            J(0).Izq += 1
            nnodo = J(0).Izq
        Else
            ' Se reutiliza un nodo dado de baja
            nnodo = J(0).reg
            Dim nd As Nodo = ReadDirectory(J(0).reg)
            J(0).reg = nd.reg ' Avanza al siguiente en la cadena de bajas
            nd.reg = 0 ' Limpieza
        End If
        WriteDirectory(J(0), 0) ' Se guarda el estado actualizado del registro 0
        Return nnodo
    End Function
    ' Marca un nodo como dado de baja y lo enlaza en la lista de registros libres
    Private Sub RemoveNode(ByVal Nnodo As Int32)
        Dim nd As Nodo = ReadDirectory(Nnodo)
        Dim ult As Int32 = J(0).reg
        nd.reg = ult ' Enlaza este nodo al inicio de la lista de registros dados de baja
        J(0).reg = Nnodo ' Actualiza el puntero en el registro 0 para que apunte a este nodo
        WriteDirectory(nd, Nnodo) ' Guarda el nodo actualizado
        WriteDirectory(J(0), 0) ' Guarda el estado del registro 0
    End Sub

    Public Shared Function CompareTwoKeys(ByVal b1 As Byte(), ByVal b2 As Byte()) As Int32
        Dim l1 As Int32 = b1.Length
        Dim l2 As Int32 = b2.Length
        Dim l As Int32 = If(l1 >= l2, l2, l1)

        For i As Int32 = 0 To l - 1
            If b1(i) > b2(i) Then
                Return 1
            ElseIf b1(i) < b2(i) Then
                Return -1
            End If
        Next
        If l1 = l2 Then
            Return 0
        ElseIf l1 > l2 Then
            Return 1
        End If
        Return -1
    End Function
    Private Function NextOrPreviousKey(ByVal modo As Modes, ByRef clave As Byte()) As Boolean
        Dim sig As Int32, Nsig As Nodo
        Dim clau() As Byte = Nothing
        Dim na1
        Nsig = ReadDirectory(0)
        sig = Nsig.Der ' next node
        na1 = -1
        If sig = 0 Then
            GoTo salida
        End If
        Dim es As String = System.Text.ASCIIEncoding.ASCII.GetString(clave)
        Do
            Nsig = ReadDirectory(sig)
            If Not (ReadKeyAndDataRegisters(sig)) Then
                GoTo salida
            End If
            Dim cmp As Int32 = CompareTwoKeys(n2S, clave)
            If cmp < 1 Then ' n2S <= clave Then
                'If  InStr(modo, "<") > 0 Then
                If (modo And Modes.SearchPrevious) > 0 Then
                    If cmp <> 0 Then
                        na1 = Nsig.reg
                        ReDim clau(n2S.Length - 1)
                        Array.Copy(n2S, clau, n2S.Length)
                        Continue Do
                    End If
                End If
                If Nsig.Der = 0 Then
                    Exit Do
                Else
                    sig = Nsig.Der
                    Continue Do
                End If
            End If
            If cmp >= 0 Then
aelse:
                '  If InStr(modo, ">") > 0 Then
                If (modo And Modes.SearchNext) > 0 Then
                    na1 = Nsig.reg
                    ReDim clau(n2S.Length - 1)
                    Array.Copy(n2S, clau, n2S.Length)
                End If
                If Nsig.Izq = 0 Then
                    Exit Do
                Else
                    sig = Nsig.Izq
                End If
            End If
        Loop
salida:
        NAdd = na1
        If NAdd = -1 Then Return False
        'clave = clau
        ReDim clave(clau.Length - 1)
        Array.Copy(clau, clave, clau.Length)
        Return True
    End Function
    Private Function remove(ByRef clave As Byte())
        If Pila(KeyNearest) <> I Then
            Return False
        End If
        If J(KeyNearest).Der = 0 Then
            If J(KeyNearest - 1).Der = Pila(KeyNearest) Then
                J(KeyNearest - 1).Der = J(KeyNearest).Izq
            Else
                J(KeyNearest - 1).Izq = J(KeyNearest).Izq
            End If
            Call WriteDirectory(J(KeyNearest - 1), Pila(KeyNearest - 1))
            NAdd = J(KeyNearest).reg ' NA=Pila(KEq) 3/julio/2004
            'Recorrido(d1, ">", "", 0, "", NA)
        Else
            K = KeyNearest + 1 : J(K) = ReadDirectory(J(KeyNearest).Der) : Pila(K) = J(KeyNearest).Der
            Do While J(K).Izq <> 0
                K = K + 1
                Pila(K) = J(K - 1).Izq
                J(K) = ReadDirectory(J(K - 1).Izq)
            Loop
            If J(KeyNearest - 1).Der = Pila(KeyNearest) Then
                J(KeyNearest - 1).Der = Pila(K)
            Else
                J(KeyNearest - 1).Izq = Pila(K)
            End If
            If K = KeyNearest + 1 Then
                ' ¡¡¡ MODIFICADO !!! 26/ABRIL/2003
                J(K).Izq = J(KeyNearest).Izq
                J(K).nivel = J(KeyNearest).nivel
                Call WriteDirectory(J(KeyNearest - 1), Pila(KeyNearest - 1))
                Call WriteDirectory(J(K), Pila(K))
            Else
                ' ¡¡¡ MODIFICADO !!! 26/ABRIL/2003
                J(K - 1).Izq = J(K).Der
                J(K).nivel = J(KeyNearest).nivel
                J(K).Izq = J(KeyNearest).Izq
                J(K).Der = J(KeyNearest).Der
                Call WriteDirectory(J(KeyNearest - 1), Pila(KeyNearest - 1))
                Call WriteDirectory(J(K - 1), Pila(K - 1))
                Call WriteDirectory(J(K), Pila(K))
            End If
            NAdd = J(KeyNearest).reg ' NA= Pila(KEq) ' 6/Julio/2004
        End If
        Dim nd As Nodo = J(KeyNearest)
        Dim l1 As Int32 = nd.lenClau
        Me.RemveNode(Pila(KeyNearest))
        Do While l1 > 0 And nd.sigRegClau > 0
            Me.RemveNode(nd.sigRegClau)
            nd = ReadDirectory(nd.sigRegClau)
            l1 -= Nodo.lenNodo
        Loop
        J(0).lenClau -= 1
        If J(0).lenClau = 0 Then
            J(0).Izq = 0 : J(0).Der = 0 : J(0).reg = 0 : J(0).nivel = 0
            'J(0).sigRegClau = 0
        End If
        WriteDirectory(J(0), 0)
        Return True
    End Function
    Private Function AddKeyToTree(ByVal V1S As Byte(), ByRef clave As Byte(),
            ByVal n2S As Byte()) ' 2020 comentado el parámetro siguiente: , ByVal modo As String)
        'Dim I2 As Int32
        Dim K1 As Int32, K2 As Int32 ', K3 As Int32
        'Dim count As Int32
        'Dim Nzero As Nodo = R(0)
        Try
            N1 = Me.GetNode()
            ' comentado 2020/06: I = Me.NA 'N1
            I = N1

            Dim NV As New Nodo
            NV.Izq = 0 : NV.Der = 0
            If CompareTwoKeys(V1S, n2S) = 1 Then ' > n2S Then
                'If compara(V1S, V1S.Length, n2S, n2S.Length) = 1 Then ' > n2S Then
                K1 = 1
                For K2 = 1 To K
                    If J(K2).nivel > B1 Then Exit For
                    K1 = K2
                Next
                If Pila(K1) = N1 Then
                    Throw New Exception($"Unknown error, unexpected Pila(k1)=N1")
                End If
                'If J(K1).Izq = Pila(K1 + 1) AndAlso _
                'V0(V1S, J(K1).nivel) = False Then
                If J(K1).Izq = Pila(K1 + 1) AndAlso
                    IsBitSet(V1S, V1S.Length, J(K1).nivel) = False Then
                    NV.nivel = B1
                    NV.reg = I
                    J(K1).Izq = N1
                    NV.Izq = Pila(K1 + 1)
                    'Call W(NV, N1) 
                    Call WriteDirectory(J(K1), Pila(K1))
                Else
                    NV.nivel = B1
                    NV.reg = I
                    J(K1).Der = N1
                    NV.Izq = Pila(K1 + 1)
                    'Call W(NV, N1) 
                    Call WriteDirectory(J(K1), Pila(K1))
                End If
                GoTo salida
                'Return True
            End If
            For K1 = K To 2 Step -1
                If B1 > J(K1).nivel Then Exit For
            Next
            For K2 = K1 To 2 Step -1
                'If V0(V1S, J(K2).nivel) = True Then Exit For
                If IsBitSet(V1S, V1S.Length, J(K2).nivel) = True Then Exit For
            Next
            NV.reg = I
            NV.nivel = J(K2).nivel
            Dim id As Int32
            If J(K2 - 1).Der = Pila(K2) Then
                J(K2 - 1).Der = N1
                id = 1
            Else
                J(K2 - 1).Izq = N1
                id = -1
            End If
            J(K2).nivel = B1
            If K1 = K2 Then
                If Pila(K2) = N1 Or Pila(K2 - 1) = N1 Then
                    Throw New Exception($"Unknown error, unexpected Pila(k2)=N1 or Pila(k2-1)=N1")
                End If

                NV.Der = Pila(K2)
                NV.Izq = J(K2).Izq
                J(K2).Izq = 0
                Call WriteDirectory(J(K2 - 1), Pila(K2 - 1))
                Call WriteDirectory(J(K2), Pila(K2))
            Else
                If Pila(K2 - 1) = N1 OrElse Pila(K2) = N1 OrElse Pila(K1) = N1 Then
                    Throw New Exception($"Unknown error, unexpected Pila(k2-1) or Pila(k2) or Pila(k1) = N1")
                End If
                NV.Der = J(K2).Der ' Pila(K2 + 1)
                NV.Izq = J(K2).Izq
                J(K2).Izq = 0
                J(K2).Der = J(K1).Izq
                J(K1).Izq = Pila(K2)
                Call WriteDirectory(J(K2 - 1), Pila(K2 - 1))
                Call WriteDirectory(J(K2), Pila(K2))
                Call WriteDirectory(J(K1), Pila(K1))
            End If
            ' Call W(NV, N1) comentado 2020/06
salida:
            Call WriteDirectory(NV, N1) ' AÑADIDO 2020/06
            WriteKeyAndDataRegisters(clave, N1)

            J(0).lenClau += 1
            WriteDirectory(J(0), 0)
            Return True
        Catch ex As Exception
            Dim s1 As String = ex.ToString()
            Dim s2 As String = s1
            Throw New Exception(s1)
        End Try
    End Function
    Function IsBitSet(ByRef V1S As Byte(), ByVal lenv1S As Int32,
            ByVal indice As Int32) As Boolean

        ' Averiguar si el bit #índice está a cero
        If indice = 0 Then
            Throw New Exception($"Unknown error, unexpected index=0")
        End If
        Dim i, i1 As Int32
        indice -= 1
        i = Math.Floor(indice / 8) ' #byte
        i1 = 7 - indice Mod 8
        If i >= V1S.Length Then Return False
        If Not ((V1S(i) And 2 ^ i1) = 0) Then
            Return True
        Else
            Return False
        End If
    End Function
    Function ReadKeyAndDataRegisters(ByVal reg) As Boolean
        NAdd = vNA(reg) ' Get the register of the key
        n2S = TheFile(reg) ' Read the Data Register (contains
        Return True
    End Function
    Sub WriteKeyAndDataRegisters(ByVal key() As Byte, ByVal regClaves As Int32)
        TheFile(regClaves) = key ' Save key and data to file
        vNA(regClaves) = regClaves
    End Sub

    Function ReadDirectory(ByVal reg1 As Int32) As Nodo
        Return AD(reg1)
    End Function

    Sub WriteDirectory(ByVal M As Nodo, ByVal reg1 As Int32)
        AD(reg1) = M
    End Sub

    Sub FindDifferingBit_B1(ByVal V1S As Byte(), ByVal v1sn2s As Int32)
        Dim l, L0, b0 As Int32
        If V1S.Length > n2S.Length Then
            L0 = n2S.Length - 1
            l = n2S.Length * 8
        Else
            L0 = V1S.Length - 1
            l = V1S.Length * 8
        End If
        B1 = 1
        For b0 = 0 To L0
            If V1S(b0) = n2S(b0) Then
                B1 += 8
            Else
                Exit For
            End If
        Next
        For B1 = B1 To B1 + 8 'l
            If IsBitSet(V1S, V1S.Length, B1) <> IsBitSet(n2S, n2S.Length, B1) Then Exit For
        Next
        Return  ' añadido 27/3/2005
    End Sub
    Function ToStringAllKeys() As String
        Dim e1 As New StringBuilder(10000)
        Try
            Dim t As Int64 = Now.Ticks
            Dim vClave(0) As Byte
            Dim r As Boolean
            e1.Append("0 " + AD(0).ToString + vbCrLf)
            'NLecturas = 0
            Dim param As New didi20250424.SearchParams With {
                    .key = vClave,
                    .register = NAdd,
                    .mode = Modes.SearchNext
                }
            Do
                r = Search(param) ' ">"
                If Not r Then Exit Do
                Dim clave As String = System.Text.ASCIIEncoding.ASCII.GetString(param.key)
                e1.Append(NAdd.ToString + " " + clave + vbCrLf)
                'NLecturas += 1
            Loop
            'tiempoLecturas = New TimeSpan(Now.Ticks - t)
        Catch ex As Exception

        End Try
        Return e1.ToString
    End Function
    Sub CamviNA(
        ByRef OldNA As Int32, ByRef NewNA As Int32)
        Dim N As Nodo
        'Me.d1 = objDir
        N = ReadDirectory(OldNA)
        N.reg = NewNA
        WriteDirectory(N, NewNA)
    End Sub
    Public Structure move
        Dim K3 As Int32
        Dim j() As Nodo
        Dim pila() As Int32
        Dim NA As Int32
        Dim clau() As Byte
    End Structure
End Class
