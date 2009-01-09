
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

	}

}

void Store3DMap()
{
	
	// conditions?
	if (!g.d3dFloor)
	{
		D3DWall *next;

		if (!g.wallTexture)
			g.wallTexture = CreateTexture(g_pd3dDevice, MAKEINTRESOURCE(DUNGEON_WALL));

		g.d3dFloor = new D3DFloor;
		
		// Create vectors to define top and bottom corners of room
		D3DVECTOR vX0Y0Z0 (0,0, 0), vXFYFZ0 (150,0,150), vXFY0Z0 (0,0,150), vX0YFZ0 (150,0,0);
		D3DVECTOR vX0Y0Z1 (0,10,150), vXFYFZ1 (150, 10,150), vXFY0Z1 (0, 10,150), vX0YFZ1 (150,10,0);
		D3DVECTOR topLeft (0, 1, 0);

		// floor
		g.d3dFloor->floorVertex[0] = D3DVERTEX ( vX0Y0Z0, D3DVECTOR (0,-1,0), 0.0f, 0.0f);
		g.d3dFloor->floorVertex[1] = D3DVERTEX ( vXFY0Z0, D3DVECTOR (0,-1,1), 1.0f, 0.0f);
		g.d3dFloor->floorVertex[2] = D3DVERTEX ( vXFYFZ0, D3DVECTOR (1,-1,1), 1.0f, 1.0f);
		g.d3dFloor->floorVertex[3] = D3DVERTEX ( vX0YFZ0, D3DVECTOR (1,-1,0), 0.0f, 1.0f);

		// ceiling
		g.d3dFloor->floorVertex[4] = D3DVERTEX ( vX0Y0Z1, D3DVECTOR (0,1,0), 0.0f, 1.0f);
		g.d3dFloor->floorVertex[5] = D3DVERTEX ( vXFY0Z1, D3DVECTOR (0,1,1), 1.0f, 0.0f);
		g.d3dFloor->floorVertex[6] = D3DVERTEX ( vXFYFZ1, D3DVECTOR (1,1,1), 1.0f, 1.0f);
		g.d3dFloor->floorVertex[7] = D3DVERTEX ( vX0YFZ1, D3DVECTOR (1,1,0), 0.0f, 1.0f);

		
		D3DMATRIX matWorld;
		ZeroMemory( &matWorld, sizeof(D3DMATRIX) );
		D3DUtil_SetIdentityMatrix (matWorld);
		//D3DUtil_SetScaleMatrix (matWorld, .1, .1, .1); 

		D3DMATRIX matProj;
		D3DUtil_SetProjectionMatrix (matProj, 10, 10, .01f, 15);

		g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_WORLD, &matWorld );
		g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_PROJECTION, &matProj );

		// create the north and west walls
		g.d3dFloor->first = new D3DWall;
		next = g.d3dFloor->first;

		next->vertex[0] = D3DVERTEX ( vX0Y0Z0, D3DVECTOR (0,-1,0), 0.0f, 0.0f);
		next->vertex[1] = D3DVERTEX ( vX0YFZ0, D3DVECTOR (1,-1,0), 0.0f, 1.0f);
		next->vertex[2] = D3DVERTEX ( vX0YFZ1, D3DVECTOR (0, 1,1), 1.0f, 1.0f);
		next->vertex[3] = D3DVERTEX ( vX0Y0Z1, D3DVECTOR (0, 1,0), 1.0f, 0.0f);

		next->vertex[4] = D3DVERTEX ( vX0Y0Z0, D3DVECTOR (0,-1,0), 0.0f, 0.0f);
		next->vertex[5] = D3DVERTEX ( vXFY0Z0, D3DVECTOR (0,-1,1), 0.0f, 1.0f);
		next->vertex[6] = D3DVERTEX ( vXFY0Z1, D3DVECTOR (0, 1,1), 1.0f, 1.0f);
		next->vertex[7] = D3DVERTEX ( vX0Y0Z1, D3DVECTOR (0, 1,0), 1.0f, 0.0f);

		// create the east and south walls
		next->next = new D3DWall;
		next = next->next;

		next->vertex[0] = D3DVERTEX ( vXFY0Z0, D3DVECTOR (0,-1,1), 0.0f, 0.0f);
		next->vertex[1] = D3DVERTEX ( vXFYFZ0, D3DVECTOR (1,-1,1), 0.0f, 1.0f);
		next->vertex[2] = D3DVERTEX ( vXFYFZ1, D3DVECTOR (1, 1,1), 1.0f, 1.0f);
		next->vertex[3] = D3DVERTEX ( vXFY0Z1, D3DVECTOR (0, 1,1), 1.0f, 0.0f);

		next->vertex[4] = D3DVERTEX ( vX0YFZ0, D3DVECTOR (1,-1,0), 0.0f, 0.0f);
		next->vertex[5] = D3DVERTEX ( vXFYFZ0, D3DVECTOR (1,-1,1), 0.0f, 1.0f);
		next->vertex[6] = D3DVERTEX ( vXFYFZ1, D3DVECTOR (1, 1,1), 1.0f, 1.0f);
		next->vertex[7] = D3DVERTEX ( vX0YFZ1, D3DVECTOR (1, 1,0), 1.0f, 0.0f);


	}

}

void Render3DMap(  )
{
	int dir = g.FaceDirection();

	D3DVECTOR topLeft (0, 1, 0);
	//D3DVECTOR eye (0,1,1);
	float	xpt = (float)g.player.x;
	float   ypt = (float)g.player.y;
	D3DVECTOR eye (ypt, .5f, xpt);
	static lastFrame = 0;

	if (g.commandMode == 3 && lastFrame == 0)
	{
		lastFrame = 1;
	}
	g.commandMode = 10;

	switch (dir)
	{
	case lotaEast:
		xpt = 15.0f;
		break;
	case lotaNorth:
		ypt = -1.0f;
		break;
	case lotaWest:
		xpt = -1.0f;
		break;
	case lotaSouth:
		ypt = 15.0f;
		break;
	}


	//D3DVECTOR at (-g.player.y / 15.0f * (dir == lotaNorth) + g.player.x / 15.0f * (dir == lotaSouth)
	//	, .3f, -g.player.y / 15.0f * (dir == lotaWest) + g.player.y / 15.0f * (dir == lotaEast));
	D3DVECTOR at (ypt, .3f, xpt);

	D3DVECTOR up (0, 1.0f, 0);
	D3DMATRIX matView;

	D3DUtil_SetViewMatrix (matView, eye , at, up);
	D3DMATRIX matRot;
	
	g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_VIEW, &matView );
	
	if( FAILED( g_pd3dDevice->BeginScene() ) )
        return;

	//g_pd3dx
    g_pd3dDevice->Clear( 1UL, NULL, D3DCLEAR_TARGET, 0x00000000,
                         0L, 0L );

	// Render the scene here:
	g_pd3dDevice->SetTexture (0, g.wallTexture);
	g_pd3dDevice->SetTextureStageState (0, D3DTSS_ADDRESS, D3DTADDRESS_MIRROR);


	g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLESTRIP, D3DFVF_VERTEX,
								  g.d3dFloor->floorVertex, 4, 0);

	g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLESTRIP, D3DFVF_VERTEX,
								  g.d3dFloor->floorVertex + 4, 4, 0);

	D3DWall *next;

	next = g.d3dFloor->first;

	while (next)
	{
		g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLESTRIP, D3DFVF_VERTEX,
									  next->vertex, 4, 0);
		g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLESTRIP, D3DFVF_VERTEX,
									  next->vertex + 4, 4, 0);

		next = next->next;
	}
	

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