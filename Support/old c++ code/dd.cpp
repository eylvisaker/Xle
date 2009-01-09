/*--------------------------------------------------------------------------*/
//#include "dd.h"
//#include <d3d.h>
//#include <ddraw.h>
#include <stdio.h>
#include "lota.h"

#define D3DDevice IID_IDirect3DHALDevice
/*--------------------------------------------------------------------------*/
LPDIRECTDRAW7			g_pDD          = NULL;  // The DirectDraw object
LPDIRECTDRAWCLIPPER		g_pClipper     = NULL;  // Clipper for primary surface
LPDIRECTDRAWSURFACE7	g_pDDS         = NULL;  // Primary surface
LPDIRECTDRAWSURFACE7	g_pDDSBack     = NULL;  // Back surface
LPDIRECTDRAWSURFACE7	g_pDDSTemp	   = NULL;	// Scratch surface
LPDIRECTDRAWSURFACE7	g_pDDSZBuffer  = NULL;	// ZBuffer
LPDIRECT3D7				g_pD3D		   = NULL;	// Direct 3D interface
LPDIRECT3DDEVICE7		g_pd3dDevice   = NULL;	// Direct 3D device
HWND					g_hWnd         = NULL;  // To store the main windows handle
bool					g_bFullScreen  = false; // Full-screen mode?
int						backWidth	   = 0;
int						backHeight	   = 0;

int						g_iBpp         = 0;     // Remember the main surface bit depth

extern Global g;

typedef HRESULT (WINAPI* LPD3DENUMPIXELFORMATSCALLBACK)(LPDDPIXELFORMAT lpDDPixFmt, LPVOID lpContext);

/*--------------------------------------------------------------------------*/
bool DDFailedCheck(HRESULT hr, char *szMessage)
{
	if (FAILED(hr))
	{
		char buf[1024];
		sprintf( buf, "%s (%s)\n", szMessage, DDErrorString(hr) );
		OutputDebugString( buf );
		return true;
	}
	return false;
}
/*--------------------------------------------------------------------------*/
// Initialize DirectDraw stuff
bool DDInit( HWND hWnd )
{
	HRESULT hr;
	
	g_hWnd = hWnd;
	
	// TODO: Enumerate devices here, get latest interfaces etc.
	
	// Initialize DirectDraw
	hr = DirectDrawCreateEx( NULL, (void**)&g_pDD, IID_IDirectDraw7, NULL );
	
	if (DDFailedCheck(hr, "DirectDrawCreateEx failed.  You must have DirectX 7 installed." ))
		return false;

#ifndef Disable3D
	// Query DirectDraw for access to Direct3D
	g_pDD->QueryInterface( IID_IDirect3D7, (VOID**)&g_pD3D );
	if( FAILED( hr) )
		return false;
#endif


	return true;
}


