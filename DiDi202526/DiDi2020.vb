Imports System
Imports System.IO
Imports System.Threading
Imports System.Text
Imports System.Runtime.InteropServices
Public Class didi2020 ' didiXml6
    Private fichDD As String, fichClaus As String, A1 As Int32, T As Int32, pos As Int32
    Private I As Int32, i1 As Int32, N1 As Int32, R1 As Int32, N1b As Int32, NBajas As Int32
    Private Dsp As Boolean, VS() As String, Inv As Boolean, St As Boolean
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
    Private NReg As Int32, NVReg As Int32, NEqReg As Int32
    Private NivelMas As Int32, B As Int32, B1 As Int32, B2 As Int32, D As Int32
    Private PNodo As Int32, RegPNodo As Int32
    'Const maxint32Clave = 255
    Private maxLenClau As Int32
    'Const longitud = maxint32Clave * 9
    Private longitud As Int32 = maxLenClau * 9
    'Private maxNumReg As Int32
    'Const longNodo = 4 * 4
    'Private J(longitud) As Nodo
    Private J() As Nodo
    Private K As Int32, KEq As Int32
    'private Pila(longitud) As Int32
    Private Pila() As Int32
    'Private V0(longitud + 1) As Boolean
    'Private N0(longitud + 1) As Boolean
    Private n2S As Byte()
    Private VarReg(3), K9S
    Private Accion As Int32
    Const comprueba = False
    Private AD() As Nodo
    Private data(-1)() As Byte
    Private vNA() As Int32
    'Private lenData As Int32 2020/06
    Public Mensaje As String
    Const hc = 1
    'Private fsDD As FileStream
    'Private brDD As BinaryReader
    'Private bwDD As BinaryWriter
    Private NA As Int32
    'Private VBajas(100000) As Boolean  ' para seguir qué claves se han dado de baixa
    ' Procedimiento de altas
    Public Shared Function ver() As String
        Return "6.0"
    End Function
    Public Sub New(ByVal directori() As Nodo,
        ByVal data()() As Byte, maxLenClaves As Int32) ' comentado 2020/06: , ByVal lenData As Int32)
        AD = directori
        Me.data = data
        ReDim vNA(AD.Length)
        'Me.lenData = lenData comentado 2020
        longitud = maxLenClaves * 8 + 1
        If longitud = 0 Then longitud = 128 * 9
    End Sub
    '<DllImport("..\dll\dd1.dll", EntryPoint:="setHandle")>
    'Public Shared Function setHandle(
    'ByVal hFile As IntPtr) As Int32
    'End Function
    Private Function anadir128(ByVal bytes As Byte()) As Byte()
        Dim b() As Byte = Nothing  ', i As Int32
        Try
            If bytes Is Nothing Then
                ReDim b(0)
                b(0) = 128
                Return b
            End If
            ReDim b(bytes.Length)
            b(0) = 128
            Array.Copy(bytes, 0, b, 1, bytes.Length)
            'For i = 0 To bytes.Length - 1
            'b(i + 1) = bytes(i)
            'Next
        Catch ex As Exception
            Dim s1 As String = ex.ToString()
            Dim s2 As String = s1
        End Try
        Return b
    End Function
    Private Function quitar128(ByVal bytes As Byte()) As Byte()
        Dim b() As Byte = Nothing  ', i As Int32
        If bytes Is Nothing Then Return b
        ReDim b(bytes.Length - 2)
        Array.Copy(bytes, 1, b, 0, b.Length)
        'For i = 1 To bytes.Length - 1
        '    b(i - 1) = bytes(i)
        'Next
        Return b
    End Function
    Function cerca(
        ByVal modo As String, ByRef clave As Byte(), ByRef NA As Int32) As Boolean
        Dim b1 As Boolean
        Me.NA = NA
        If modo = "alta" Then
            Dim i As Int32
            For i = clave.Length - 1 To 0 Step -1
                If clave(i) Then GoTo noCeros
            Next
            Mensaje = "El darrer byte de la clau no pot ser cero."
            Return False
        End If
