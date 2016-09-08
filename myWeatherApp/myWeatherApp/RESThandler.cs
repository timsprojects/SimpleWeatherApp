using System;
using RestSharp;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace myWeatherApp
{
    class RESThandler
    {
        private string url;
        private IRestResponse response;
        private RestRequest request;

        public RESThandler()
        {
            url = "";
        }

        public RESThandler(string lurl)
        {
            url = lurl;
            request = new RestRequest();
        }

        //public void AddParamater(string name, string value)
        //{
        //    if (request != null)
        //    {
        //        request.AddParameter(name, value);
        //    }
        //}

        //public RootObject ExecuteRequest()
        //{
        //    var client = new RestClient(url);

        //    response = client.Execute(request);

        //    RootObject objRoot = new RootObject();
        //    objRoot = JsonConvert.DeserializeObject<RootObject>(response.Content);

        //    return objRoot;
        //}

        public async Task<RootObject> ExecuteRequestAsync()
        {
            var client = new RestClient(url);
            var request = new RestRequest();

            response = await client.ExecuteTaskAsync(request);

            RootObject objRoot = new RootObject();
            objRoot = JsonConvert.DeserializeObject<RootObject>(response.Content);

            return objRoot;
        }

    }
}