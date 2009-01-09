
#include "lota.h"

LPDIRECTINPUT7			g_pDI				= NULL;		// Direct Input Device
DIDEVCAPS				g_diDevCaps;

extern Global g;

//-----------------------------------------------------------------------------
// Name: EnumJoysticksCallback()
// Desc: Called once for each enumerated joystick. If we find one, create a
//       device interface on it so we can play with it.
//-----------------------------------------------------------------------------
BOOL CALLBACK EnumJoysticksCallback( const DIDEVICEINSTANCE* pdidInstance,
                                     VOID* pContext )
{
    HRESULT hr;

	if (g.pJoystick)
		return DIENUM_STOP;

    // Obtain an interface to the enumerated joystick.
    hr = g_pDI->CreateDeviceEx( pdidInstance->guidInstance, IID_IDirectInputDevice2,
		                        (VOID**)&g.pJoystick, NULL );

    // If it failed, then we can't use this joystick. (Maybe the user unplugged
    // it while we were in the middle of enumerating it.)
    if( FAILED(hr) ) 
	{
		g.pJoystick = NULL;
        return DIENUM_CONTINUE;
	}

    // Stop enumeration. Note: we're just taking the first joystick we get. You
    // could store all the enumerated joysticks and let the user pick.
    return DIENUM_STOP;
}

//-----------------------------------------------------------------------------
// Name: InitDirectInput()
// Desc: Initialize the DirectInput variables.
//-----------------------------------------------------------------------------
HRESULT InitDirectInput( HWND hDlg )
{
    HRESULT hr;

    // Register with the DirectInput subsystem and get a pointer
    // to a IDirectInput interface we can use.
    hr = DirectInputCreateEx( g.hInstance(), DIRECTINPUT_VERSION,IID_IDirectInput7, (LPVOID*)&g_pDI, NULL );
    if( FAILED(hr) ) 
        return hr;

    // Look for a simple joystick we can use for this sample program.
    hr = g_pDI->EnumDevices( DIDEVTYPE_JOYSTICK, EnumJoysticksCallback,
                             NULL, DIEDFL_ATTACHEDONLY );
    if( FAILED(hr) ) 
        return hr;

    // Make sure we got a joystick
    if( NULL == g.pJoystick )
    {
        return E_FAIL;
    }

    // Set the data format to "simple joystick" - a predefined data format 
    //
    // A data format specifies which controls on a device we are interested in,
    // and how they should be reported. This tells DInput that we will be
    // passing a DIJOYSTATE structure to IDirectInputDevice::GetDeviceState().
    hr = g.pJoystick->SetDataFormat( &c_dfDIJoystick );
    if( FAILED(hr) ) 
        return hr;

    // Set the cooperative level to let DInput know how this device should
    // interact with the system and with other DInput applications.
    hr = g.pJoystick->SetCooperativeLevel( hDlg, DISCL_EXCLUSIVE|DISCL_FOREGROUND );
    if( FAILED(hr) ) 
        return hr;

    // Determine how many axis the joystick has (so we don't error out setting
    // properties for unavailable axis)
    g_diDevCaps.dwSize = sizeof(DIDEVCAPS);
    hr = g.pJoystick->GetCapabilities(&g_diDevCaps);
    if ( FAILED(hr) ) 
        return hr;


    // Enumerate the axes of the joystick and set the range of each axis. Note:
    // we could just use the defaults, but we're just trying to show an example
    // of enumerating device objects (axes, buttons, etc.).

//    g_pJoystick->EnumObjects( EnumAxesCallback, (VOID*)g_pJoystick, DIDFT_AXIS );
    g.pJoystick->EnumObjects( EnumAxesCallback, (VOID*)hDlg, DIDFT_AXIS );

	g.pJoystick->Acquire();

    return S_OK;
}


//-----------------------------------------------------------------------------
// Name: EnumAxesCallback()
// Desc: Callback function for enumerating the axes on a joystick
//-----------------------------------------------------------------------------
BOOL CALLBACK EnumAxesCallback( const DIDEVICEOBJECTINSTANCE* pdidoi,
                                VOID* pContext )
{
    HWND hDlg = (HWND)pContext;

    DIPROPRANGE diprg; 
    diprg.diph.dwSize       = sizeof(DIPROPRANGE); 
    diprg.diph.dwHeaderSize = sizeof(DIPROPHEADER); 
    diprg.diph.dwHow        = DIPH_BYOFFSET; 
    diprg.diph.dwObj        = pdidoi->dwOfs; // Specify the enumerated axis
    diprg.lMin              = -1000; 
    diprg.lMax              = +1000; 
    
    // Set the range for the axis
    if( FAILED( g.pJoystick->SetProperty( DIPROP_RANGE, &diprg.diph ) ) )
        return DIENUM_STOP;


    return DIENUM_CONTINUE;
}


//-----------------------------------------------------------------------------
// Name: FreeDirectInput()
// Desc: Release the DirectInput variables.
//-----------------------------------------------------------------------------
HRESULT FreeDirectInput()
{
    // Unacquire and release any DirectInputDevice objects.
    if( NULL != g.pJoystick ) 
    {
        // Unacquire the device one last time just in case 
        // the app tried to exit while the device is still acquired.
        g.pJoystick->Unacquire();
        g.pJoystick->Release();
        g.pJoystick = NULL;
    }


    // Release any DirectInput objects.
    if( g_pDI ) 
    {
        g_pDI->Release();
        g_pDI = NULL;
    }

    return S_OK;
}



