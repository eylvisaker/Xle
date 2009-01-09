/*
 * Input file	: ..\menu.exe
 * File type	: EXE
 */

#include "dcc.h"


void  ()
/* Takes no parameters.
 * Untranslatable routine.  Assembler provided.
 */
{
        MOV            ax, es
        ADD            ax, 10h
        PUSH           cs
        POP            ds
        MOV            [4], ax
        ADD            ax, [0Ch]
        MOV            es, ax
        MOV            cx, [6]
        MOV            di, cx
        DEC            di
        MOV            si, di
        STD
        REP MOVSB
        PUSH           ax
        MOV            ax, 32h
        PUSH           ax
        RETF
}