noCeros:
        If (modo = "alta" Or modo = "baixa") AndAlso clave Is Nothing Then
            Mensaje = "La clau no pot ser nula."
            Return False
        End If
        clave = anadir128(clave) ' a todas las claves se les antepone 128 en binario
        Try
            '   fsClaus = New FileStream(fichClaus, FileMode.OpenOrCreate, FileAccess.ReadWrite, _
            'FileShare.None)
            '   fsDD = New FileStream(fichDD, FileMode.OpenOrCreate, FileAccess.ReadWrite, _
            'FileShare.None)
            '   fsDD.Lock(0, 16)
            'Do While lock() = 0
            '    System.Threading.Thread.Sleep(0)
            'Loop
            b1 = DirDif1(modo, clave, NA)
            'bwDD.Flush()
            'fsDD.Unlock(0, 16)
            'unlock()
            'System.Threading.Thread.Sleep(0)
        Catch e As Exception
            Dim s As String = e.ToString()
            Throw New Exception("ad3.bas  " & modo & " " & s _
            & " NA=" & Me.NA, New Exception)
            b1 = False
        End Try
        clave = quitar128(clave)
        If clave Is Nothing Then NA = -1 : Return False
        NA = Me.NA
        Return b1
    End Function
    'Public Function ocupacio() As Int32
    '    Do While lock() = 0
    '        System.Threading.Thread.Sleep(0)
    '    Loop
    '    Dim nzero As Nodo = R(0)
    '    unlock()
    '    Return nzero.lenClau
    'End Function
    '<DllImport("..\dll\dd1.dll", EntryPoint:="flush")>
    'Public Shared Function flush() As Int32
    'End Function
    '<DllImport("..\dll\dd1.dll", EntryPoint:="milock")>
    'Public Shared Function lock() As Int32
    'End Function
    '<DllImport("..\dll\dd1.dll", EntryPoint:="miunlock")>
    'Public Shared Function unlock() As Int32
    'End Function
    '<DllImport("..\dll\dd1.dll", EntryPoint:="Close")>
    'Public Shared Function Closenew() As Int32
    'End Function
    'Public Sub Close()
    '    Try
    '        'Closenew()
    '        fsDD.Flush()
    '        fsDD.Close()
    '    Catch
    '    End Try
    '    'fsClaus.Close()
    'End Sub
    Private Function DirDif1(
        ByVal modo As String, ByRef clave As Byte(),
         ByVal regNuevo As Int32) As Boolean
        ' Modo:
        ' alta
        ' baixa
        ' =
        ' >
        ' <
        Dim K1 As Int32, V1S As Byte()
        'mtx.WaitOne()
        'I = NA ' registro asociado mod.23/3/05
        N1 = regNuevo
        DirDif1 = True
        V1S = clave
        modo = LCase(modo)
        ReDim J((V1S.Length + 1) * 8), Pila((V1S.Length + 1) * 8)
        NReg = 0 ' nodo raíz árbol derecho
        N = R(NReg) : J(0) = N : Pila(0) = NReg
        If N.Der = 0 Then GoTo A1
        NReg = N.Der : D = 1 : N = R(NReg)
        'Else
        '  If N.Izq = 0 Then GoTo A1
        '  NReg = N.Izq: D = 0: N = R(NReg)
        'End If
        GoTo A2
A1:
        If modo <> "alta" Then NA = -1 : GoTo salfalse
        NV.Der = 0 : NV.Izq = 0
        For K1 = 1 To longitud
            'If V0(V1S, K1) = True Then NV.nivel = K1 : Exit For
            If V0(V1S, V1S.Length, K1) = True Then NV.nivel = K1 : Exit For
        Next
        NV.reg = NA ': Call W(NV, 1)
        Dim nnodo As Int32 = getNodo()
        J(0).nivel = 0 'J(0).Izq = nnodo ': N.reg = 0 ' 1 registro
        J(0).Der = nnodo 'If Mid(V1S, 2, 1) = "1" Then N.Der = NA Else N.Izq = NA
        J(0).lenClau += 1
        'J(0) = N
        'Call W(N, NReg)
        'GRABA(nnodo, NV, clave, 1) 2020/06
        Call W(NV, nnodo) ' añadido 2020/06 (nnodo se supone=1 )
        GRABA(clave, nnodo)
        Call W(J(0), 0)
        GoTo saltrue
