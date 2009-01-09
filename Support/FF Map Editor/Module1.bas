Attribute VB_Name = "MainModule"
Option Explicit

Global Const maxMapSize = 3000
Global Const maxSpecial = 120
Global Const maxRoofs = 40

Type Point
    x As Integer
    y As Integer
End Type

Type CustomObject
    name As String
    width As Integer
    height As Integer
    Matrix(10, 10) As Integer
End Type

Type Roof
    anchor As Point
    anchorTarget As Point
    width As Integer
    height As Integer
    Matrix(100, 100) As Integer
End Type

Type MyArea
    Matrix(6, 6) As Integer
End Type

Global DX7 As New DirectX7
Global DDraw7 As DirectDraw7

Global DDSBack As DirectDrawSurface7
Global DDSPrimary As DirectDrawSurface7
Global DDSTiles As DirectDrawSurface7
Global DDSTemp As DirectDrawSurface7
Global DDSChar As DirectDrawSurface7

Global ddsd As DDSURFACEDESC2
Global ddsd1 As DDSURFACEDESC2
Global ddClipper As DirectDrawClipper

Global backWidth As Long
Global backHeight As Long

Global x1 As Integer, x2 As Integer
Global y1 As Integer, y2 As Integer
Global dispX As Integer, dispY As Integer

Global mMap(maxMapSize, maxMapSize) As Integer
Global mapWidth As Integer
Global mapHeight As Integer
Global mapName As String * 16
Global MapType As Integer
Global MapBpT As Integer

Global picTilesX As Integer
Global picTilesY As Integer
Global leftX As Integer, topy As Integer
Global fileName As String
Global defaultTile As Integer

Global special(maxSpecial) As Integer
Global specialx(maxSpecial) As Integer, specialy(maxSpecial) As Integer
Global specialdata(maxSpecial) As String * 100
Global specialwidth(maxSpecial) As Integer, specialheight(maxSpecial) As Integer
Global specialCount As Integer
Global mail(0 To 3) As Integer

Global currentTile As Integer
Global fileOffset As Integer
Global BuyRaftMap As Integer
Global BuyRaftX As Integer
Global BuyRaftY As Integer

Global guard(100) As Point
Global guardAttack As Integer
Global guardDefense As Integer
Global guardColor As Integer
Global guardHP As Integer

Global LotaPath As String
Global Const TileSize = 16

Global SelectedOK As Boolean
Global UpdateScreen As Boolean
Global TileSet As String
Global StartNewMap As Boolean
Global ImportMap As Boolean
Global PreDefObjects(20) As CustomObject
Global NumRoofs As Integer
Global Roofs(1 To maxRoofs) As Roof

Global ImportedData() As Byte
Global AutoHeightWidth As Boolean
Global ImportOffset As Integer
Global TrimCrLf As Boolean

Global ImportHeight As Integer
Global ImportWidth As Integer
Global AreaWidth As Integer, AreaHeight As Integer
Global Areas(0 To 255) As MyArea

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
    Dim i As Integer, j As Integer, k As Integer
    
    ff = FreeFile
    
    Open path For Output As #ff
    
    Print #ff, AreaWidth, AreaHeight
    
    For i = 0 To 255
        For j = 0 To AreaHeight - 1
            For k = 0 To AreaWidth - 1
                Print #ff, Areas(i).Matrix(j, k),
            Next
            Print #ff, ""
        Next
    Next
    
    Close #ff
End Sub

Public Sub LoadMapping(ByVal path As String)
    Dim ff As Integer
    Dim i As Integer, j As Integer, k As Integer
    
    ff = FreeFile
    
    Open path For Input As #ff
    
    Input #ff, AreaWidth, AreaHeight
    
    For i = 0 To 255
        For j = 0 To AreaHeight - 1
            For k = 0 To AreaWidth - 1
                Input #ff, Areas(i).Matrix(j, k)
            Next
        Next
    Next
    
    Close #ff
    
    RecalibrateImport
End Sub

Public Function Map(ByVal x As Integer, ByVal y As Integer) As Integer
    Map = mMap(x, y)
End Function

Public Sub RecalibrateImport()
    Dim i As Integer, j As Integer, ii As Integer, jj As Integer
    Dim src As Integer, loc As Integer
    
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
            loc = ImportLocation(i, j)
            src = ImportedData(loc)
            
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
    ImportLocation = (y \ AreaHeight) * ImportWidth + x \ AreaWidth + ImportOffset
