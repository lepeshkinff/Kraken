using System;
using System.Threading.Tasks;

namespace Kraken.Engine
{
	public interface IHttpClient
	{
		Task<string> GetStringAsync(Uri uri);
	}
}