A2:
        NEq.nivel = 0
        NV.Der = 0 : NV.Izq = 0 : NV.nivel = 0 : NV.reg = 0
        K = 0 : KEq = 0
        NEq = N : NEqReg = 1 : KEq = 1
        Do
            K = K + 1 : J(K) = N : Pila(K) = NReg : Pila(K + 1) = 0 : J(K + 1).nivel = 0
            'If V0(V1S, N.nivel) = True Then
            If V0(V1S, V1S.Length, N.nivel) = True Then
                D = 1 : NEq = N : NEqReg = NReg : KEq = K
                If N.Der = 0 Then Exit Do
                NReg = N.Der : N = R(NReg)
            Else
                D = 0
                If N.Izq = 0 Then Exit Do
                NReg = N.Izq : N = R(NReg)
            End If
        Loop Until N.nivel = 0
        If Not (LECTURA(NEqReg)) Then GoTo salfalse
        Dim rc As Int32 = Compara(V1S, n2S)
        'comentado 2020 Dim rc As Int32 = ComparaV0conN2(V1S, V1S.Length, n2S, n2S.Length, B1)
        If rc <> 0 Then CALCULO_B1(V1S, rc)
        If InStr(modo, "=") < 1 AndAlso
            InStr(modo, ">") > 0 OrElse InStr(modo, "<") > 0 Then
            Return Ajustar(modo, clave)
        End If
        If rc = 0 Then
            If InStr(modo, "=") > 0 Then
                NA = NEqReg
                GoTo saltrue ' añadido 2020/06
            ElseIf modo = "baixa" Then
                NA = NEqReg : I = NEqReg
                If Baixa(clave) Then
                    GoTo saltrue
                Else
                    GoTo salfalse
                End If
                'N1 = N1 - 1
                'setRegLliure(NA)
                'Recorrido(Me.d1, ">", "", 0, "", NA)
                'NA = NReg
            ElseIf modo = "alta" Then
                'NA = -1
                GoTo salfalse
            Else ' bajas
                Stop
                'VBajas(N1) = True
                R1 = R1 + 1
            End If
        Else
            If modo = "baixa" Then
                'Throw New Exception("Error en baixa", New Exception)
                GoTo salfalse
            ElseIf modo = "alta" Then
                '2020/06 Dim b1 As Boolean = Inclusio(V1S, clave, n2S, modo)
                Dim b1 As Boolean = Inclusio(V1S, clave, n2S)
                If b1 Then W(J(0), 0)
                Return b1
            Else
                Mensaje = "Modus desconegut."
                GoTo salfalse
            End If
        End If
        '    ver (0): Debug.Print: Stop
salfalse:
        'mtx.ReleaseMutex()
        Return False
saltrue:
        'mtx.ReleaseMutex()
        Return True
    End Function
    '<DllImport("..\dll\dd1.dll", EntryPoint:="ComparaV0conN2")>
    'Public Shared Function ComparaV0conN2(
    'ByVal V1S() As Byte, ByVal v1len As Int32,
    'ByVal n2s() As Byte, ByVal n2len As Int32,
    '<InAttribute(), OutAttribute()> ByRef B1 As Int32) As Int32
    'End Function
    'Public Shared Function ComparaV0conN2(
    'ByVal V1S() As Byte, ByVal v1len As Int32,
    'ByVal n2s() As Byte, ByVal n2len As Int32,
    '<InAttribute(), OutAttribute()> ByRef B1 As Int32) As Int32
    '    Dim r As Int32 = 0
    '    Try
    '        Dim i, j As Int32
    '        For i = 0 To Math.Min(v1len, n2len) - 1
    '            Dim a As Byte = V1S(i)
    '            Dim b As Byte = n2s(i)
    '            For j = 0 To 7
    '                If (a And 2 ^ j) <> (b And 2 ^ j) Then Exit For
    '            Next
    '            If j < 8 Then Exit For
    '        Next
    '        If i = v1len AndAlso i = n2len AndAlso j = 8 Then
    '            r = v1len * 8 ' ??
    '        Else
    '            r = i * 8 + j
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    '    Return r
    'End Function

    Private Function getNodo() As Int32
        Dim nnodo As Int32
        If J(0).reg = 0 Then
            J(0).Izq += 1
            nnodo = J(0).Izq
        Else
            nnodo = J(0).reg
            Dim nd As Nodo = R(J(0).reg)
            J(0).reg = nd.reg
            nd.reg = 0
        End If
        'W(Nzero, 0)
        'J(0) =  'R(0)
        W(J(0), 0) ' descomentado 2020/06
        Return nnodo
    End Function
    Private Sub remouNode(ByVal Nnodo As Int32)
        Dim nd As Nodo = R(Nnodo)
        Dim ult As Int32 = J(0).reg
        nd.reg = ult
        J(0).reg = Nnodo
        W(nd, Nnodo)
    End Sub
    '<DllImport("..\dll\dd1.dll", EntryPoint:="compara")>
    'Public Shared Function compara(
    'ByVal b1() As Byte, ByVal b1len As Int32,
    'ByVal b2() As Byte, ByVal b2len As Int32) As Int32
    'End Function
    Public Shared Function Compara(ByVal b1 As Byte(), ByVal b2 As Byte()) As Int32
        Dim l1 As Int32 = b1.Length
        Dim l2 As Int32 = b2.Length
        Dim l, i As Int32
        If l1 >= l2 Then
            l = l2
        Else
            l = l1
        End If
        'Dim by As Int32
        For i = 0 To l - 1
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
        Else
            Return -1
        End If
    End Function
    Private Function Ajustar(ByVal modo As String, ByRef clave As Byte()) As Boolean
        Dim sig As Int32, Nsig As Nodo
        Dim clau() As Byte = Nothing
        Dim na1
        Nsig = R(0)
        sig = Nsig.Der
        na1 = -1
        If sig = 0 Then
            GoTo salida
        End If
        Dim es As String =
            System.Text.ASCIIEncoding.ASCII.GetString(clave)
        Do
            Nsig = R(sig)
            'If Not LECTURA(Nsig.reg) Then
            If Not (LECTURA(sig)) Then
                GoTo salida
            End If
            Dim cmp As Int32 = Compara(n2S, clave)
            'Dim cmp As Int32 = Compara(n2S, n2S.Length, clave, clave.Length)
            If cmp < 1 Then ' n2S <= clave Then
                If InStr(modo, "<") > 0 Then
                    If cmp = 0 Then GoTo aelse
                    na1 = Nsig.reg  '=Nsig.reg
                    ReDim clau(n2S.Length - 1)
                    Array.Copy(n2S, clau, n2S.Length)
                    'clau = n2S
                End If
                If Nsig.Der = 0 Then
                    GoTo salida
                Else
                    sig = Nsig.Der
                End If
            Else
