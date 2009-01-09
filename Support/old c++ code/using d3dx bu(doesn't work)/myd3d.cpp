
#include "lota.h"

extern Global g;

void Set3DViewPort()
{
	if (g.d3dViewport == false)
	{

		DWORD dwRenderWidth  = 624 - g.vertLine - 16;
		DWORD dwRenderHeight = 17 * 16;
		D3DVIEWPORT7 vp = { g.vertLine + 16, 16, dwRenderWidth, dwRenderHeight, 0.0f, 1.0f };			

		if (FAILED( g_pd3dDevice->SetViewport( &vp )))
			return;


		// set ambient lighting and white material
		D3DMATERIAL7 mtrl;
		ZeroMemory( &mtrl, sizeof(mtrl) );
		mtrl.diffuse.r = mtrl.diffuse.g = mtrl.diffuse.b = 1.0f;
		mtrl.ambient.r = mtrl.ambient.g = mtrl.ambient.b = 1.0f;
		g_pd3dDevice->SetMaterial( &mtrl );
		g_pd3dDevice->SetRenderState( D3DRENDERSTATE_AMBIENT, 0xffffffff );

		// Set the projection matrix. Note that the view and world matrices are
		// set in the App_FrameMove() function, so they can be animated each
		// frame.
		D3DMATRIX matProj;
		ZeroMemory( &matProj, sizeof(D3DMATRIX) );
		matProj._11 =  1.0f;
		matProj._22 =  1.0f;
		matProj._33 =  1.0f;
		matProj._34 =  1.0f/150.0f;
		matProj._43 =  -150.0f;
		g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_PROJECTION, &matProj );

	}

}

void Store3DMap()
{
	
	// conditions?
	if (!g.d3dFloor)
	{
	    g.wallTexture = CreateTexture(g_pd3dDevice, MAKEINTRESOURCE(DUNGEON_WALL));

		g.d3dFloor = new D3DFloor;
		
		// Create vectors to define top and bottom corners of room
		D3DVECTOR vX0Y0Z0 (0,-10, 0), vXFYFZ0 (150,-10,150), vX0YFZ0 (0,-10,150), vXFY0Z0 (150,-10,0);
		D3DVECTOR vX0Y0Z1 (0,10,150), vXFYFZ1 (150, 10,150), vX0YFZ1 (0, 10,150), vXFY0Z1 (150,10,0);

		g.d3dFloor->floorVertex[0] = D3DVERTEX ( vX0Y0Z0, vX0Y0Z0, 0.0f, 0.0f);
		g.d3dFloor->floorVertex[1] = D3DVERTEX ( vXFY0Z0, vXFY0Z0, 1.0f, 0.0f);
		g.d3dFloor->floorVertex[2] = D3DVERTEX ( vXFYFZ0, vXFYFZ0, 0.0f, 1.0f);
		g.d3dFloor->floorVertex[3] = D3DVERTEX ( vX0YFZ0, vX0YFZ0, 1.0f, 1.0f);
		g.d3dFloor->floorVertex[4] = D3DVERTEX ( vX0Y0Z1, vX0Y0Z1, 0.0f, 0.0f);
		g.d3dFloor->floorVertex[5] = D3DVERTEX ( vXFY0Z1, vXFY0Z1, 1.0f, 0.0f);
		g.d3dFloor->floorVertex[6] = D3DVERTEX ( vXFYFZ1, vXFYFZ1, 0.0f, 1.0f);
		g.d3dFloor->floorVertex[7] = D3DVERTEX ( vX0YFZ1, vX0YFZ1, 1.0f, 1.0f);

		D3DMATRIX matIdent;
		D3DUtil_SetIdentityMatrix (matIdent);

		D3DMATRIX matView;
		D3DUtil_SetTranslateMatrix (matView, 0, 2, 0);

		g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_VIEW, &matView );

		D3DMATRIX matWorld;
		ZeroMemory( &matWorld, sizeof(D3DMATRIX) );
		g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_WORLD, &matView );

		//ID3DXSimpleShape	*floor;

		D3DXCreateBox (g_pd3dDevice, 150, 1, 150, D3DX_DEFAULT, &g.floor);



	}

}

void Render3DMap(  )
{

	if( FAILED( g_pd3dDevice->BeginScene() ) )
        return;

	//g_pd3dx
    g_pd3dDevice->Clear( 1UL, NULL, D3DCLEAR_TARGET, 0x00007700,
                         0L, 0L );

	// Render the scene here:
	g_pd3dDevice->SetTexture (0, g.wallTexture);

	g.floor->Draw();
/*
	g.floor->
	g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLESTRIP, D3DFVF_VERTEX,
								  g.d3dFloor->floorVertex, 4, 0);

	g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLESTRIP, D3DFVF_VERTEX,
								  g.d3dFloor->floorVertex + 4, 4, 0);
*/
								  

    // End the scene.
    g_pd3dDevice->EndScene();

}


// class functions
D3DWall::D3DWall()
{
	for (int i = 0; i < 8; i++)
		memset((void*)&vertex[i], 0, sizeof(D3DVERTEX));

	next = NULL;

}

D3DWall::~D3DWall()
{
	if (next)
		delete next;
}

D3DFloor::D3DFloor()
{

	first = NULL;

}

D3DFloor::~D3DFloor()
{

	if (first)
		delete first;

}