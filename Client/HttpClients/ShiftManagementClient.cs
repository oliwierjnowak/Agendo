using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agendo.Client.HttpClient
{
	public class ShiftManagementClient
	{
		private readonly HttpClient _client;

		public ShiftManagementClient(HttpClient httpClient)
		{
			_client = new httpClient;

        }
	}
}