/*--------------------------------------------------------------------------*/
// Create surfaces manually
bool DDCreateSurfaces( bool bFullScreen, int width, int height, int bpp)
{
	HRESULT hr; // Holds return values for DirectX function calls
	
	g_bFullScreen = bFullScreen;
	
	backWidth = width;
	backHeight = height;

	// If we want to be in full-screen mode
	if (g_bFullScreen)
	{
		g_iBpp = bpp;

		// Set the "cooperative level" so we can use full-screen mode
		hr = g_pDD->SetCooperativeLevel(g_hWnd, DDSCL_EXCLUSIVE|DDSCL_FULLSCREEN|DDSCL_NOWINDOWCHANGES);
		if (DDFailedCheck(hr, "SetCooperativeLevel"))
			return false;
		
		// Set 16 bit color full-screen mode
		hr = g_pDD->SetDisplayMode(width, height, bpp, 0, 0);
		if (DDFailedCheck(hr, "SetDisplayMode" ))
			return false;
	}
	else
	{
		// Set DDSCL_NORMAL to use windowed mode
		hr = g_pDD->SetCooperativeLevel(g_hWnd, DDSCL_NORMAL);
		if (DDFailedCheck(hr, "SetCooperativeLevel windowed" ))
			return false;
	}
	
	DDSURFACEDESC2 ddsd;					// A structure to describe the surfaces we want

	// Clear all members of the structure to 0
	memset(&ddsd, 0, sizeof(ddsd));

	// The first parameter of the structure must contain the size of the structure
	ddsd.dwSize = sizeof(ddsd);
	
	if (g_bFullScreen)
	{

		//-- Create the FULL SCREEN primary surface
		
		// The dwFlags paramater tell DirectDraw which DDSURFACEDESC
		// fields will contain valid values
		ddsd.dwFlags = DDSD_CAPS | DDSD_BACKBUFFERCOUNT;
		ddsd.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE | DDSCAPS_FLIP | DDSCAPS_COMPLEX;
#ifndef Disable3D
		ddsd.ddsCaps.dwCaps |= DDSCAPS_3DDEVICE;
#endif // Disable3D

		ddsd.dwBackBufferCount = 1;

		hr = g_pDD->CreateSurface(&ddsd, &g_pDDS, NULL);
		if (DDFailedCheck(hr, "Create primary surface"))
			return false;
		
		//-- Get the pointer to the back buffer
		DDSCAPS2 ddscaps;

		memset (&ddscaps, 0, sizeof(ddscaps));

		ddscaps.dwCaps = DDSCAPS_BACKBUFFER;

		hr = g_pDDS->GetAttachedSurface(&ddscaps, &g_pDDSBack);
	
		if (DDFailedCheck(hr, "Create back surface"))
			return false;


		//	END FULL SCREEN SURFACE
	}
	else
	{
		
		//-- Create the primary surface
		
		// The dwFlags paramater tell DirectDraw which DDSURFACEDESC
		// fields will contain valid values
		ddsd.dwFlags = DDSD_CAPS;
		ddsd.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE;
#ifndef Disable3D
		ddsd.ddsCaps.dwCaps |= DDSCAPS_3DDEVICE;
#endif

		hr = g_pDD->CreateSurface(&ddsd, &g_pDDS, NULL);
		if (DDFailedCheck(hr, "Create primary surface"))
			return false;
		
		//-- Create the back buffer
		
		ddsd.dwFlags = DDSD_WIDTH | DDSD_HEIGHT | DDSD_CAPS;

		// Make our off-screen surface the height and width we want
		backWidth = 640;
		backHeight = 400;

		ddsd.dwWidth = backWidth;
		ddsd.dwHeight = backHeight;

		// Create an offscreen surface
		ddsd.ddsCaps.dwCaps = DDSCAPS_OFFSCREENPLAIN;
#ifndef Disable3D
		ddsd.ddsCaps.dwCaps |= DDSCAPS_3DDEVICE;
#endif
		
		hr = g_pDD->CreateSurface(&ddsd, &g_pDDSBack, NULL);
		if (DDFailedCheck(hr, "Create back surface"))
			return false;
		
	}

	//-- Lock back buffer to retrieve surface information
	if (g_pDDSBack)
	{
		hr= g_pDDSBack->Lock( NULL, &ddsd, DDLOCK_WAIT, NULL );
		if (DDFailedCheck(hr, "Lock back buffer failed" ))
			return false;
		
		// Store bit depth of surface
		g_iBpp = ddsd.ddpfPixelFormat.dwRGBBitCount;
		
		// Unlock surface
		hr = g_pDDSBack->Unlock( NULL );
		if (DDFailedCheck(hr, "Unlock back buffer failed" ))
			return false;
	}

	// Create a temporary off screen scratch surface for double transparency fonts
	ddsd.dwFlags = DDSD_WIDTH | DDSD_HEIGHT | DDSD_CAPS;
	ddsd.dwWidth = backWidth;
	ddsd.dwHeight = backHeight;
	ddsd.ddsCaps.dwCaps = DDSCAPS_OFFSCREENPLAIN;
	
	hr = g_pDD->CreateSurface(&ddsd, &g_pDDSTemp, NULL);
	if (DDFailedCheck(hr, "Create temp surface"))
		return false;

    // Check the display mode, and 
    ddsd.dwSize = sizeof(DDSURFACEDESC2);
    g_pDD->GetDisplayMode( &ddsd );
    if( ddsd.ddpfPixelFormat.dwRGBBitCount <= 8 )
        return false;
	
#ifndef Disable3D
	if (g.ZBufferEnable)
	{
		// Enumerate the Z Buffer formats
		DDPIXELFORMAT ddpfZBuffer;
		memset(&ddpfZBuffer, 0, sizeof(DDPIXELFORMAT));
		ddpfZBuffer.dwSize = sizeof(DDPIXELFORMAT);
		ddpfZBuffer.dwFlags = DDPF_ZBUFFER;

		g_pD3D->EnumZBufferFormats( D3DDevice , EnumZBufferCallback, (VOID*)&ddpfZBuffer );

		// If we found a good zbuffer format, then the dwSize field will be
		// properly set during enumeration. Else, we have a problem and will exit.
		if( sizeof(DDPIXELFORMAT) != ddpfZBuffer.dwSize )
			return false;
 
		// set the appropriate flags (including PIXELFORMAT) and copy that data into the 
		// proper structure
		memset(&ddsd, 0, sizeof(DDSURFACEDESC2));
		ddsd.dwSize			= sizeof(ddsd);
		ddsd.dwFlags        = DDSD_CAPS | DDSD_WIDTH | DDSD_HEIGHT | DDSD_PIXELFORMAT;
		ddsd.ddsCaps.dwCaps = DDSCAPS_ZBUFFER;
		ddsd.dwWidth        = backWidth;
		ddsd.dwHeight       = backHeight;
		
		memcpy( &ddsd.ddpfPixelFormat, &ddpfZBuffer, sizeof(DDPIXELFORMAT) );
		ddsd.ddpfPixelFormat.dwSize = sizeof(DDPIXELFORMAT);
		//ddsd.ddpfPixelFormat.dwFlags |= DDPF_ZPIXELS;

		// choose the proper type of memory to use for the z buffer
		if( IsEqualIID( D3DDevice, IID_IDirect3DHALDevice ) )
			ddsd.ddsCaps.dwCaps |= DDSCAPS_VIDEOMEMORY;
		else
		    ddsd.ddsCaps.dwCaps |= DDSCAPS_SYSTEMMEMORY;
 
		hr = g_pDD->CreateSurface(&ddsd, &g_pDDSZBuffer, NULL);
		if (DDFailedCheck(hr, "Create Z Buffer surface"))
			return false;

		hr = g_pDDSBack->AddAttachedSurface (g_pDDSZBuffer);
		if (DDFailedCheck(hr, "Attach Z Buffer"))
			return false;

	}
#endif


	
    // The GUID here is hard coded. In a real-world application
    // this should be retrieved by enumerating devices.
#ifndef Disable3D
    hr = g_pD3D->CreateDevice( D3DDevice , g_pDDSBack, &g_pd3dDevice );

    if( FAILED( hr ) )
    {
        // If the hardware GUID doesn't work, try a software device.
        hr = g_pD3D->CreateDevice( IID_IDirect3DRGBDevice, g_pDDSBack, &g_pd3dDevice );
        if( FAILED( hr ) )
            return false;
    }
#endif


	//-- Create a clipper for the primary surface in windowed mode
	if (!g_bFullScreen)
	{
		
		// Create the clipper using the DirectDraw object
		hr = g_pDD->CreateClipper(0, &g_pClipper, NULL);
		if (DDFailedCheck(hr, "Create clipper"))
			return false;
		
		// Assign your window's HWND to the clipper
		hr = g_pClipper->SetHWnd(0, g_hWnd);
		if (DDFailedCheck(hr, "Assign hWnd to clipper"))
			return false;
		
		// Attach the clipper to the primary surface
		hr = g_pDDS->SetClipper(g_pClipper);
		if (DDFailedCheck(hr, "Set clipper"))
			return false;
	}
	
	return true;
}
/*--------------------------------------------------------------------------*/
// Destroy surfaces
void DDDestroySurfaces()
{
    if (g_pDD != NULL)
    {
        g_pDD->SetCooperativeLevel(g_hWnd, DDSCL_NORMAL);

		if (g_pDDSTemp != NULL)
		{
			g_pDDSTemp->Release();
			g_pDDSTemp = NULL;
		}
		if (g_pDDSZBuffer != NULL)
		{
			g_pDDSZBuffer->Release();
			g_pDDSZBuffer = NULL;
		}

        if (g_pDDSBack != NULL)
        {
            g_pDDSBack->Release();
            g_pDDSBack = NULL;
        }
        if (g_pDDS != NULL)
        {
            g_pDDS->Release();
            g_pDDS = NULL;
        }

    }

}
/*--------------------------------------------------------------------------*/
// Clean up DirectDraw stuff
void DDDone()
{
	if (g_pd3dDevice)
	{
		g_pd3dDevice->Release();
		g_pd3dDevice = NULL;
	}
	if (g_pDD != NULL)
	{
		g_pDD->Release();
		g_pDD = NULL;
	}
}
/*--------------------------------------------------------------------------*/
// PutPixel routine for a DirectDraw surface
void DDPutPixel( LPDIRECTDRAWSURFACE7 pDDS, int x, int y, int r, int g, int b )
{
	HRESULT hr;
	DDBLTFX ddbfx;
	RECT    rcDest;
	
	// Safety net
	if (pDDS == NULL)
		return;
	
	// Initialize the DDBLTFX structure with the pixel color
	ddbfx.dwSize = sizeof( ddbfx );
	ddbfx.dwFillColor = (DWORD)CreateRGB( r, g, b );
	
	// Prepare the destination rectangle as a 1x1 (1 pixel) rectangle
	SetRect( &rcDest, x, y, (x+1), (y+1) );
	
	// Blit 1x1 rectangle using solid color op
	hr = pDDS->Blt( &rcDest, NULL, NULL, DDBLT_WAIT | DDBLT_COLORFILL, &ddbfx );
	DDFailedCheck(hr, "Blt failure");
}
/*--------------------------------------------------------------------------*/

