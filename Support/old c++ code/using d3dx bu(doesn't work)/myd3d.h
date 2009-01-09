
#ifndef __MY_D3D_H__
#define __MY_D3D_H__

#include "lota.h"

class D3DWall {

public:
	D3DWall();
	~D3DWall();

	D3DVERTEX           vertex[8];
	D3DWall				*next;

};

class D3DFloor {

public:
	D3DFloor();
	~D3DFloor();

	D3DVERTEX           floorVertex[8];
	D3DWall				*first;

};

void Set3DViewPort();
void Store3DMap();
void Render3DMap();
LPDIRECTDRAWSURFACE7 CreateTexture( LPDIRECT3DDEVICE7 pd3dDevice, 
                                    CHAR* strName );
static LPDIRECTDRAWSURFACE7 CreateTextureFromBitmap( LPDIRECT3DDEVICE7 pd3dDevice,
                                                     HBITMAP hbm );



#endif