aelse:
                If InStr(modo, ">") > 0 Then
                    na1 = Nsig.reg
                    ReDim clau(n2S.Length - 1)
                    Array.Copy(n2S, clau, n2S.Length)
                    'clau = n2S
                End If
                If Nsig.Izq = 0 Then
                    GoTo salida
                Else
                    sig = Nsig.Izq
                End If
            End If
        Loop
salida:
        NA = na1
        If NA = -1 Then Return False
        'clave = clau
        ReDim clave(clau.Length - 1)
        Array.Copy(clau, clave, clau.Length)
        Return True
    End Function
    Private Function Baixa(ByRef clave As Byte())
        If Pila(KEq) <> I Then
            Return False
        End If
        If J(KEq).Der = 0 Then
            If J(KEq - 1).Der = Pila(KEq) Then
                J(KEq - 1).Der = J(KEq).Izq
            Else
                J(KEq - 1).Izq = J(KEq).Izq
            End If
            Call W(J(KEq - 1), Pila(KEq - 1))
            NA = J(KEq).reg ' NA=Pila(KEq) 3/julio/2004
            'Recorrido(d1, ">", "", 0, "", NA)
        Else
            K = KEq + 1 : J(K) = R(J(KEq).Der) : Pila(K) = J(KEq).Der
            Do While J(K).Izq <> 0
                K = K + 1
                Pila(K) = J(K - 1).Izq
                J(K) = R(J(K - 1).Izq)
            Loop
            If J(KEq - 1).Der = Pila(KEq) Then
                J(KEq - 1).Der = Pila(K)
            Else
                J(KEq - 1).Izq = Pila(K)
            End If
            If K = KEq + 1 Then
                ' ¡¡¡ MODIFICADO !!! 26/ABRIL/2003
                J(K).Izq = J(KEq).Izq
                J(K).nivel = J(KEq).nivel
                Call W(J(KEq - 1), Pila(KEq - 1))
                Call W(J(K), Pila(K))
            Else
                ' ¡¡¡ MODIFICADO !!! 26/ABRIL/2003
                J(K - 1).Izq = J(K).Der
                J(K).nivel = J(KEq).nivel
                J(K).Izq = J(KEq).Izq
                J(K).Der = J(KEq).Der
                Call W(J(KEq - 1), Pila(KEq - 1))
                Call W(J(K - 1), Pila(K - 1))
                Call W(J(K), Pila(K))
            End If
            NA = J(KEq).reg ' NA= Pila(KEq) ' 6/Julio/2004
        End If
        Dim nd As Nodo = J(KEq)
        Dim l1 As Int32 = nd.lenClau
        Me.remouNode(Pila(KEq))
        Do While l1 > 0 And nd.sigRegClau > 0
            Me.remouNode(nd.sigRegClau)
            nd = R(nd.sigRegClau)
            l1 -= Nodo.lenNodo
        Loop
        J(0).lenClau -= 1
        W(J(0), 0)
        Return True
    End Function
    Private Function Inclusio(ByVal V1S As Byte(), ByRef clave As Byte(),
            ByVal n2S As Byte()) ' 2020 comentado el parámetro siguiente: , ByVal modo As String)
        'Dim I2 As Int32
        Dim K1 As Int32, K2 As Int32 ', K3 As Int32
        'Dim count As Int32
        'Dim Nzero As Nodo = R(0)
        Try
            N1 = Me.getNodo()
            ' comentado 2020/06: I = Me.NA 'N1
            I = N1
            'If I = &H1A Then Stop
            'I = N1 ' añadido pues claves (en fichClaus) y nodos (en fichDD)
            '       ocupan mismo registro
            'Dim NV As Nodo = R(N1)
            Dim NV As New Nodo
            NV.Izq = 0 : NV.Der = 0
            If Compara(V1S, n2S) = 1 Then ' > n2S Then
                'If compara(V1S, V1S.Length, n2S, n2S.Length) = 1 Then ' > n2S Then
                K1 = 1
                For K2 = 1 To K
                    If J(K2).nivel > B1 Then Exit For
                    K1 = K2
                Next
                If Pila(K1) = N1 Then Stop ' ¡¡¡¡ !!!!!
                'If J(K1).Izq = Pila(K1 + 1) AndAlso _
                'V0(V1S, J(K1).nivel) = False Then
                If J(K1).Izq = Pila(K1 + 1) AndAlso
                    V0(V1S, V1S.Length, J(K1).nivel) = False Then
                    NV.nivel = B1
                    NV.reg = I
                    J(K1).Izq = N1
                    NV.Izq = Pila(K1 + 1)
                    'Call W(NV, N1) 
                    Call W(J(K1), Pila(K1))
                Else
                    NV.nivel = B1
                    NV.reg = I
                    J(K1).Der = N1
                    NV.Izq = Pila(K1 + 1)
                    'Call W(NV, N1) 
                    Call W(J(K1), Pila(K1))
                End If
                GoTo salida
                'Return True
            End If
            For K1 = K To 2 Step -1
                If B1 > J(K1).nivel Then Exit For
            Next
            For K2 = K1 To 2 Step -1
                'If V0(V1S, J(K2).nivel) = True Then Exit For
                If V0(V1S, V1S.Length, J(K2).nivel) = True Then Exit For
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
                If Pila(K2) = N1 Or Pila(K2 - 1) = N1 Then Stop ' ¡¡¡¡!!!!
                NV.Der = Pila(K2)
                NV.Izq = J(K2).Izq
                J(K2).Izq = 0
                Call W(J(K2 - 1), Pila(K2 - 1))
                Call W(J(K2), Pila(K2))
            Else
                If Pila(K2 - 1) = N1 OrElse Pila(K2) = N1 OrElse Pila(K1) = N1 Then
                    Throw New Exception("Error en alta")
                    Stop ' ¡¡¡¡¡!!!!!
                End If
                NV.Der = J(K2).Der ' Pila(K2 + 1)
                NV.Izq = J(K2).Izq
                J(K2).Izq = 0
                J(K2).Der = J(K1).Izq
                J(K1).Izq = Pila(K2)
                Call W(J(K2 - 1), Pila(K2 - 1))
                Call W(J(K2), Pila(K2))
                Call W(J(K1), Pila(K1))
            End If
            ' Call W(NV, N1) comentado 2020/06
