Option Strict On
Option Explicit On
Imports VB = Microsoft.VisualBasic

Module MainModule
	
    Public Const maxMapSize As Integer = 3000
    Public Const maxSpecial As Integer = 120
    Public Const maxRoofs As Integer = 40
	
	
	Structure CustomObject
		Dim name As String
        Dim width As Integer
        Dim height As Integer
        <VBFixedArray(10, 10)> Dim Matrix(,) As Integer
		
		'UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
		Public Sub Initialize()
			ReDim Matrix(10, 10)
		End Sub
	End Structure
	
    Structure EditorRoof
        Dim anchor As Point
        Dim anchorTarget As Point
        Dim width As Integer
        Dim height As Integer
        <VBFixedArray(100, 100)> Dim Matrix(,) As Integer

        'UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
        Public Sub Initialize()
            ReDim Matrix(100, 100)
        End Sub
    End Structure

    Structure MyArea
        <VBFixedArray(6, 6)> Dim Matrix(,) As Integer

        'UPGRADE_TODO: "Initialize" must be called to initialize instances of this structure. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B4BFF9E0-8631-45CF-910E-62AB3970F27B"'
        Public Sub Initialize()
            ReDim Matrix(6, 6)
        End Sub
    End Structure

    
    Public TileSurface As ERY.AgateLib.Surface
    Public CharSurface As ERY.AgateLib.Surface

    Public backWidth As Integer
    Public backHeight As Integer

    Public x1, x2 As Integer
    Public y1, y2 As Integer
    Public dispX, dispY As Integer

    Public TheMap As ERY.Xle.XleMap

    <Obsolete()> Public mMap(maxMapSize, maxMapSize) As Integer
    <Obsolete()> Public mapWidth As Integer
    <Obsolete()> Public mapHeight As Integer
    <Obsolete()> Public mapName As New VB6.FixedLengthString(16)
    <Obsolete()> Public MapType As Integer
    <Obsolete()> Public MapBpT As Integer

    <Obsolete()> Public picTilesX As Integer
    <Obsolete()> Public picTilesY As Integer
    Public leftX, topy As Integer
    Public fileName As String
    Public defaultTile As Integer

    Public special(maxSpecial) As Integer
    Public specialx(maxSpecial) As Integer
    Public specialy(maxSpecial) As Integer
    Public specialdata(maxSpecial) As VB6.FixedLengthString
    Public specialwidth(maxSpecial) As Integer
    Public specialheight(maxSpecial) As Integer
    Public specialCount As Integer
    Public mail(3) As Integer

    Public currentTile As Integer
    Public fileOffset As Integer
    Public BuyRaftMap As Integer
    Public BuyRaftX As Integer
    Public BuyRaftY As Integer

    Public guard(100) As Point
    Public guardAttack As Integer
    Public guardDefense As Integer
    Public guardColor As Integer
    Public guardHP As Integer

    Public LotaPath As String
    Public Const TileSize As Integer = 16

    Public SelectedOK As Boolean
    Public UpdateScreen As Boolean
    Public TileSet As String
    Public StartNewMap As Boolean
    Public ImportMap As Boolean
    'UPGRADE_WARNING: Array PreDefObjects may need to have individual elements initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B97B714D-9338-48AC-B03F-345B617E2B02"'
    Public PreDefObjects(20) As CustomObject
    Public NumRoofs As Integer
    'UPGRADE_WARNING: Lower bound of array Roofs was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
    Public Roofs(maxRoofs) As EditorRoof

    Public ImportedData() As Byte
    Public AutoHeightWidth As Boolean
    Public ImportOffset As Integer
    Public TrimCrLf As Boolean

    Public ImportHeight As Integer
    Public ImportWidth As Integer
    Public AreaWidth, AreaHeight As Integer
    'UPGRADE_WARNING: Array Areas may need to have individual elements initialized. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B97B714D-9338-48AC-B03F-345B617E2B02"'
    Public Areas(255) As MyArea

    Public Enum EnumMapType
        mapMuseum = 1
        mapOutside
        maptown
        mapDungeon
        mapCastle
        mapTemple
    End Enum

    Public Sub SaveMapping(ByVal path As String)
        Dim ff As Integer
        Dim j, i, k As Integer

        ff = FreeFile

        FileOpen(ff, path, OpenMode.Output)

        PrintLine(ff, AreaWidth, AreaHeight)

        For i = 0 To 255
            For j = 0 To AreaHeight - 1
                For k = 0 To AreaWidth - 1
                    Print(ff, Areas(i).Matrix(j, k), TAB)
                Next
                PrintLine(ff, "")
            Next
        Next

        FileClose(ff)
    End Sub

    Public Sub LoadMapping(ByVal path As String)
        Dim j, i, k As Integer
        Dim ff As New StreamReader(File.Open(path, FileMode.Open, FileAccess.Read))

        AreaWidth = Integer.Parse(ff.ReadLine)
        AreaHeight = Integer.Parse(ff.ReadLine)


        For i = 0 To 255
            For j = 0 To AreaHeight - 1
                For k = 0 To AreaWidth - 1
                    Areas(i).Matrix(j, k) = Integer.Parse(ff.ReadLine)
                Next
            Next
        Next

        ff.Close()

        RecalibrateImport()
    End Sub

    Public Function Map(ByVal x As Integer, ByVal y As Integer) As Integer
        Return mMap(x, y)
    End Function
	
	Public Sub RecalibrateImport()
        Dim ii, i, j, jj As Integer
		'UPGRADE_NOTE: loc was upgraded to loc_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
        Dim src, loc_Renamed As Integer
		
		If AutoHeightWidth Then
			For i = ImportOffset To UBound(ImportedData)
				If ImportedData(i) = 13 And ImportedData(i + 1) = 10 Then
					ImportWidth = i
					Exit For
				End If
			Next 
			
            ImportWidth = ImportWidth + 2
            ImportHeight = (UBound(ImportedData) - ImportOffset) \ ImportWidth
			
			mapWidth = AreaWidth * (ImportWidth - 2)
			mapHeight = AreaHeight * ImportHeight
			
			
			TrimCrLf = True
		Else
			If TrimCrLf Then
				mapWidth = AreaWidth * (ImportWidth - 2)
			Else
				mapWidth = AreaWidth * ImportWidth
			End If
			
			If ImportHeight > (UBound(ImportedData) - ImportOffset) \ ImportWidth Then
				ImportHeight = (UBound(ImportedData) - ImportOffset) \ ImportWidth
			End If
			
			mapHeight = AreaHeight * ImportHeight
			
			
		End If
		
		For j = 0 To mapHeight - 1 Step AreaHeight
			For i = 0 To mapWidth - 1 Step AreaWidth
				loc_Renamed = ImportLocation(i, j)
				src = ImportedData(loc_Renamed)
				
				For jj = 0 To AreaHeight - 1
					For ii = 0 To AreaWidth - 1
						mMap(i + ii, j + jj) = Areas(src).Matrix(ii, jj)
					Next 
				Next 
				
			Next 
		Next 
		
		AutoHeightWidth = False
		
	End Sub
	
    Public Function ImportLocation(ByVal x As Integer, ByVal y As Integer) As Integer
        Return (y \ AreaHeight) * ImportWidth + x \ AreaWidth + ImportOffset
    End Function
	
	Public Sub ResetImportDefinitions()
		AreaWidth = 1
		AreaHeight = 1
		
        Dim j, i, k As Integer
		
		For i = 0 To 255
			For j = 0 To 6
				For k = 0 To 6
					Areas(i).Matrix(j, k) = i
				Next 
			Next 
		Next 
		
	End Sub
	
    Public Sub PaintArea(ByVal x As Integer, ByVal y As Integer, ByRef value As Integer)
        Dim sourceByte As Integer
        Dim ax, ay As Integer

        sourceByte = ImportLocation(x, y)

        ax = x Mod AreaWidth
        ay = y Mod AreaHeight

        Areas(ImportedData(sourceByte)).Matrix(ax, ay) = value

        RecalibrateImport()
    End Sub
	
	'UPGRADE_WARNING: Application will terminate when Sub Main() finishes. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="E08DDC71-66BA-424F-A612-80AF11498FF8"'
    Public Sub Main()
        Application.EnableVisualStyles()

        Dim i As Integer
        Dim j, k As Integer

        For i = 1 To maxRoofs

            Roofs(i).Initialize()

            For j = 0 To 100
                For k = 0 To 100

                    Roofs(i).Matrix(j, k) = 127
                Next
            Next
        Next

        For i = 1 To maxSpecial
            special(i) = 0
            specialx(i) = 0
            specialy(i) = 0

            specialwidth(i) = 1
            specialheight(i) = 1
        Next

        Randomize(VB.Timer())

        Using setup As New AgateSetup()
            setup.Initialize(True, False, False)
            If setup.Cancel Then Return

            frmMEdit.Show()

            Application.Run(frmMEdit)
        End Using
    End Sub
	
	
    Sub CreateSurfaces(ByRef width As Integer, ByRef height As Integer)

        FileManager.ImagePath = New SearchPath(Directory.GetCurrentDirectory & "/game/images")

        LotaPath = Directory.GetCurrentDirectory & "\..\game"


        CharSurface = New ERY.AgateLib.Surface("character.png")

    End Sub
	
	Sub LoadTiles(ByRef theFile As String)
        Dim j, i, o As Integer
        Dim ff As TextReader
        Dim name As String = ""
		
        TileSurface = New ERY.AgateLib.Surface(LotaPath & "\images\" & theFile)


		For i = 0 To 20
            With PreDefObjects(i)
                .name = ""
                .height = 0
                .width = 0

            End With
        Next
		
        Dim myfile As FileStream = File.Open(My.Application.Info.DirectoryPath & "\predef.txt", FileMode.Open, FileAccess.Read)
        ff = New StreamReader(myfile)

        Do
            name = ff.ReadLine()
        Loop Until name = Nothing Or LCase(name) = "[" & LCase(theFile) & "]"
		

        If Not name = Nothing Then
            Do
                name = ff.ReadLine()

                If name <> "" And Left(name, 1) <> "[" Then
                    With PreDefObjects(o)

                        .name = name
                        .width = Integer.Parse(ff.ReadLine())
                        .height = Integer.Parse(ff.ReadLine())

                        For i = 0 To .height - 1
                            For j = 0 To .width - 1
                                .Matrix(j, i) = Integer.Parse(ff.ReadLine())
                            Next
                        Next

                    End With

                    o = o + 1
                End If


            Loop Until name = Nothing Or name = "" Or Left(name, 1) = "["
        End If
		
		frmMEdit.lstPreDef.Items.Clear()
		
		For i = 0 To 20

			If PreDefObjects(i).name <> "" Then
                frmMEdit.lstPreDef.Items.Insert(i, PreDefObjects(i).name)
			End If
			
		Next 
	End Sub
	
End Module