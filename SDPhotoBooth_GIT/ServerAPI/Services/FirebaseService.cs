using Google;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Threading.Tasks;

public class FirebaseService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public FirebaseService(IConfiguration configuration)
    {
        var firebaseConfig = configuration.GetSection("Firebase").Get<Dictionary<string, string>>();
        if (firebaseConfig == null || !firebaseConfig.Any())
        {
            throw new InvalidOperationException("Firebase configuration is missing or invalid.");
        }

        if (firebaseConfig.ContainsKey("private_key"))
        {
            firebaseConfig["private_key"] = firebaseConfig["private_key"].Replace("\\n", "\n");
        }

        var jsonCredentials = System.Text.Json.JsonSerializer.Serialize(firebaseConfig);

        var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(jsonCredentials);
        _storageClient = StorageClient.Create(credential);
        _bucketName = firebaseConfig["project_id"] + ".appspot.com";
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
    {
        await _storageClient.UploadObjectAsync(
            _bucketName, fileName, contentType, imageStream
        );

        return $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(fileName)}?alt=media";
    }

    public async Task<bool> IsExistsAsync(string fileName)
    {
        try
        {
            var obj = await _storageClient.GetObjectAsync(_bucketName, fileName);
            return obj != null;
        }
        catch (GoogleApiException ex)
        {
            if (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            throw;
        }
    }
    public async Task<string> UploadIfNotExistsAsync(Stream imageStream, string fileName, string contentType)
    {
        if (await IsExistsAsync(fileName))
        {
            return $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(fileName)}?alt=media";
        }

        await _storageClient.UploadObjectAsync(_bucketName, fileName, contentType, imageStream);
        return $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(fileName)}?alt=media";
    }

}