salida:
            Call W(NV, N1) ' AÑADIDO 2020/06
            GRABA(clave, N1)


            'Dim NV1 As Nodo = R(N1)
            'GRABA(N1, NV, V1S, 1)
            J(0).lenClau += 1
            W(J(0), 0)
            Return True
        Catch ex As Exception
            Dim s1 As String = ex.ToString()
            Dim s2 As String = s1
            Throw New Exception(s1)
        End Try
    End Function
    Public Function mou(ByRef mv As move, ByVal modo As String) As Boolean
        If modo = "<" Then Return Me.mouAnt(mv)
        'Dim i As Int32
        Dim K3 As Int32 = mv.K3
        If K3 = 0 Then
            ReDim mv.j(0) : mv.j(0) = R(0) : ReDim mv.pila(0) : mv.pila(0) = 0
        End If
        If InStr(modo, ">") > 0 Then
dcha:
            If mv.j(K3).Der <> 0 Then
                mv.K3 += 1 : K3 = mv.K3
                ReDim Preserve mv.j(K3), mv.pila(K3)
                mv.pila(K3) = mv.j(K3 - 1).Der
                mv.j(K3) = R(mv.pila(K3))
                If mv.j(K3).Izq = 0 Then
                    mv.NA = mv.j(K3).reg
                    If mv.j(K3).reg = 0 Then Stop
                    GoTo salTrue
                Else
                    Do While mv.j(K3).Izq <> 0
                        mv.K3 += 1 : K3 = mv.K3
                        ReDim Preserve mv.j(K3), mv.pila(K3)
                        mv.pila(K3) = mv.j(K3 - 1).Izq
                        mv.j(K3) = R(mv.pila(K3))
                    Loop
                    mv.NA = mv.j(K3).reg
                    If mv.j(K3).reg = 0 Then Stop
                    GoTo salTrue
                End If
            End If
            Do While K3 > 1
                If mv.j(K3 - 1).Izq = mv.pila(K3) Then
                    mv.K3 -= 1
                    K3 = mv.K3
                    mv.NA = mv.j(K3).reg
                    If mv.j(K3).reg = 0 Then Stop
                    mv.j(K3) = R(mv.pila(K3))
                    GoTo salTrue
                End If
                mv.K3 -= 1
                K3 = mv.K3
            Loop
            If K3 < 2 Then
                mv.NA = -1 : Return False
            Else
                mv.NA = mv.j(K3 - 1).reg
                mv.K3 -= 1
                K3 = mv.K3
                If mv.j(K3).reg = 0 Then Stop
                GoTo salTrue
            End If
            If mv.j(K3).reg = 0 Then Stop
        End If
