# Microsoft Developer Studio Generated NMAKE File, Based on Lota.dsp
!IF "$(CFG)" == ""
CFG=Lota - Win32 Debug
!MESSAGE No configuration specified. Defaulting to Lota - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "Lota - Win32 Release" && "$(CFG)" != "Lota - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
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
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 

CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "Lota - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\Lota.exe"


CLEAN :
	-@erase "$(INTDIR)\commands.obj"
	-@erase "$(INTDIR)\d3dutil.obj"
	-@erase "$(INTDIR)\dd.obj"
	-@erase "$(INTDIR)\ddutil.obj"
	-@erase "$(INTDIR)\global.obj"
	-@erase "$(INTDIR)\LotA.obj"
	-@erase "$(INTDIR)\map.obj"
	-@erase "$(INTDIR)\menulist.obj"
	-@erase "$(INTDIR)\myd3d.obj"
	-@erase "$(INTDIR)\player.obj"
	-@erase "$(INTDIR)\res.res"
	-@erase "$(INTDIR)\string.obj"
	-@erase "$(INTDIR)\texture.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(OUTDIR)\Lota.exe"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /ML /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /Fp"$(INTDIR)\Lota.pch" /YX /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 
MTL_PROJ=/nologo /D "NDEBUG" /mktyplib203 /win32 
RSC_PROJ=/l 0x409 /fo"$(INTDIR)\res.res" /d "NDEBUG" 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\Lota.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /incremental:no /pdb:"$(OUTDIR)\Lota.pdb" /machine:I386 /out:"$(OUTDIR)\Lota.exe" 
LINK32_OBJS= \
	"$(INTDIR)\commands.obj" \
	"$(INTDIR)\d3dutil.obj" \
	"$(INTDIR)\dd.obj" \
	"$(INTDIR)\ddutil.obj" \
	"$(INTDIR)\global.obj" \
	"$(INTDIR)\LotA.obj" \
	"$(INTDIR)\map.obj" \
	"$(INTDIR)\menulist.obj" \
	"$(INTDIR)\myd3d.obj" \
	"$(INTDIR)\player.obj" \
	"$(INTDIR)\string.obj" \
	"$(INTDIR)\texture.obj" \
	"$(INTDIR)\res.res"

"$(OUTDIR)\Lota.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\Lota.exe" "$(OUTDIR)\Lota.bsc"


CLEAN :
	-@erase "$(INTDIR)\commands.obj"
	-@erase "$(INTDIR)\commands.sbr"
	-@erase "$(INTDIR)\d3dutil.obj"
	-@erase "$(INTDIR)\d3dutil.sbr"
	-@erase "$(INTDIR)\dd.obj"
	-@erase "$(INTDIR)\dd.sbr"
	-@erase "$(INTDIR)\ddutil.obj"
	-@erase "$(INTDIR)\ddutil.sbr"
	-@erase "$(INTDIR)\global.obj"
	-@erase "$(INTDIR)\global.sbr"
	-@erase "$(INTDIR)\LotA.obj"
	-@erase "$(INTDIR)\LotA.sbr"
	-@erase "$(INTDIR)\map.obj"
	-@erase "$(INTDIR)\map.sbr"
	-@erase "$(INTDIR)\menulist.obj"
	-@erase "$(INTDIR)\menulist.sbr"
	-@erase "$(INTDIR)\myd3d.obj"
	-@erase "$(INTDIR)\myd3d.sbr"
	-@erase "$(INTDIR)\player.obj"
	-@erase "$(INTDIR)\player.sbr"
	-@erase "$(INTDIR)\res.res"
	-@erase "$(INTDIR)\string.obj"
	-@erase "$(INTDIR)\string.sbr"
	-@erase "$(INTDIR)\texture.obj"
	-@erase "$(INTDIR)\texture.sbr"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(OUTDIR)\Lota.bsc"
	-@erase "$(OUTDIR)\Lota.exe"
	-@erase "$(OUTDIR)\Lota.ilk"
	-@erase "$(OUTDIR)\Lota.pdb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /MLd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /FR"$(INTDIR)\\" /Fp"$(INTDIR)\Lota.pch" /YX /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 