End Function

Public Sub ResetImportDefinitions()
    AreaWidth = 1
    AreaHeight = 1
    
    Dim i As Integer, j As Integer, k As Integer
    
    For i = 0 To 255
        For j = 0 To 6
            For k = 0 To 6
                Areas(i).Matrix(j, k) = i
            Next
        Next
    Next
    
End Sub

Public Sub PaintArea(ByVal x As Integer, ByVal y As Integer, value As Integer)
    Dim sourceByte As Integer
    Dim ax As Integer, ay As Integer
    
    sourceByte = ImportLocation(x, y)
    
    ax = x Mod AreaWidth
    ay = y Mod AreaHeight
    
    Areas(ImportedData(sourceByte)).Matrix(ax, ay) = value
    
    RecalibrateImport
End Sub

Sub Main()
    Dim i As Integer, j, k
    
    For i = 1 To maxRoofs
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
    
    Randomize Timer

    Set DDraw7 = DX7.DirectDrawCreate("")
        
    frmMEdit.show
    
End Sub

Sub CreateBackBuffer(width As Integer, height As Integer)

    ddsd1.lFlags = DDSD_CAPS Or DDSD_WIDTH Or DDSD_HEIGHT
    ddsd1.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
    ddsd1.lWidth = width
    ddsd1.lHeight = height
    
    backWidth = ddsd1.lWidth
    backHeight = ddsd1.lHeight
    

    Set DDSBack = DDraw7.CreateSurface(ddsd1)
    
End Sub

Sub CreateSurfaces(width As Integer, height As Integer)
    
    Call DDraw7.SetCooperativeLevel(frmMEdit.hWnd, DDSCL_NORMAL)
   
    ddsd.lFlags = DDSD_CAPS
    ddsd.ddsCaps.lCaps = DDSCAPS_PRIMARYSURFACE
    
    Set DDSPrimary = DDraw7.CreateSurface(ddsd)
    
    CreateBackBuffer width, height
    
    ddsd.lFlags = DDSD_CAPS Or DDSD_CKSRCBLT Or DDSD_WIDTH Or DDSD_HEIGHT
    ddsd.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
    ddsd.lWidth = 512
    ddsd.lHeight = 512
    
    LotaPath = App.path & "\.."
    
    Set DDSChar = DDraw7.CreateSurfaceFromFile(LotaPath & "\images\character.bmp", ddsd)
    Set DDSTemp = DDraw7.CreateSurface(ddsd)
    
    Set ddClipper = DDraw7.CreateClipper(0)

    DDSBack.SetForeColor vbWhite
End Sub

Sub LoadTiles(theFile As String)
    Dim i, j, o
    Dim file
    Dim name As String
    
    ddsd.lFlags = DDSD_CAPS Or DDSD_CKSRCBLT Or DDSD_WIDTH Or DDSD_HEIGHT
    ddsd.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
    ddsd.lWidth = 512
    ddsd.lHeight = 256
    
    Set DDSTiles = DDraw7.CreateSurfaceFromFile(LotaPath & "\images\" & theFile, ddsd)

    For i = 0 To 20
        With PreDefObjects(i)
        
            .name = ""
            .height = 0
            .width = 0
            
        End With
    Next

    file = FreeFile
    Open App.path & "\predef.txt" For Input As #file
    
    Do Until LCase(name) = "[" & LCase(theFile) & "]" Or EOF(file)
        Input #file, name
    Loop
    
    If Not EOF(file) Then
        Do
            Input #file, name
            If name <> "" And Left(name, 1) <> "[" Then
                With PreDefObjects(o)
                    
                    .name = name
                                
                    Input #file, name
                    .width = Val(name)
                    Input #file, name
                    .height = Val(name)
                                    
                    For i = 0 To .height - 1
                        For j = 0 To .width - 1
                            Input #file, name
                            .Matrix(j, i) = Val(name)
                        Next
                    Next
                    
                End With
                
                o = o + 1
            End If
            
        Loop Until EOF(file) Or name = "" Or Left(name, 1) = "["
    End If
    
    frmMEdit.lstPreDef.Clear
    
    For i = 0 To 20
        If PreDefObjects(i).name <> "" Then
            frmMEdit.lstPreDef.AddItem PreDefObjects(i).name, i
        End If
        
    Next
End Sub

Public Function min(a, b)
    If a < b Then min = a Else min = b
End Function

Public Function max(a, b)
    If a < b Then max = b Else max = a
End Function