salTrue:
        Dim b2 As Int32 = mv.j(K3).nivel
        Dim eStr0 As String = Hex(mv.j(K3).nivel)
        If Len(eStr0) < 2 Then eStr0 = "0" + eStr0
        Dim eStr = Hex(mv.pila(K3))
        If Len(eStr) < 2 Then eStr = "0" + eStr
        Dim eStr1 = Hex(mv.j(K3).Izq)
        If Len(eStr1) < 2 Then eStr1 = "0" + eStr1
        Dim eStr2 = Hex(mv.j(K3).Der)
        If Len(eStr2) < 2 Then eStr2 = "0" + eStr2
        If b2 = 0 Then Stop
        Console.Write(eStr0 + " " + eStr + " " + eStr1 + " " + eStr2 + " ")
        Return True
    End Function
    Private Function mouAnt(ByRef mv As move) As Boolean
        'Dim i As Int32
        Dim K3 As Int32 = mv.K3
        If K3 = 0 Then
            ReDim mv.j(0) : mv.j(0) = R(0) : ReDim mv.pila(0) : mv.pila(0) = 0
            Do While mv.j(K3).Der <> 0
                K3 += 1 : mv.K3 = K3
                ReDim Preserve mv.j(K3), mv.pila(K3)
                mv.pila(K3) = mv.j(K3 - 1).Der
                mv.j(K3) = R(mv.pila(K3))
            Loop
            mv.NA = mv.j(K3).reg
            GoTo salTrue
        End If
        If mv.j(K3).Izq = 0 Then
            K3 -= 1 : mv.K3 = K3
            If K3 < 1 Then
                mv.NA = -1 : Return False
            Else
                mv.NA = mv.j(K3).reg
                GoTo salTrue
            End If
        Else
            'K3 += 1 : mv.K3 = K3
            'ReDim Preserve mv.j(K3), mv.pila(K3)
            mv.pila(K3) = mv.j(K3).Izq
            mv.j(K3) = R(mv.pila(K3))
            Do While mv.j(K3).Der <> 0
                K3 += 1 : mv.K3 = K3
                ReDim Preserve mv.j(K3), mv.pila(K3)
                mv.pila(K3 - 1) = mv.j(K3 - 1).Der
                mv.j(K3) = R(mv.pila(K3 - 1))
            Loop
            mv.NA = mv.j(K3).reg
            GoTo salTrue
        End If