MTL_PROJ=/nologo /D "_DEBUG" /mktyplib203 /win32 
RSC_PROJ=/l 0x409 /fo"$(INTDIR)\res.res" /d "_DEBUG" 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\Lota.bsc" 
BSC32_SBRS= \
	"$(INTDIR)\commands.sbr" \
	"$(INTDIR)\d3dutil.sbr" \
	"$(INTDIR)\dd.sbr" \
	"$(INTDIR)\ddutil.sbr" \
	"$(INTDIR)\global.sbr" \
	"$(INTDIR)\LotA.sbr" \
	"$(INTDIR)\map.sbr" \
	"$(INTDIR)\menulist.sbr" \
	"$(INTDIR)\myd3d.sbr" \
	"$(INTDIR)\player.sbr" \
	"$(INTDIR)\string.sbr" \
	"$(INTDIR)\texture.sbr"

"$(OUTDIR)\Lota.bsc" : "$(OUTDIR)" $(BSC32_SBRS)
    $(BSC32) @<<
  $(BSC32_FLAGS) $(BSC32_SBRS)
<<

LINK32=link.exe
LINK32_FLAGS=kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib winmm.lib ddraw.lib d3dx.lib /nologo /subsystem:windows /incremental:yes /pdb:"$(OUTDIR)\Lota.pdb" /debug /machine:I386 /out:"$(OUTDIR)\Lota.exe" /pdbtype:sept 
LINK32_OBJS= \
	"$(INTDIR)\commands.obj" \
	"$(INTDIR)\d3dutil.obj" \
	"$(INTDIR)\dd.obj" \
	"$(INTDIR)\ddutil.obj" \
	"$(INTDIR)\global.obj" \
	"$(INTDIR)\LotA.obj" \
	"$(INTDIR)\map.obj" \
	"$(INTDIR)\menulist.obj" \
	"$(INTDIR)\myd3d.obj" \
	"$(INTDIR)\player.obj" \
	"$(INTDIR)\string.obj" \
	"$(INTDIR)\texture.obj" \
	"$(INTDIR)\res.res"

"$(OUTDIR)\Lota.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.c{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<


!IF "$(NO_EXTERNAL_DEPS)" != "1"
!IF EXISTS("Lota.dep")
!INCLUDE "Lota.dep"
!ELSE 
!MESSAGE Warning: cannot find "Lota.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "Lota - Win32 Release" || "$(CFG)" == "Lota - Win32 Debug"
SOURCE=.\commands.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\commands.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\commands.obj"	"$(INTDIR)\commands.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\d3dutil.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\d3dutil.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\d3dutil.obj"	"$(INTDIR)\d3dutil.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\dd.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\dd.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\dd.obj"	"$(INTDIR)\dd.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\ddutil.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\ddutil.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\ddutil.obj"	"$(INTDIR)\ddutil.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\global.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\global.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\global.obj"	"$(INTDIR)\global.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\LotA.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\LotA.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\LotA.obj"	"$(INTDIR)\LotA.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\map.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\map.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\map.obj"	"$(INTDIR)\map.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\menulist.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\menulist.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\menulist.obj"	"$(INTDIR)\menulist.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\myd3d.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\myd3d.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\myd3d.obj"	"$(INTDIR)\myd3d.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\player.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\player.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\player.obj"	"$(INTDIR)\player.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\res.rc

"$(INTDIR)\res.res" : $(SOURCE) "$(INTDIR)"
	$(RSC) $(RSC_PROJ) $(SOURCE)


SOURCE=.\string.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\string.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\string.obj"	"$(INTDIR)\string.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 

SOURCE=.\texture.cpp

!IF  "$(CFG)" == "Lota - Win32 Release"


"$(INTDIR)\texture.obj" : $(SOURCE) "$(INTDIR)"


!ELSEIF  "$(CFG)" == "Lota - Win32 Debug"


"$(INTDIR)\texture.obj"	"$(INTDIR)\texture.sbr" : $(SOURCE) "$(INTDIR)"


!ENDIF 


!ENDIF 

