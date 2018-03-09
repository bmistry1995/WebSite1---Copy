using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for RestClient
/// </summary>
public enum httpVerb
{
    GET, POST, PUT, DELETE
}
public class RestClient
{
    public string endPoint { get; set; }
    public httpVerb httpMethod { get; set; }
    public RestClient()
    {
        endPoint = string.Empty;
        httpMethod = httpVerb.GET;
    }

    public string makeRequest()
    {
        string responseValue = string.Empty;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
        request.UseDefaultCredentials = true;
        request.Method = httpMethod.ToString();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("error code: " + response.StatusCode.ToString());
                }//end of iff

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }//end of stream reader
                    }//enf if null
                }//end of using response stream
            }//end of using response
        return responseValue;
    }
}