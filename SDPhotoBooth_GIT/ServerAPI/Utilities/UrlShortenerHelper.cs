namespace ServerAPI.Utilities
{
    public class UrlShortenerHelper
    {
        public static async Task<List<string>> ShortenUrlsParallel(string[] longUrls)
        {
            using (var client = new HttpClient())
            {
                var tasks = longUrls.Select(async url =>
                {
                    try
                    {
                        string requestUrl = $"https://tinyurl.com/api-create.php?url={Uri.EscapeDataString(url)}";
                        return await client.GetStringAsync(requestUrl);
                    }
                    catch
                    {
                        return null;
                    }
                });

                var shortUrls = await Task.WhenAll(tasks);
                return shortUrls.ToList();
            }
        }

    }
}
