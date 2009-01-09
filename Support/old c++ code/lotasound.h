
#ifndef __LOTASOUND__
#define __LOTASOUND__

extern LPDIRECTSOUND					g_pDirectSound;		// Direct Sound Device

class LotaSoundBuffer {

public:

	int						soundID;
	LPDIRECTSOUNDBUFFER		buffer;
	LotaSoundBuffer			*next;
	double					fading;
	int						firstFade;

public:
	~LotaSoundBuffer();
	LotaSoundBuffer();

};

void InitDSound();						// Initializes Direct Sound
void LotaPlaySound(int snd, int mode=0, int restart = true);// Plays a sound buffer
void LotaStopSound(int snd);			// Stops a continuous ocean sound
void ShutDownDSound();					// Shuts down Direct Sound
DWORD LotaGetSoundStatus(int snd);		// returns status on a sound
void LotaFadeSound(int snd, double fade, int mode = 0);
void CheckFade();
void StopAllSounds();

#endif