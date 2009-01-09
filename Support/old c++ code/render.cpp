
#include "lota.h"

extern Global g;

const float h = 10;

void Set3DViewPort()
{
	if (g.d3dViewport == false)
	{

		DWORD dwRenderWidth  = 624 - g.vertLine - 16;
		DWORD dwRenderHeight = 17 * 16 - 2;
		D3DVIEWPORT7 vp = { g.vertLine + 16, 16, dwRenderWidth, dwRenderHeight, 0.0f, 1.0f };			

		if (FAILED( g_pd3dDevice->SetViewport( &vp )))
			return;


		// set ambient lighting and white material
		D3DMATERIAL7 mtrl;
		ZeroMemory( &mtrl, sizeof(mtrl) );
		mtrl.diffuse.r = mtrl.diffuse.g = mtrl.diffuse.b = 1.0f;
		mtrl.ambient.r = mtrl.ambient.g = mtrl.ambient.b = 1.0f;
		g_pd3dDevice->SetMaterial( &mtrl );
		
		g_pd3dDevice->SetRenderState( D3DRENDERSTATE_AMBIENT, 0xff888888 );
		g_pd3dDevice->SetRenderState( D3DRENDERSTATE_LIGHTING, TRUE);

		if (g.ZBufferEnable)
		{
			g_pd3dDevice->SetRenderState( D3DRENDERSTATE_ZENABLE, D3DZB_TRUE );
			g_pd3dDevice->SetRenderState( D3DRENDERSTATE_ZWRITEENABLE, D3DZB_TRUE );
		}

		g.d3dViewport = true;
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

		if (!g.floorTexture)
			g.floorTexture = CreateTexture(g_pd3dDevice, MAKEINTRESOURCE(DUNGEON_FLOOR));

		if (!g.ceilingTexture)
			g.ceilingTexture = CreateTexture(g_pd3dDevice, MAKEINTRESOURCE(DUNGEON_WALL));  // DUNGEON_CEILING

		if (!g.floorHoleTexture)
			g.floorHoleTexture = CreateTexture(g_pd3dDevice, MAKEINTRESOURCE(DUNGEON_FLOORHOLE));

		g.d3dFloor = new D3DFloor;
		
		const float w = g.map.MapWidth() * 10.0f;
		const float l = g.map.MapHeight() * 10.0f;

		// Create vectors to define top and bottom corners of room
		D3DVECTOR vX0Y0Z0 (0,0,0), vXFYFZ0 (l,0,w), vXFY0Z0 (0,0,w), vX0YFZ0 (w,0,0);
		D3DVECTOR vX0Y0Z1 (0,h,0), vXFYFZ1 (l,h,w), vXFY0Z1 (0,h,w), vX0YFZ1 (w,h,0);

		float t0 = 0.0f;
		float t1 = 4.0f;

		// floor
		g.d3dFloor->floorVertex[0] = D3DVERTEX ( vX0Y0Z0, vX0Y0Z0, 150.0f, 150.0f);
		g.d3dFloor->floorVertex[1] = D3DVERTEX ( vXFY0Z0, vXFY0Z0, 0.0f, 150.0f);
		g.d3dFloor->floorVertex[2] = D3DVERTEX ( vXFYFZ0, vXFYFZ0, 0.0f, 0.0f);
		g.d3dFloor->floorVertex[3] = D3DVERTEX ( vX0YFZ0, vX0YFZ0, 150.0f, 0.0f);

		// ceiling
		g.d3dFloor->floorVertex[4] = D3DVERTEX ( vX0YFZ1, vX0YFZ1, 150.0f, 150.0f);
		g.d3dFloor->floorVertex[5] = D3DVERTEX ( vXFYFZ1, vXFYFZ1, 0.0f, 150.0f);
		g.d3dFloor->floorVertex[6] = D3DVERTEX ( vXFY0Z1, vXFY0Z1, 0.0f, 0.0f);
		g.d3dFloor->floorVertex[7] = D3DVERTEX ( vX0Y0Z1, vX0Y0Z1, 150.0f, 0.0f);

		// create the north and west walls
		g.d3dFloor->first = new D3DWall;
		next = g.d3dFloor->first;
		next->texture = g.wallTexture;

		next->vertex[0] = D3DVERTEX ( vX0Y0Z0, vX0Y0Z0, 0.0f, 0.0f);
		next->vertex[1] = D3DVERTEX ( vX0YFZ0, vX0YFZ0, 0.0f, 7.0f);
		next->vertex[2] = D3DVERTEX ( vX0YFZ1, vX0YFZ1, 7.0f, 7.0f);
		next->vertex[3] = D3DVERTEX ( vX0Y0Z1, vX0Y0Z1, 7.0f, 0.0f);

		next->vertex[4] = D3DVERTEX ( vX0Y0Z0, vX0Y0Z0, 0.0f, 0.0f);
		next->vertex[5] = D3DVERTEX ( vXFY0Z0, vXFY0Z0, 0.0f, 7.0f);
		next->vertex[6] = D3DVERTEX ( vXFY0Z1, vXFY0Z1, 7.0f, 7.0f);
		next->vertex[7] = D3DVERTEX ( vX0Y0Z1, vX0Y0Z1, 7.0f, 0.0f);

		// create the east and south walls
		next->next = new D3DWall;
		next = next->next;
		next->texture = g.wallTexture;

		next->vertex[0] = D3DVERTEX ( vXFY0Z0, vXFY0Z0, 0.0f, 1.0f);
		next->vertex[1] = D3DVERTEX ( vXFYFZ0, vXFYFZ0, 0.0f, 1.0f);
		next->vertex[2] = D3DVERTEX ( vXFYFZ1, vXFYFZ1, 1.0f, 0.0f);
		next->vertex[3] = D3DVERTEX ( vXFY0Z1, vXFY0Z1, 1.0f, 0.0f);

		next->vertex[4] = D3DVERTEX ( vX0YFZ0, vX0YFZ0, 0.0f, 1.0f);
		next->vertex[5] = D3DVERTEX ( vXFYFZ0, vXFYFZ0, 0.0f, 1.0f);
		next->vertex[6] = D3DVERTEX ( vXFYFZ1, vXFYFZ1, 1.0f, 1.0f);
		next->vertex[7] = D3DVERTEX ( vX0YFZ1, vX0YFZ1, 1.0f, 0.0f);

		int face = g.player.FaceDirection();
		float j, i;

		for (j = -10; j < (g.map.MapHeight() + 1) * 10.0f; j += 10)
		{
			for (i = -10; i < (g.map.MapWidth() + 1) * 10.0f; i += 10)
			{			

				D3DVECTOR vLTF(j, h, i+10), vRTF(j+10, h, i+10), vLBF(j, 0, i+10), vRBF(j+10, 0, i+10);
				D3DVECTOR vLTB(j, h, i   ), vRTB(j+10, h, i   ), vLBB(j, 0, i   ), vRBB(j+10, 0, i   );
				
				switch (g.map.M(int(j / 10), int(i / 10)))
				{
				case 0xff:			// wall
					
					next->next = new D3DWall;
					next = next->next;

					// north face (seen when looking south) (works)
					next->vertex[0] = D3DVERTEX ( vLTF, vLTF, t0, t0);
					next->vertex[1] = D3DVERTEX ( vLTB, vLTB, t1, t0);
					next->vertex[2] = D3DVERTEX ( vLBB, vLBB, t1, t1);
					next->vertex[3] = D3DVERTEX ( vLBF, vLBF, t0, t1);
					next->numVert = 4;
					next->texture = g.wallTexture;
					
					next->next = new D3DWall;
					next = next->next;

					// south face 
					next->vertex[0] = D3DVERTEX ( vRTB, vRTB, t0, t0);
					next->vertex[1] = D3DVERTEX ( vRTF, vRTF, t1, t0);
					next->vertex[2] = D3DVERTEX ( vRBF, vRBF, t1, t1);
					next->vertex[3] = D3DVERTEX ( vRBB, vRBB, t0, t1);
					next->numVert = 4;
					next->texture = g.wallTexture;

					next->next = new D3DWall;
					next = next->next;

					// east face
					next->vertex[0] = D3DVERTEX ( vRTF, vRTF, t0, t0);
					next->vertex[1] = D3DVERTEX ( vLTF, vLTF, t1, t0);
					next->vertex[2] = D3DVERTEX ( vLBF, vLBF, t1, t1);
					next->vertex[3] = D3DVERTEX ( vRBF, vRBF, t0, t1);
					next->numVert = 4;
					next->texture = g.wallTexture;

					next->next = new D3DWall;
					next = next->next;

					// east face
					next->vertex[0] = D3DVERTEX ( vLTB, vLTB, t0, t0);
					next->vertex[1] = D3DVERTEX ( vRTB, vRTB, t1, t0);
					next->vertex[2] = D3DVERTEX ( vRBB, vRBB, t1, t1);
					next->vertex[3] = D3DVERTEX ( vLBB, vLBB, t0, t1);
					next->numVert = 4;
					next->texture = g.wallTexture;

					break;

				case 0x0D:				// ceiling hole
					next->next = new D3DWall;
					next = next->next;

					next->vertex[0] = D3DVERTEX ( D3DVECTOR (j+3, h-.2f, i+3), D3DVECTOR (j+3, h-.2f, i+3), 0, 0);
					next->vertex[1] = D3DVERTEX ( D3DVECTOR (j+7, h-.2f, i+3), D3DVECTOR (j+7, h-.2f, i+3), 1.0f, 0);
					next->vertex[2] = D3DVERTEX ( D3DVECTOR (j+7, h-.2f, i+7), D3DVECTOR (j+7, h-.2f, i+7), 1.0f, 1.0f);
					next->vertex[3] = D3DVERTEX ( D3DVECTOR (j+3, h-.2f, i+7), D3DVECTOR (j+3, h-.2f, i+7), 0, 1.0f);
					next->numVert = 4;
					next->texture = g.floorHoleTexture;

					break;

				case 0x0A:				// floor hole
					next->next = new D3DWall;
					next = next->next;

					next->vertex[0] = D3DVERTEX ( D3DVECTOR (j+3, .2f, i+7), D3DVECTOR (j+3, h, i+7), 0, 0);
					next->vertex[1] = D3DVERTEX ( D3DVECTOR (j+7, .2f, i+7), D3DVECTOR (j+7, h, i+7), 1, 0);
					next->vertex[2] = D3DVERTEX ( D3DVECTOR (j+7, .2f, i+3), D3DVECTOR (j+7, h, i+3), 1, 1);
					next->vertex[3] = D3DVERTEX ( D3DVECTOR (j+3, .2f, i+3), D3DVECTOR (j+3, h, i+3), 0, 1);
					next->numVert = 4;
					next->texture = g.floorHoleTexture;

					break;
				case 00:				// empty space
					next->next = new D3DWall;
					next = next->next;

					next->vertex[0] = D3DVERTEX ( D3DVECTOR (j+4, h, i+10), D3DVECTOR (j+4, h, i+10), t0, t1);
					next->vertex[1] = D3DVERTEX ( D3DVECTOR (j+2, h-1, i+10), D3DVECTOR (j+2, h-1, i+10), t0, t1/3);
					next->vertex[2] = D3DVERTEX ( D3DVECTOR (j+1, h-2, i+10), D3DVECTOR (j+1, h-2, i+10), t1/3, t0);
					next->vertex[3] = D3DVERTEX ( D3DVECTOR (j, h-4, i+10), D3DVECTOR (j, h-2, i+10), t0, t0);
					next->vertex[4] = D3DVERTEX ( vLTF, vLTF, t1, t0);
					next->texture = g.wallTexture;

					next->numVert = 5;

					break;
				}

			}

		}

		
		D3DMATRIX matWorld;

		D3DUtil_SetIdentityMatrix (matWorld);
		D3DUtil_SetScaleMatrix (matWorld, 1/10.0f, 1/10.0f, 1/10.0f); 

		g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_WORLD, &matWorld );

	}

}

