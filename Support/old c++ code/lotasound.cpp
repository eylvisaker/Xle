
#include "lota.h"

LPDIRECTSOUND					g_pDirectSound = NULL;		// Direct Sound Device

bool							worked = false;				// Keeps us from playing sounds if init failed

extern Global g;

LotaSoundBuffer *first = NULL;
int				totalSounds = 0;

LotaSoundBuffer::~LotaSoundBuffer()
{
	if (next)
		delete next;
}

LotaSoundBuffer::LotaSoundBuffer()
{
	next = NULL;
	buffer = NULL;
	soundID = 0;
	fading = 0;
	firstFade = 0;
	
}

void InitDSound()
{
	HRESULT hr = DirectSoundCreate(NULL, &g_pDirectSound, NULL);
	
	String failed = "DirectSound failed!";
	
	if (FAILED(hr))
	{
		MessageBox (g.hwnd(), "Couldn't initialize DirectSound.  Continuing without sound.",
			failed, 0);
		worked = false;
		return;
	}
	else
	{
		worked = true;
	}
	
	hr = g_pDirectSound->SetCooperativeLevel(g.hwnd(), DSSCL_PRIORITY);
	
	if (FAILED(hr))
	{
		MessageBox (g.hwnd(), "Couldn't set DirectSound cooperative level.  Continuing without sound.",
			failed, 0);
		
		worked = false;
	}
	
	DSCAPS dscaps; 
	
	dscaps.dwSize = sizeof(DSCAPS); 
	hr = g_pDirectSound->GetCaps(&dscaps);
	
}

void LotaPlaySound(int snd, int mode, int restart)
{
	LotaSoundBuffer *next;
	LotaSoundBuffer *last;
	HRESULT			hr;
	bool			found = false;
	
	if (!worked)
		return;
	
	next = first;
	last = first;
	
	while (next && !found)
	{
		if (snd == next->soundID)
		{
			found = true;
			
			if (restart)
			{
				next->buffer->SetCurrentPosition(0);
			}
			
			hr = next->buffer->Play(0, 0, mode);
			if (hr == DSERR_BUFFERLOST)
			{
				next->buffer->Restore();
				next->buffer->Play(0,0,mode);
			}
			
			next->buffer->SetVolume(0);
			
		}
		
		last = next;
		next = next->next;
	}
	
	if (found == false)
	{
		if (totalSounds > 0)
		{
			last->next = new LotaSoundBuffer;
			next = last->next;
		}
		else
		{
			first = new LotaSoundBuffer;
			next = first;
		}
		
		next->buffer = DSLoadSoundBuffer(g_pDirectSound, MAKEINTRESOURCE(snd));
		next->soundID = snd;
		next->next = NULL;
		
		if (next->buffer)
		{
			totalSounds++;
			
			next->buffer->Play(0,0,mode);
		}
		
	}
	
	
}

void LotaStopSound(int snd)
{
	LotaSoundBuffer *next;
	LotaSoundBuffer *last;
	bool			found = false;
	HRESULT			hr;
	
	if (!worked)
		return;

	next = first;
	last = first;
	
	while (next && !found)
	{
		if (snd == next->soundID)
		{
			found = true;
			next->buffer->SetCurrentPosition(0);
			
			hr = next->buffer->Stop();
		}
		
		last = next;
		next = next->next;
	}
	
}

DWORD LotaGetSoundStatus(int snd)
{
	DWORD		stat;
	bool		found = false;
	LotaSoundBuffer *next;
	
	if (!worked)
		return 0;

	next = first;
	
	while (next && !found)
	{
		if (snd == next->soundID)
		{
			found = true;
			next->buffer->GetStatus(&stat);
		}
		
		next = next->next;
	}
	
	if (found)
		return stat;
	
	return 0;
	
}

void LotaFadeSound(int snd, double fade, int mode)
{
	LotaSoundBuffer *next;
	bool			found = false;

	if (!worked)
		return;

	next = first;
	
	while (next && !found)
	{
		if (snd == next->soundID)
		{
			found = true;
			
			if (next->fading == 0 && fade > 0)
			{
				//LotaPlaySound(next->soundID, mode, true);
				next->buffer->SetVolume(DSBVOLUME_MIN);
				next->firstFade = clock();
				
			}
			else if (next->fading == 0 && fade < 0)
			{
				next->buffer->SetVolume(0);
				next->firstFade = clock();
			}
			

			if (next->fading == 0)
				next->fading = fade;
			
		}
		next = next->next;
	}
}

void CheckFade()
{
	if (!worked)
		return;

	LotaSoundBuffer *next;
	long			vol = 0;
	static int		lastTime;
	int				thisTime = clock();
	HRESULT			hr;
	int				newVol;
	const int		fadeTime = 50;
	const double	fadeSecs = 2;
	double			x0 = 0, x1 = 0;
	const int		volDif = DSBVOLUME_MAX - DSBVOLUME_MIN;
	
	if (lastTime + 1000 / fadeTime < thisTime)
	{
		
		next = first;
		
		while (next)
		{
			if (next->fading)
			{
				
				hr = next->buffer->GetVolume(&vol);
				
				// store amount of time passed
				x1 = thisTime - next->firstFade;
				x1 /= 1000;
				
				// Logarithmic volume fade
				//newVol = int((log10 (9 * ((x1 - fadeSecs / 2) * next->fading + fadeSecs
				//	/ 2) / fadeSecs + 1) - 1) * volDif);
				
				newVol = (int)( 1.443E+4 * log10(4 - x1/fadeSecs * abs(next->fading)) - 1E+4);

				if (x1 > fadeSecs)
				{
					if (next->fading > 0)
						newVol = DSBVOLUME_MAX;
					else
						newVol = DSBVOLUME_MIN;
				}

				if (newVol >= DSBVOLUME_MAX)
				{
					newVol = DSBVOLUME_MAX;
					next->fading = 0;
				}
				else if (newVol <= DSBVOLUME_MIN)
				{
					newVol = DSBVOLUME_MIN;
					next->fading = 0;
					next->buffer->Stop();
					next->buffer->SetCurrentPosition(0);
				}
				
				hr = next->buffer->SetVolume(newVol);
				
			}
			
			next = next->next;
		}
		
		lastTime = thisTime;
	}
	
}

void StopAllSounds()
{
	if (!worked)
		return;

	LotaSoundBuffer *next = first;
	
	while (next)
	{
		next->buffer->Stop();
		next = next->next;
		
	}
	
}

void ShutDownDSound()
{
	LotaSoundBuffer *next = first;
	
	while (next)
	{
		next->buffer->Stop();
		next->buffer->Release();
		next = next->next;
	}
	
	delete first;
	
	if (worked && g_pDirectSound)
		g_pDirectSound->Release();
	
}

