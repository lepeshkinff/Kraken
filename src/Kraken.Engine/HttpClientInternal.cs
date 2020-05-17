using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kraken.Engine
{
	public class HttpClientInternal : IHttpClient
	{
		private readonly HttpClient httpClient;

		public HttpClientInternal(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public Task<string> GetStringAsync(Uri uri) =>
			httpClient.GetStringAsync(uri);
	}
}