D3DMATRIX *ProjectionMatrix(D3DMATRIX *ret, const float near_plane, // Distance to near clipping 
                                         // plane
                 const float far_plane,  // Distance to far clipping 
                                         // plane
                 const float fov_horiz,  // Horizontal field of view 
                                         // angle, in radians
                 const float fov_vert)   // Vertical field of view 
                                         // angle, in radians
{
    float    h, w, Q;
 
    w = (float)1/tanf(fov_horiz*0.5f);  // 1/tan(x) == cot(x)
    h = (float)1/tanf(fov_vert*0.5f);   // 1/tan(x) == cot(x)
    Q = far_plane/(far_plane - near_plane);
 
    ZeroMemory(ret, sizeof(D3DMATRIX));

    ret->_11 = w;
    ret->_22 = h;
    ret->_33 = Q;
    ret->_43 = -Q*near_plane;
    ret->_34 = 1;
    return ret;
}   // End of ProjectionMatrix


void Render3DMap(  )
{
	int dir = g.player.FaceDirection();

	static int walkForBack;
	float	xpt = (float)g.player.X() + 0.5f;
	float	ypt = (float)g.player.Y() + 0.5f;
	float	eyexpt = xpt;
	float   eyeypt = ypt;
	float	atxpt = xpt;
	float	atypt = ypt;
	const float degToRad = pi / 180;
	static float lastFrameDeg = 0;
	static float lastClock = 0;
	static float lastStep = 0;
	static float timeArray[90];
	static int time = 0;
	//const float eyeset = 0.0f;

	float	lastFrame;
	int thisClock = clock();
	float lastSign;

	if (g.commandMode == cmd2 && lastStep == 0)
	{
		walkForBack = g.menuKey;
		lastStep = -1;
	}

	/*if (g.commandMode == 2 && lastStep >= 0)
	{
		walkForBack = g.menuKey;
	}
*/
	if (g.commandMode == cmd3 && lastFrameDeg == 0)
	{
		lastFrameDeg = 89;
		g.commandMode = cmdPrompt;
	}

	if (lastClock + 10 < thisClock && g.commandMode == 2)
	{
		lastStep += (thisClock - lastClock) / 15 / 10;//20
		timeArray[time++] = lastStep;
	}

	if (lastClock + 10 < thisClock && lastFrameDeg > 0 && lastStep == 0)
	{
		lastFrameDeg -= (thisClock - lastClock) / 5;//10
	}

	lastClock = (float)thisClock;

	if (lastFrameDeg > 0)
	{
		switch (g.menuKey )
		{
		case lotaEast:
			lastSign = 1;
			break;
		case lotaNorth:
			lastSign = -1;
			break;
		case lotaWest:
			lastSign = -1;
			break;
		case lotaSouth:
			lastSign = 1;
			break;
		}
	}
	else
		lastSign = 1;

	// done with animation
	if (lastFrameDeg <= 0 && g.commandMode == 4)
	{
		g.commandMode = cmdEnterCommand;
		lastFrameDeg = 0;
	}

	if (lastStep >= 0 && g.commandMode == 2)
	{
		lastStep = 0;
		g.commandMode = cmdEnterCommand;
		time = 0;
	}

	lastFrame = lastFrameDeg * degToRad;

	switch (dir)
	{
	case lotaEast:
		eyexpt += lastStep * walkForBack;
		atxpt += 20.0f * (float)cos(lastFrame);
		atypt += 20.0f * lastSign * (float)sin(lastFrame);
		break;
	case lotaNorth:
		eyeypt -= lastStep * walkForBack;
		atypt -= 20.0f * (float)cos(lastFrame);
		atxpt += 20.0f * lastSign * (float)sin(lastFrame);
		break;
	case lotaWest:
		eyexpt -= lastStep * walkForBack;
		atxpt -= 20.0f * (float)cos(lastFrame);
		atypt += 20.0f * lastSign * (float)sin(lastFrame);
		break;
	case lotaSouth:
		eyeypt += lastStep * walkForBack;
		atypt += 20.0f * (float)cos(lastFrame);
		atxpt += 20.0f * lastSign * (float)sin(lastFrame);
		break;
	}

	D3DVECTOR eye (eyeypt, 0.5f, eyexpt);
	D3DVECTOR at (atypt, 0.5f, atxpt);
	D3DVECTOR diff = at - eye;
	
	float len = powf(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z, 0.5f);

	eye.x -= diff.x / len;
	eye.y -= diff.y / len;
	eye.z -= diff.z / len;
	
	D3DVECTOR up (0, 1.0f, 0);
	D3DMATRIX matView;

	D3DUtil_SetViewMatrix (matView, eye , at, up);
	
	
	D3DMATRIX matProj;
	// D3DUtil_SetProjectionMatrix( D3DMATRIX& mat, FLOAT fFOV, FLOAT fAspect,
    //                                 FLOAT fNearPlane, FLOAT fFarPlane )

	float aspect = float(17 * 16 - 2) / float(624 - g.vertLine - 16);

	D3DUtil_SetProjectionMatrix (matProj, pi/4, aspect, 0.1f, 10.0f);
	
	D3DMATRIX matTempProj;
	ProjectionMatrix(&matTempProj, 0.5f, 15.0f, pi/4, pi/4);

	g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_VIEW, &matView );
	g_pd3dDevice->SetTransform( D3DTRANSFORMSTATE_PROJECTION, &matTempProj );

	/************************************************************************
	 *	Begin rendering the scene
	 ************************************************************************/
	float fogStart = 0.0f;
	float fogEnd = 6.0f;

	g_pd3dDevice->SetRenderState( D3DRENDERSTATE_FOGENABLE, TRUE );
	g_pd3dDevice->SetRenderState( D3DRENDERSTATE_FOGTABLEMODE , D3DFOG_LINEAR );
	g_pd3dDevice->SetRenderState( D3DRENDERSTATE_FOGSTART , *((DWORD*) &fogStart ) );
	g_pd3dDevice->SetRenderState( D3DRENDERSTATE_FOGEND , *((DWORD*) &fogEnd ) );
	//g_pd3dDevice->SetRenderState( D3DRENDERSTATE_FOGTABLEDENSITY , 6 );
	g_pd3dDevice->SetRenderState( D3DRENDERSTATE_FOGCOLOR , D3DRGB (0.2f, 0.2f, 0.2f));
	g_pd3dDevice->SetRenderState( D3DRENDERSTATE_ANTIALIAS , D3DANTIALIAS_SORTINDEPENDENT );

	if( FAILED( g_pd3dDevice->BeginScene() ) )
        return;

    /*g_pd3dDevice->Clear( 1UL, NULL, D3DCLEAR_TARGET | D3DCLEAR_ZBUFFER, 0x00666666,
                         0L, 0L );
						 */
    g_pd3dDevice->Clear( 0, NULL, D3DCLEAR_TARGET | D3DCLEAR_ZBUFFER, 0x000000ff,
                         1.0f, 0 );

	// Render the scene here:


	// test lighting
    D3DLIGHT7 d3dLight;
    HRESULT   hr;
	float	  lightIntensity = rnd(35, 40) / 100.0f;
	
	// make the light Intensity maximum for now
	lightIntensity = 0.4f;

    // Initialize the structure.
    ZeroMemory(&d3dLight, sizeof(D3DLIGHT7));
 
    // Set up for a white point light.
    d3dLight.dltType = D3DLIGHT_POINT;
    d3dLight.dcvDiffuse.r = lightIntensity / 2;
    d3dLight.dcvDiffuse.g = lightIntensity / 2;
    d3dLight.dcvDiffuse.b = lightIntensity / 2;
    d3dLight.dcvAmbient.r = lightIntensity;
    d3dLight.dcvAmbient.g = lightIntensity;
    d3dLight.dcvAmbient.b = lightIntensity;
    d3dLight.dcvSpecular.r = lightIntensity;
    d3dLight.dcvSpecular.g = lightIntensity;
    d3dLight.dcvSpecular.b = lightIntensity;
 
    d3dLight.dvPosition.x = eyeypt;
    d3dLight.dvPosition.y = 0.5f;
    d3dLight.dvPosition.z = eyexpt;

	//d3dLight.dvDirection.x = atypt;
	//d3dLight.dvDirection.y = 0.5f;
	//d3dLight.dvDirection.z = atxpt;
 
    // Don't attenuate.
    d3dLight.dvAttenuation0 = 0.0f; 
    d3dLight.dvAttenuation1 = 0.2f; 
    d3dLight.dvAttenuation2 = 0.1f; 

    d3dLight.dvRange = 4.0f;
	
    // Set the property info for the first light.
    hr = g_pd3dDevice->SetLight(0, &d3dLight);
    if (FAILED(hr))
    {
// Code to handle the error goes here.
    }

	g_pd3dDevice->LightEnable (0, true);

	g_pd3dDevice->SetTexture (0, g.floorTexture);
	g_pd3dDevice->SetTextureStageState (0, D3DTSS_ADDRESS, D3DTADDRESS_WRAP);
	g_pd3dDevice->SetTextureStageState (0, D3DTSS_MINFILTER, D3DTFN_ANISOTROPIC );
	g_pd3dDevice->SetTextureStageState (0, D3DTSS_MAGFILTER, D3DTFG_ANISOTROPIC );
	g_pd3dDevice->SetTextureStageState (0, D3DTSS_MIPFILTER, D3DTFP_NONE      );

	g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLEFAN, D3DFVF_VERTEX, g.d3dFloor->floorVertex, 4, 0);

	g_pd3dDevice->SetTexture (0, g.ceilingTexture);
	g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLEFAN, D3DFVF_VERTEX, g.d3dFloor->floorVertex + 4, 4, 0);

	g_pd3dDevice->SetTexture (0, g.wallTexture);


	D3DWall *next;
	
	next = g.d3dFloor->first;

	while (next)
	{
		g_pd3dDevice->SetTexture (0, next->texture);
		//g_pd3dDevice->SetTextureStageState(0, D3DTSS_MAGFILTER, D3DTFG_LINEAR);

		if (next->numVert > 0)
			g_pd3dDevice->DrawPrimitive ( D3DPT_TRIANGLEFAN, D3DFVF_VERTEX, next->vertex, next->numVert, 0);
		next = next->next;
	}

    // End the scene.
    g_pd3dDevice->EndScene();

}


// class functions
D3DWall::D3DWall()
{
	//for (int i = 0; i < 8; i++)
	//	memset((void*)(vertex + i), 0, sizeof(D3DVERTEX));

	next = NULL;
	numVert = 0;

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