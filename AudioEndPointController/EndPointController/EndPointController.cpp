// EndPointController.cpp : Defines the entry point for the console application.
//
#include <stdio.h>
#include <wchar.h>
#include <tchar.h>
#include "windows.h"
#include "Mmdeviceapi.h"
#include "PolicyConfig.h"
#include "Propidl.h"
#include "Functiondiscoverykeys_devpkey.h"

HRESULT SetDefaultAudioPlaybackDevice(LPCWSTR devID)
{	
	IPolicyConfigVista *pPolicyConfig;
	ERole reserved = eMultimedia;

    HRESULT hr = CoCreateInstance(__uuidof(CPolicyConfigVistaClient), 
		NULL, CLSCTX_ALL, __uuidof(IPolicyConfigVista), (LPVOID *)&pPolicyConfig);
	if (SUCCEEDED(hr))
	{
		hr = pPolicyConfig->SetDefaultEndpoint(devID, reserved);
		pPolicyConfig->Release();
	}
	return hr;
}

HRESULT GetDeviceName(IMMDevice* pDevice, char** outString) 
{
	HRESULT hr = CoInitialize(NULL);

	IPropertyStore *pStore;
	hr = pDevice->OpenPropertyStore(STGM_READ, &pStore); //get the properties of the current device
	if (SUCCEEDED(hr))
	{
		PROPVARIANT friendlyName;
		PropVariantInit(&friendlyName);
		hr = pStore->GetValue(PKEY_Device_FriendlyName, &friendlyName); //get the readably name of the current device
		if (SUCCEEDED(hr))
		{
			*outString = (char*)friendlyName.pwszVal;
			PropVariantClear(&friendlyName);
		}
		pStore->Release();
	}
	
	return hr;
}

// EndPointController.exe [NewDefaultDeviceID]
int _tmain(int argc, _TCHAR* argv[])
{
	// read the command line option, -1 indicates list devices.
	int option = -1;
	if (argc == 2) option = atoi((char*)argv[1]);

	if (option == -2) //if default is requested 
	{
		HRESULT hr = CoInitialize(NULL);
		if (SUCCEEDED(hr))
		{
			IMMDeviceEnumerator *pEnum = NULL;
			// Create a multimedia device enumerator.
			hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL,
				CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (void**)&pEnum);
			if (SUCCEEDED(hr))
			{
				IMMDevice *pDevice = NULL;
				hr = pEnum->GetDefaultAudioEndpoint(EDataFlow::eRender, ERole::eMultimedia, &pDevice);
				if (SUCCEEDED(hr)) 
				{
					char* name = NULL;
					hr = GetDeviceName(pDevice, &name);
					if (SUCCEEDED(hr))
					{
						LPWST
						hr = pDevice->GetId()
					}
				}
				pDevice->Release();
			}
		}
		return hr;
	}

	HRESULT hr = CoInitialize(NULL);
	if (SUCCEEDED(hr))
	{
		IMMDeviceEnumerator *pEnum = NULL;
		// Create a multimedia device enumerator.
		hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL,
			CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (void**)&pEnum);
		if (SUCCEEDED(hr))
		{
			IMMDeviceCollection *pDevices;
			// Enumerate the output devices.
			hr = pEnum->EnumAudioEndpoints(eRender, DEVICE_STATE_ACTIVE, &pDevices);
			if (SUCCEEDED(hr))
			{
				UINT count;
				pDevices->GetCount(&count);
				if (SUCCEEDED(hr))
				{
					for (int i = 0; i < count; i++) //loop through all devices
					{
						IMMDevice *pDevice;
						hr = pDevices->Item(i, &pDevice); //get current device
						if (SUCCEEDED(hr))
						{
							LPWSTR wstrID = NULL;
							hr = pDevice->GetId(&wstrID); //get the id of the current device
							if (SUCCEEDED(hr))
							{
								IPropertyStore *pStore;
								hr = pDevice->OpenPropertyStore(STGM_READ, &pStore); //get the properties of the current device
								if (SUCCEEDED(hr))
								{
									PROPVARIANT friendlyName;
									PropVariantInit(&friendlyName);
									hr = pStore->GetValue(PKEY_Device_FriendlyName, &friendlyName); //get the readably name of the current device
									if (SUCCEEDED(hr))
									{
										// if no options, print the device
										// otherwise, find the selected device and set it to be default
										if (option == -1) printf("%d %ws\n",i, friendlyName.pwszVal);
										if (i == option) SetDefaultAudioPlaybackDevice(wstrID);
										PropVariantClear(&friendlyName);
									}
									pStore->Release();
								}
							}
							pDevice->Release();
						}
					}
				}
				pDevices->Release();
			}
			pEnum->Release();
		}
	}
	fflush(stdout);
	return hr;
}