# Microsoft Developer Studio Project File - Name="Lota" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Application" 0x0101

CFG=Lota - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "Lota.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "Lota.mak" CFG="Lota - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "Lota - Win32 Release" (based on "Win32 (x86) Application")
!MESSAGE "Lota - Win32 Debug" (based on "Win32 (x86) Application")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "Lota - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /c
# ADD CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /c
# ADD BASE MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /machine:I386
# ADD LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib winmm.lib ddraw.lib dxguid.lib dsound.lib dinput.lib /nologo /version:0.3 /subsystem:windows /machine:I386

!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /YX /FD /GZ /c
# ADD CPP /nologo /MTd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /FR /YX /FD /GZ /c
# ADD BASE MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept
# ADD LINK32 erikutils.lib winmm.lib ddraw.lib dxguid.lib dsound.lib dinput.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /debug /machine:I386 /nodefaultlib:"libcmt" /pdbtype:sept
# SUBTRACT LINK32 /incremental:no /nodefaultlib

!ENDIF 

# Begin Target

# Name "Lota - Win32 Release"
# Name "Lota - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=.\commands.cpp
# End Source File
# Begin Source File

SOURCE=.\d3dutil.cpp
# End Source File
# Begin Source File

SOURCE=.\dd.cpp
# End Source File
# Begin Source File

SOURCE=.\ddutil.cpp
# End Source File
# Begin Source File

SOURCE=.\dsutil3d.c
# End Source File
# Begin Source File

SOURCE=..\Utilities\ErikString.cpp
# End Source File
# Begin Source File

SOURCE=.\global.cpp
# End Source File
# Begin Source File

SOURCE=.\joystick.cpp
# End Source File
# Begin Source File

SOURCE=.\LotA.cpp
# End Source File
# Begin Source File

SOURCE=.\lotasound.cpp
# End Source File
# Begin Source File

SOURCE=.\map.cpp
# End Source File
# Begin Source File

SOURCE=.\menulist.cpp
# End Source File
# Begin Source File

SOURCE=.\player.cpp
# End Source File
# Begin Source File

SOURCE=.\render.cpp
# End Source File
# Begin Source File

SOURCE=.\res.rc
# End Source File
# Begin Source File

SOURCE=.\specialevent.cpp
# End Source File
# Begin Source File

SOURCE=.\texture.cpp
# End Source File
# Begin Source File

SOURCE=.\town.cpp
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\d3dutil.h
# End Source File
# Begin Source File

SOURCE=.\dd.h
# End Source File
# Begin Source File

SOURCE=.\ddutil.h
# End Source File
# Begin Source File

SOURCE=.\dsutil3d.h
# End Source File
# Begin Source File

SOURCE=.\joystick.h
# End Source File
# Begin Source File

SOURCE=.\LotA.h
# End Source File
# Begin Source File

SOURCE=.\lotasound.h
# End Source File
# Begin Source File

SOURCE=.\map.h
# End Source File
# Begin Source File

SOURCE=.\menulist.h
# End Source File
# Begin Source File

SOURCE=.\player.h
# End Source File
# Begin Source File

SOURCE=.\render.h
# End Source File
# Begin Source File

SOURCE=.\resource.h
# End Source File
# Begin Source File

SOURCE=.\town.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;rgs;gif;jpg;jpeg;jpe;"
# Begin Group "Wave Audio"

# PROP Default_Filter "wav"
# Begin Source File

SOURCE=".\sounds\board raft.wav"
# End Source File
# Begin Source File

SOURCE=".\sounds\building close.wav"
# End Source File
# Begin Source File

SOURCE=".\sounds\building open.wav"
# End Source File
# Begin Source File

SOURCE=.\sounds\bump.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\desert.wav
# End Source File
# Begin Source File

SOURCE=".\sounds\Enemy Die.wav"
# End Source File
# Begin Source File

SOURCE=".\sounds\enemy hit.wav"
# End Source File
# Begin Source File

SOURCE=".\sounds\enemy miss.wav"
# End Source File
# Begin Source File

SOURCE=.\sounds\good.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\invalid.WAV
# End Source File
# Begin Source File

SOURCE=.\sounds\Medium.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\mountains.wav
# End Source File
# Begin Source File

SOURCE=".\sounds\player hit.wav"
# End Source File
# Begin Source File

SOURCE=.\sounds\question.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\raft1.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\sale.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\shore1.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\shore2.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\swamp.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\townwalk.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\VeryBad.wav
# End Source File
# Begin Source File

SOURCE=.\sounds\VeryGood.wav
# End Source File
# Begin Source File

SOURCE=".\sounds\walk outside.wav"
# End Source File
# End Group
# Begin Source File

SOURCE=.\images\CastleTiles.bmp
# End Source File
# Begin Source File

SOURCE=.\character.bmp
# End Source File
# Begin Source File

SOURCE=.\images\character.bmp
# End Source File
# Begin Source File

SOURCE=".\dungeon ceiling texture.bmp"
# End Source File
# Begin Source File

SOURCE=".\dungeon floor ceiling.bmp"
# End Source File
# Begin Source File

SOURCE=".\dungeon floor texture.bmp"
# End Source File
# Begin Source File

SOURCE=".\dungeon wall texture.bmp"
# End Source File
# Begin Source File

SOURCE=".\textures\floor hole texture.bmp"
# End Source File
# Begin Source File

SOURCE=.\images\font.bmp
# End Source File
# Begin Source File

SOURCE=.\images\icon1.ico
# End Source File
# Begin Source File

SOURCE=.\Map1.map
# End Source File
# Begin Source File

SOURCE=.\pirates.dng
# End Source File
# Begin Source File

SOURCE=.\pirates.map
# End Source File
# Begin Source File

SOURCE=.\images\tiles.bmp
# End Source File
# Begin Source File

SOURCE=.\images\towntiles.bmp
# End Source File
# Begin Source File

SOURCE=.\images\wave1.bin
# End Source File
# End Group
# Begin Source File

SOURCE=".\Included Maps\castle.twn"
# End Source File
# Begin Source File

SOURCE=".\Included Maps\Map1.map"
# End Source File
# Begin Source File

SOURCE=".\Included Maps\pirates.dng"
# End Source File
# Begin Source File

SOURCE=".\Included Maps\pirates.map"
# End Source File
# Begin Source File

SOURCE=".\Included Maps\thornberry.twn"
# End Source File
# End Target
# End Project
