using System;

namespace Tomefico.Service;

public static class DeviceIdService
{
    private const string DeviceIdKey = "TomeficoDeviceId";

    public static async Task<string> GetOrCreateDeviceId()
    {
#if DEBUG
        if (Preferences.ContainsKey(DeviceIdKey))
            return Preferences.Get(DeviceIdKey, string.Empty);

        var newId = Guid.NewGuid().ToString();

        Preferences.Set(DeviceIdKey, newId);
        return newId;
#else
        var existing = await SecureStorage.GetAsync(DeviceIdKey);
        if(!string.IsNullOrEmpty(existing))
            return existing;

        var newId = Guid.NewGuid().ToString();
        await SecureStorage.SetAsync(DeviceIdKey,newId);
        return newId;
#endif
    }
    public static void ResetDeviceId()
    {
#if DEBUG
        Preferences.Remove(DeviceIdKey);
#else
        SecureStorage.Remove(DeviceIdKey);
#endif
    }
}
