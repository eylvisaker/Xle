/*--------------------------------------------------------------------------*/
#ifndef _DD_H_
#define _DD_H_
/*--------------------------------------------------------------------------*/
#include <ddraw.h>
#include <d3d.h>

//#define Disable3D
/*--------------------------------------------------------------------------*/
// Variables
/*--------------------------------------------------------------------------*/
extern LPDIRECTDRAW7			g_pDD;        // The DirectDraw object
extern LPDIRECTDRAWCLIPPER		g_pClipper;   // Clipper for primary surface
extern LPDIRECTDRAWSURFACE7		g_pDDS;       // Primary surface
extern LPDIRECTDRAWSURFACE7		g_pDDSBack;   // Back surface
extern LPDIRECTDRAWSURFACE7		g_pDDSTemp;   // Scratch surface
extern LPDIRECT3D7				g_pD3D;			// Direct 3D interface
extern LPDIRECT3DDEVICE7		g_pd3dDevice;	// Direct 3D device
//extern ID3DXContext				*g_pd3dContext;	// Direct 3DX context handle
extern HWND						g_hWnd;			// To store the main window's handle
extern bool						g_bFullScreen;	// Full-screen mode?

extern int						g_iBpp;     // Remember the main surface bit depth
/*--------------------------------------------------------------------------*/
// Functions
/*--------------------------------------------------------------------------*/

//-- Housekeeping

extern bool DDInit (HWND hWnd);						// Initialize basic DirectDraw stuff
extern bool DDCreateSurfaces (bool bFullScreen,		// Create surfaces
							  int width, int height, int bpp = 16);
extern void DDDestroySurfaces ();					// Destroy surfaces
extern void DDDone ();								// Clean up DirectDraw stuff
extern void DDCheckSurfaces ();						// Checks if the memory associated with surfaces is lost and restores if necessary.

//-- Drawing
extern void DDPutPixel (LPDIRECTDRAWSURFACE7 pDDS, int x, int y,		// PutPixel routine for a DirectDraw surface
						int r, int g, int b );
extern void DDPutBox( LPDIRECTDRAWSURFACE7 pDDS, int x, int y,		// PutBox draws a box
					    int width, int height, DWORD coloring);
extern unsigned int CreateRGB (int r, int g, int b );				// Create color from RGB triple
extern void DDClear( LPDIRECTDRAWSURFACE7 pDDS, int x1, int y1,		// Clear a surface area with black
					int x2, int y2 );
extern void DDFlip ();												// Double buffering flip

//--- Error checking stuff
extern bool  DDFailedCheck (HRESULT hr, char *szMessage);
extern char *DDErrorString (HRESULT hr);

//--- Callback functions
static HRESULT WINAPI EnumZBufferCallback( DDPIXELFORMAT* pddpf, VOID* pddpfDesired );
/*--------------------------------------------------------------------------*/
#endif
/*--------------------------------------------------------------------------*/