// Create color from RGB triple
unsigned int CreateRGB( int r, int g, int b )
{
	unsigned int rr, gg, bb, total;
	switch (g_iBpp)
	{
	case 15:
		rr = r >> 3;
		rr <<= 10;
		gg = g >> 3;
		gg <<= 5;
		bb = b >> 3;
		
		total = rr | gg | bb;
		return total;

		break;

	case 16:
		// Break down r,g,b into 5-5-5 format.
		// for some reason, 5-6-5 doesn't work?
		// neither does 5-5-5.  How come?

		// for some reason, 4-5-5 works?  14 bit color?????????
		/*
		rr = r >> 4;
		rr <<= 11;
		gg = g >> 3;
		gg <<= 5;
		bb = b >> 3;
		*/

		rr = r >> 3;
		rr <<= 11;
		gg = g >> 2;
		gg <<= 5;
		bb = b >> 3;

		total = rr | gg | bb;

		return total;
	case 24:
	case 32:
		return (r<<16) | (g<<8) | (b);
	}
	return 0;
}
/*--------------------------------------------------------------------------*/
// Checks if the memory associated with surfaces is lost and restores if necessary.
void DDCheckSurfaces()
{
	// Check the primary surface
	if (g_pDDS)
	{
		if (g_pDDS->IsLost() == DDERR_SURFACELOST)
			g_pDDS->Restore();
	}
	// Check the back buffer
	if (g_pDDSBack)
	{
		if (g_pDDSBack->IsLost() == DDERR_SURFACELOST)
			g_pDDSBack->Restore();
	}
	// Check the temp buffer
	if (g_pDDSTemp)
	{
		if (g_pDDSTemp->IsLost() == DDERR_SURFACELOST)
			g_pDDSTemp->Restore();
	}
	if (g_pDDSZBuffer)
	{
		if (g_pDDSZBuffer->IsLost() == DDERR_SURFACELOST)
			g_pDDSZBuffer->Restore();
	}


}
/*--------------------------------------------------------------------------*/
// Double buffering flip
void DDFlip()
{
	HRESULT hr;
	
	// if we're windowed do the blit, else just Flip
	if (!g_bFullScreen)
	{
		RECT    rcSrc;  // source blit rectangle
		RECT    rcDest; // destination blit rectangle
		POINT   p;
		
		// find out where on the primary surface our window lives
		p.x = 0; p.y = 0;
		::ClientToScreen(g_hWnd, &p);
		::GetClientRect(g_hWnd, &rcDest);
		OffsetRect(&rcDest, p.x, p.y);
		SetRect(&rcSrc, 0, 0, backWidth, backHeight);
		hr = g_pDDS->Blt(&rcDest, g_pDDSBack, &rcSrc, DDBLT_WAIT, NULL);

	}
	else
	{
		hr = g_pDDS->Flip(NULL, DDFLIP_WAIT);
	}
}
/*--------------------------------------------------------------------------*/
// Clear a surface area with black
void DDClear( LPDIRECTDRAWSURFACE7 pDDS, int x1, int y1, int x2, int y2 )
{
	HRESULT hr;
	DDBLTFX ddbfx;
	RECT    rcDest;

	// Safety net
	if (pDDS == NULL)
		return;
	
	// Initialize the DDBLTFX structure with the pixel color
	ddbfx.dwSize = sizeof( ddbfx );
	ddbfx.dwFillColor = (DWORD)CreateRGB( 0, 0, 0 );
	
	SetRect( &rcDest, x1, y1, x2, y2 );
	
	// Blit the rectangle using solid color op
	hr = pDDS->Blt( &rcDest, pDDS, NULL, DDBLT_WAIT | DDBLT_COLORFILL, &ddbfx );
	DDFailedCheck(hr, "Blt failure");
}
/*--------------------------------------------------------------------------*/
// PutBox routine for a DirectDraw surface
void DDPutBox( LPDIRECTDRAWSURFACE7 pDDS, int x, int y, int width, int height, DWORD coloring)
{
	HRESULT hr;
	DDBLTFX ddbfx;
	RECT    rcDest;
	DWORD   flags = 0;

	// Safety net
	if (pDDS == NULL)
		return;
	
	// Initialize the DDBLTFX structure with the pixel color
	ddbfx.dwSize = sizeof( ddbfx );
	ddbfx.dwFillColor = coloring;
	
	// Prepare the destination rectangle as our rectangle
	SetRect( &rcDest, x, y, (x + width), (y + height));
	
	// set up our DD flags
	flags |= DDBLT_WAIT | DDBLT_COLORFILL;

	// Blit the rectangle
	hr = pDDS->Blt ( &rcDest, NULL, NULL, flags, &ddbfx );
	DDFailedCheck(hr, "Blt failure");
}
/*--------------------------------------------------------------------------*/
/*--------------------------------------------------------------------------*/
char *DDErrorString(HRESULT hr)
{
	switch (hr)
	{
	case DDERR_ALREADYINITIALIZED:           return "DDERR_ALREADYINITIALIZED";
	case DDERR_CANNOTATTACHSURFACE:          return "DDERR_CANNOTATTACHSURFACE";
	case DDERR_CANNOTDETACHSURFACE:          return "DDERR_CANNOTDETACHSURFACE";
	case DDERR_CURRENTLYNOTAVAIL:            return "DDERR_CURRENTLYNOTAVAIL";
	case DDERR_EXCEPTION:                    return "DDERR_EXCEPTION";
	case DDERR_GENERIC:                      return "DDERR_GENERIC";
	case DDERR_HEIGHTALIGN:                  return "DDERR_HEIGHTALIGN";
	case DDERR_INCOMPATIBLEPRIMARY:          return "DDERR_INCOMPATIBLEPRIMARY";
	case DDERR_INVALIDCAPS:                  return "DDERR_INVALIDCAPS";
	case DDERR_INVALIDCLIPLIST:              return "DDERR_INVALIDCLIPLIST";
	case DDERR_INVALIDMODE:                  return "DDERR_INVALIDMODE";
	case DDERR_INVALIDOBJECT:                return "DDERR_INVALIDOBJECT";
	case DDERR_INVALIDPARAMS:                return "DDERR_INVALIDPARAMS";
	case DDERR_INVALIDPIXELFORMAT:           return "DDERR_INVALIDPIXELFORMAT";
	case DDERR_INVALIDRECT:                  return "DDERR_INVALIDRECT";
	case DDERR_LOCKEDSURFACES:               return "DDERR_LOCKEDSURFACES";
	case DDERR_NO3D:                         return "DDERR_NO3D";
	case DDERR_NOALPHAHW:                    return "DDERR_NOALPHAHW";
	case DDERR_NOCLIPLIST:                   return "DDERR_NOCLIPLIST";
	case DDERR_NOCOLORCONVHW:                return "DDERR_NOCOLORCONVHW";
	case DDERR_NOCOOPERATIVELEVELSET:        return "DDERR_NOCOOPERATIVELEVELSET";
	case DDERR_NOCOLORKEY:                   return "DDERR_NOCOLORKEY";
	case DDERR_NOCOLORKEYHW:                 return "DDERR_NOCOLORKEYHW";
	case DDERR_NODIRECTDRAWSUPPORT:          return "DDERR_NODIRECTDRAWSUPPORT";
	case DDERR_NOEXCLUSIVEMODE:              return "DDERR_NOEXCLUSIVEMODE";
	case DDERR_NOFLIPHW:                     return "DDERR_NOFLIPHW";
	case DDERR_NOGDI:                        return "DDERR_NOGDI";
	case DDERR_NOMIRRORHW:                   return "DDERR_NOMIRRORHW";
	case DDERR_NOTFOUND:                     return "DDERR_NOTFOUND";
	case DDERR_NOOVERLAYHW:                  return "DDERR_NOOVERLAYHW";
	case DDERR_NORASTEROPHW:                 return "DDERR_NORASTEROPHW";
	case DDERR_NOROTATIONHW:                 return "DDERR_NOROTATIONHW";
	case DDERR_NOSTRETCHHW:                  return "DDERR_NOSTRETCHHW";
	case DDERR_NOT4BITCOLOR:                 return "DDERR_NOT4BITCOLOR";
	case DDERR_NOT4BITCOLORINDEX:            return "DDERR_NOT4BITCOLORINDEX";
	case DDERR_NOT8BITCOLOR:                 return "DDERR_NOT8BITCOLOR";
	case DDERR_NOTEXTUREHW:                  return "DDERR_NOTEXTUREHW";
	case DDERR_NOVSYNCHW:                    return "DDERR_NOVSYNCHW";
	case DDERR_NOZBUFFERHW:                  return "DDERR_NOZBUFFERHW";
	case DDERR_NOZOVERLAYHW:                 return "DDERR_NOZOVERLAYHW";
	case DDERR_OUTOFCAPS:                    return "DDERR_OUTOFCAPS";
	case DDERR_OUTOFMEMORY:                  return "DDERR_OUTOFMEMORY";
	case DDERR_OUTOFVIDEOMEMORY:             return "DDERR_OUTOFVIDEOMEMORY";
	case DDERR_OVERLAYCANTCLIP:              return "DDERR_OVERLAYCANTCLIP";
	case DDERR_OVERLAYCOLORKEYONLYONEACTIVE: return "DDERR_OVERLAYCOLORKEYONLYONEACTIVE";
	case DDERR_PALETTEBUSY:                  return "DDERR_PALETTEBUSY";
	case DDERR_COLORKEYNOTSET:               return "DDERR_COLORKEYNOTSET";
	case DDERR_SURFACEALREADYATTACHED:       return "DDERR_SURFACEALREADYATTACHED";
	case DDERR_SURFACEALREADYDEPENDENT:      return "DDERR_SURFACEALREADYDEPENDENT";
	case DDERR_SURFACEBUSY:                  return "DDERR_SURFACEBUSY";
	case DDERR_CANTLOCKSURFACE:              return "DDERR_CANTLOCKSURFACE";
	case DDERR_SURFACEISOBSCURED:            return "DDERR_SURFACEISOBSCURED";
	case DDERR_SURFACELOST:                  return "DDERR_SURFACELOST";
	case DDERR_SURFACENOTATTACHED:           return "DDERR_SURFACENOTATTACHED";
	case DDERR_TOOBIGHEIGHT:                 return "DDERR_TOOBIGHEIGHT";
	case DDERR_TOOBIGSIZE:                   return "DDERR_TOOBIGSIZE";
	case DDERR_TOOBIGWIDTH:                  return "DDERR_TOOBIGWIDTH";
	case DDERR_UNSUPPORTED:                  return "DDERR_UNSUPPORTED";
	case DDERR_UNSUPPORTEDFORMAT:            return "DDERR_UNSUPPORTEDFORMAT";
	case DDERR_UNSUPPORTEDMASK:              return "DDERR_UNSUPPORTEDMASK";
	case DDERR_VERTICALBLANKINPROGRESS:      return "DDERR_VERTICALBLANKINPROGRESS";
	case DDERR_WASSTILLDRAWING:              return "DDERR_WASSTILLDRAWING";
	case DDERR_XALIGN:                       return "DDERR_XALIGN";
	case DDERR_INVALIDDIRECTDRAWGUID:        return "DDERR_INVALIDDIRECTDRAWGUID";
	case DDERR_DIRECTDRAWALREADYCREATED:     return "DDERR_DIRECTDRAWALREADYCREATED";
	case DDERR_NODIRECTDRAWHW:               return "DDERR_NODIRECTDRAWHW";
	case DDERR_PRIMARYSURFACEALREADYEXISTS:  return "DDERR_PRIMARYSURFACEALREADYEXISTS";
	case DDERR_NOEMULATION:                  return "DDERR_NOEMULATION";
	case DDERR_REGIONTOOSMALL:               return "DDERR_REGIONTOOSMALL";
	case DDERR_CLIPPERISUSINGHWND:           return "DDERR_CLIPPERISUSINGHWND";
	case DDERR_NOCLIPPERATTACHED:            return "DDERR_NOCLIPPERATTACHED";
	case DDERR_NOHWND:                       return "DDERR_NOHWND";
	case DDERR_HWNDSUBCLASSED:               return "DDERR_HWNDSUBCLASSED";
	case DDERR_HWNDALREADYSET:               return "DDERR_HWNDALREADYSET";
	case DDERR_NOPALETTEATTACHED:            return "DDERR_NOPALETTEATTACHED";
	case DDERR_NOPALETTEHW:                  return "DDERR_NOPALETTEHW";
	case DDERR_BLTFASTCANTCLIP:              return "DDERR_BLTFASTCANTCLIP";
	case DDERR_NOBLTHW:                      return "DDERR_NOBLTHW";
	case DDERR_NODDROPSHW:                   return "DDERR_NODDROPSHW";
	case DDERR_OVERLAYNOTVISIBLE:            return "DDERR_OVERLAYNOTVISIBLE";
	case DDERR_NOOVERLAYDEST:                return "DDERR_NOOVERLAYDEST";
	case DDERR_INVALIDPOSITION:              return "DDERR_INVALIDPOSITION";
	case DDERR_NOTAOVERLAYSURFACE:           return "DDERR_NOTAOVERLAYSURFACE";
	case DDERR_EXCLUSIVEMODEALREADYSET:      return "DDERR_EXCLUSIVEMODEALREADYSET";
	case DDERR_NOTFLIPPABLE:                 return "DDERR_NOTFLIPPABLE";
	case DDERR_CANTDUPLICATE:                return "DDERR_CANTDUPLICATE";
	case DDERR_NOTLOCKED:                    return "DDERR_NOTLOCKED";
	case DDERR_CANTCREATEDC:                 return "DDERR_CANTCREATEDC";
	case DDERR_NODC:                         return "DDERR_NODC";
	case DDERR_WRONGMODE:                    return "DDERR_WRONGMODE";
	case DDERR_IMPLICITLYCREATED:            return "DDERR_IMPLICITLYCREATED";
	case DDERR_NOTPALETTIZED:                return "DDERR_NOTPALETTIZED";
	case DDERR_UNSUPPORTEDMODE:              return "DDERR_UNSUPPORTEDMODE";
	case DDERR_NOMIPMAPHW:                   return "DDERR_NOMIPMAPHW";
	case DDERR_INVALIDSURFACETYPE:           return "DDERR_INVALIDSURFACETYPE";
	case DDERR_DCALREADYCREATED:             return "DDERR_DCALREADYCREATED";
	case DDERR_CANTPAGELOCK:                 return "DDERR_CANTPAGELOCK";
	case DDERR_CANTPAGEUNLOCK:               return "DDERR_CANTPAGEUNLOCK";
	case DDERR_NOTPAGELOCKED:                return "DDERR_NOTPAGELOCKED";
	case DDERR_NOTINITIALIZED:               return "DDERR_NOTINITIALIZED";
	}
	return "Unknown Error";
}

static HRESULT WINAPI EnumZBufferCallback( DDPIXELFORMAT* pddpf,
                                           VOID* pddpfDesired )
{
	// check to see if it's null
	if (pddpf == NULL || pddpfDesired == NULL)
		return D3DENUMRET_CANCEL;

    // For this tutorial, we are only interested in z-buffers, so ignore any
    // other formats (e.g. DDPF_STENCILBUFFER) that get enumerated. An app
    // could also check the depth of the z-buffer (16-bit, etc,) and make a
    // choice based on that, as well. For this tutorial, we'll take the first
    // one we get.
    if( pddpf->dwFlags == DDPF_ZBUFFER && pddpf->dwZBufferBitDepth == 16 )
    {
        memcpy( pddpfDesired, pddpf, sizeof(DDPIXELFORMAT) );
 
        // Return with D3DENUMRET_CANCEL to end the search.
        return D3DENUMRET_CANCEL;
    }
 
    // Return with D3DENUMRET_OK to continue the search.
    return D3DENUMRET_OK;
}
