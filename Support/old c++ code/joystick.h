
#ifndef __LOTA_JOYSTICK__
#define __LOTA_JOYSTICK__

#include "lota.h"

extern DIDEVCAPS            g_diDevCaps;


BOOL CALLBACK EnumJoysticksCallback( const DIDEVICEINSTANCE* pdidInstance, VOID* pContext );
BOOL CALLBACK EnumAxesCallback( const DIDEVICEOBJECTINSTANCE* pdidoi, VOID* pContext );
HRESULT InitDirectInput( HWND hDlg );
HRESULT FreeDirectInput();


#endif