salTrue:
        Dim b2 As Int32 = mv.j(K3).nivel
        Dim eStr0 As String = Hex(mv.j(K3).nivel)
        If Len(eStr0) < 2 Then eStr0 = "0" + eStr0
        Dim eStr = Hex(mv.pila(K3))
        If Len(eStr) < 2 Then eStr = "0" + eStr
        Dim eStr1 = Hex(mv.j(K3).Izq)
        If Len(eStr1) < 2 Then eStr1 = "0" + eStr1
        Dim eStr2 = Hex(mv.j(K3).Der)
        If Len(eStr2) < 2 Then eStr2 = "0" + eStr2
        If b2 = 0 Then Stop
        Console.Write(eStr0 + " " + eStr + " " + eStr1 + " " + eStr2 + " ")
        Return True
    End Function
    '<DllImport("..\dll\dd1.dll", EntryPoint:="v0")>
    'Public Shared Function v0(
    'ByVal v1() As Byte, ByVal v1len As Int32,
    'ByVal indice As Int32) As Boolean
    'End Function
    Function V0(ByRef V1S As Byte(), ByVal lenv1S As Int32,
            ByVal indice As Int32) As Boolean

        ' Averiguar si el bit #índice está a cero
        If indice = 0 Then Stop
        Dim i, i1 As Int32
        indice -= 1
        i = Math.Floor(indice / 8) ' #byte
        i1 = 7 - indice Mod 8
        If i >= V1S.Length Then Return False
        Try
            If Not ((V1S(i) And 2 ^ i1) = 0) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Dim s1 As String = ex.ToString()
            Dim s2 As String = s1
            Throw New Exception(s1)
        End Try
    End Function
    Function N0old(ByRef N2S As Byte(), ByVal indice As Int32) As Boolean
        Dim i, i1 As Int32
        indice -= 1
        i = Math.Floor(indice / 8)
        i1 = 7 - indice Mod 8
        If (N2S(i) And 2 ^ i1) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Function LECTURA(ByVal reg) As Boolean
        NA = vNA(reg)
        n2S = data(reg)
        'Dim data(127) As Byte
        'fsClaus.Seek(reg * 132, SeekOrigin.Begin)
        'Dim l1 As Byte = brCl.ReadByte() - 1 'Data(reg * lenData)
        'RegPNodo = reg
        'ReDim n2S(l1 - 1)
        'n2S = brCl.ReadBytes(l1)
        'NA = brCl.ReadInt32()
        'n2S = anadir128(n2S) 'Chr(128) & n2S
        Return True
    End Function
    Sub GRABA(ByVal key() As Byte, ByVal regClaves As Int32)
        data(regClaves) = key
        vNA(regClaves) = regClaves
        'Dim l1 As Byte = key.Length
        'ReDim Preserve key(127)
        'If l1 > 127 Then
        '    Throw New Exception("Error. La clau es de més de 127 bytes")
        'End If
        'fsClaus.Seek(regClaves * 132, SeekOrigin.Begin)
        'key(0) = l1
        'bwCl.Write(key, 0, l1)
        'bwCl.Write(NA)
        'key(0) = 128
    End Sub
    Private Function LECTURAantesDe202006(ByVal reg As Int32) As Boolean
        Dim nd As New Nodo
        nd = R(reg)
        ReDim n2S(nd.lenClau)
        'LECTURAclau(nd.sigRegClau, n2S, nd.lenClau)
        Return True
    End Function
    '<DllImport("..\dll\dd1.dll", EntryPoint:="LECTURAclau")>
    'Public Shared Function LECTURAclau(
    'ByVal sigClau As Int32,
    '<InAttribute(), OutAttribute()> ByVal clau() As Byte,
    'ByVal lenAGrabar As Int32) As Int32
    'End Function
    'Private Sub GRABA(ByVal reg As Int32, ByVal nd As Nodo, ByVal clau() As Byte, ByVal index As Int32)
    '    nd.lenClau = clau.Length - index
    '    Dim l1 As Int32 = nd.lenClau
    '    Dim l2 As Int32 = Nodo.lenNodo - 4
    '    Dim sigClau As Int32 = getNodo()
    '    nd.sigRegClau = sigClau
    '    Call W(nd, reg)
    '    Do While l1 > 0
    '        If l1 <= l2 Then
    '            'fsDD.Seek(sigClau * Nodo.lenNodo, SeekOrigin.Begin)
    '            'sigClau = 0
    '            'bwDD.Write(sigClau)
    '            'bwDD.Write(clau, index, l1)
    '            GRABAclau(sigClau, 0, clau(index), l1)
    '            l1 = 0
    '        Else
    '            Dim sigClau1 As Int32 = getNodo()
    '            ' necesitamos 2 vars. sigClau pues en getNodo se pierde el 'seek'
    '            'fsDD.Seek(sigClau * Nodo.lenNodo, SeekOrigin.Begin)
    '            'bwDD.Write(sigClau1)
    '            'bwDD.Write(clau, index, l2)
    '            'sigClau = sigClau1
    '            GRABAclau(sigClau, sigClau1, clau(index), l2)
    '            index += l2
    '            l1 -= l2
    '        End If
    '    Loop
    '    'bwDD.Flush()
    'End Sub
    '<DllImport("..\dll\dd1.dll", EntryPoint:="GRABAclau")>
    'Public Shared Function GRABAclau(
    'ByVal reg1 As Int32, ByVal sigClau As Int32,
    'ByRef clau As Byte, ByVal lenAGrabar As Int32) As Int32
    'End Function



    Function R(ByVal reg1 As Int32) As Nodo
        Return AD(reg1)
    End Function




    '<DllImport("..\dll\dd1.dll", EntryPoint:="R")>
    'Public Shared Function Rd(
    'ByVal reg1 As Int32,
    '<InAttribute(), OutAttribute()> ByRef nd As Nodo) As Int32
    'End Function
    'Function R(ByVal reg1 As Int32) As Nodo
    '    'R = AD(reg1)
    '    '<FieldOffset(0)> Public sigRegClau As Int32
    '    '<FieldOffset(4)> Public nivel As Int32
    '    '<FieldOffset(8)> Public Der As Int32
    '    '<FieldOffset(12)> Public Izq As Int32
    '    '<FieldOffset(16)> Public reg As Int32
    '    '<FieldOffset(20)> Public lenClau As Int32
    '    Dim nd As New Nodo
    '    Try
    '        fsDD.Flush()
    '        Rd(reg1, nd)
    '        Return nd
    '        'fsDD.Seek(reg1 * Nodo.lenNodo, SeekOrigin.Begin)
    '        'nd.sigRegClau = brDD.ReadInt32()
    '        'nd.nivel = brDD.ReadInt32()
    '        'nd.Der = brDD.ReadInt32()
    '        'nd.Izq = brDD.ReadInt32()
    '        'nd.reg = brDD.ReadInt32()
    '        'nd.lenClau = brDD.ReadInt32()
    '    Catch ex As Exception
    '        Dim s1 As String = ex.ToString()
    '        Dim s2 As String = s1
    '    End Try
    '    Return nd
    'End Function
    '<DllImport("..\dll\dd1.dll", EntryPoint:="W")>
    'Public Shared Function W(
    'ByRef M As Nodo,
    'ByVal reg1 As Int32) As Int32
    'End Function


    Sub W(ByVal M As Nodo, ByVal reg1 As Int32)
        AD(reg1) = M
    End Sub

    Sub CALCULO_B1(ByVal V1S As Byte(), ByVal v1sn2s As Int32)
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
            ''If V0(V1S, B1) <> N0(n2S, B1) Then Exit For
            If V0(V1S, V1S.Length, B1) <> V0(n2S, n2S.Length, B1) Then Exit For
        Next
        Return  ' añadido 27/3/2005
        If v1sn2s = 1 Then
            For B1 = B1 To V1S.Length * 8
                'If V0(V1S, B1) Then Exit Sub
                If V0(V1S, V1S.Length, B1) Then Exit Sub
            Next
        Else
            For B1 = B1 To n2S.Length * 8
                'If N0(n2S, B1) Then Exit Sub
                If V0(n2S, n2S.Length, B1) Then Exit Sub
            Next
        End If
    End Sub
    Public Shared NLecturas As Int32
    Public Shared tiempoLecturas As TimeSpan
    Function AStr() As String
        Dim e1 As New StringBuilder(10000)
        Try
            Dim t As Int64 = Now.Ticks
            Dim vClave(0) As Byte
            Dim r As Boolean
            e1.Append("0 " + AD(0).ToString + vbCrLf)
            NLecturas = 0
            Do
                r = cerca(">", vClave, NA)
                If Not r Then Exit Do
                Dim clave As String = System.Text.ASCIIEncoding.ASCII.GetString(vClave)
                e1.Append(NA.ToString + " " + clave + vbCrLf)
                'e1.Append(NA.ToString + " " + AD(NA).ToString + vbCrLf)
                NLecturas += 1
            Loop
            tiempoLecturas = New TimeSpan(Now.Ticks - t)
        Catch ex As Exception

        End Try
        Return e1.ToString
    End Function
    Public Overrides Function ToString() As String
        Return astr()
    End Function
    Sub CamviNA(
        ByRef OldNA As Int32, ByRef NewNA As Int32)
        Dim N As Nodo
        'Me.d1 = objDir
        N = R(OldNA)
        N.reg = NewNA
        W(N, NewNA)
    End Sub
    Public Structure move
        Dim K3 As Int32
        Dim j() As Nodo
        Dim pila() As Int32
        Dim NA As Int32
        Dim clau() As Byte
    End Structure
End Class
