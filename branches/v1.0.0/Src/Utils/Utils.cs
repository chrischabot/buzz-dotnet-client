/*
 * Copyright 2010 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Google.GData.Client;
using Google.Api.Buzz.Models;

namespace Google.Api.Buzz.Utils {
 /// <summary>
 /// Provides utility methods for handling the feed.
 /// </summary>
    public class Utils
    {
        public enum LINK_TYPE { NOTE, PHOTO, VIDEO };
        /// <summary>
        /// Retrieves a feed from the web given its url.
        /// </summary>
        /// <param name="feedUrl">The url of the feed to be retrieved.</param>
        /// <returns>The feed as an array of bytes.</returns>
        public static XmlDocument GetFeedXML(string url, Buzz buzz)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Encoding.UTF8.GetString(Utils.GetFeedFromUrl(url, String.Empty, buzz)));
            return xmlDoc;
        }
        public static XmlDocument GetFeedXML(string url, string postData, Buzz buzz)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Encoding.UTF8.GetString(Utils.GetFeedFromUrl(url, postData, buzz)));
            return xmlDoc;
        }

        public static byte[] GetFeedFromUrl(string feedUrl, string postData, Buzz buzz)
        {
            string httpMthod = "GET";
            if (!String.IsNullOrEmpty(postData))
            {
                httpMthod = "POST";
            }
            else
                httpMthod = "GET";
            return GetFeedFromUrl(feedUrl, httpMthod, postData, buzz);
        }

        public static byte[] GetFeedFromUrl(string feedUrl, string httpMethod, string postData, Buzz buzz)
        {

            int bufferSize = 2 << 20;
            WebRequest request = WebRequest.Create(feedUrl);
            request.Method = httpMethod;
            HttpWebRequest http = request as HttpWebRequest;
            string oauthHeader = OAuthUtil.GenerateHeader(http.RequestUri,
                                                                 buzz.ConsumerKey,
                                                                 buzz.ConsumerSecret,
                                                                 buzz.Token,
                                                                 buzz.TokenSecret,
                                                                 request.Method);
            request.Headers.Remove("Authorization");
            request.Headers.Add(oauthHeader);

            if (!String.IsNullOrEmpty(postData))
            {
                StreamWriter requestWriter = null;
                Encoding encoding = Encoding.GetEncoding("utf-8");
                byte[] data = encoding.GetBytes(postData);
                request.ContentLength = data.Length;
                request.ContentType = "application/atom+xml";// "text/xml"; //"application/x-www-form-urlencoded";

                requestWriter = new StreamWriter(request.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }
            else if (request.Method == "PUT")
                request.ContentLength = 0;

            WebResponse response = request.GetResponse();

            MemoryStream memStream = new MemoryStream();
            using (Stream responseStream = response.GetResponseStream())
            {
                byte[] strmBuffer = new byte[bufferSize];

                int bytesRead = responseStream.Read(strmBuffer, 0, bufferSize);
                while (bytesRead != 0)
                {
                    memStream.Write(strmBuffer, 0, bytesRead);
                    bytesRead = responseStream.Read(strmBuffer, 0, bufferSize);
                }
            }
            return memStream.ToArray();
        }

        public static bool IsNotEmptyString(string str)
        {
            return (str != null && str.Length > 0);
        }
        public static string constructComment(string comment)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<entry xmlns=\"http://www.w3.org/2005/Atom\" ><content>");
            sb.Append(comment);
            sb.Append("</content></entry>");
            return sb.ToString();
        }

        /**
       * Generates new post request body.
       * 
       * @param content of the new post
       * @param link (if any)
       * @return the xml representation of the new entry.
       * @throws BuzzValidationException if any required value is missing.
       */
        public static string constructPayload(string content, string contentType, Link link, string linkTitle, LINK_TYPE linkType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<entry xmlns=\"http://www.w3.org/2005/Atom\" xmlns:activity=\"http://activitystrea.ms/spec/1.0/\" xmlns:buzz=\"http://schemas.google.com/buzz/2010\" >");
            sb.Append("<activity:object><activity:object-type>http://activitystrea.ms/schema/1.0/note</activity:object-type>");
            sb.Append(constructContent(content, contentType));
            if (String.IsNullOrEmpty(linkTitle))
                linkTitle = "link";
            if ( linkType == LINK_TYPE.PHOTO)
                sb.Append(link != null ? constructPhoto(linkTitle, link) : "");
            else if ( linkType == LINK_TYPE.VIDEO)
                sb.Append(link != null ? constructVideo(linkTitle, link) : "");
            else
                sb.Append(link != null ? constructLink(linkTitle, link) : "");
            sb.Append("</activity:object></entry>");
            return sb.ToString();
        }

        private static string constructContent(string content, string type)
        {
            if (String.IsNullOrEmpty(content))
            {
                return ""; ;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<content type=\"");
            if (String.IsNullOrEmpty(type))
                sb.Append("html");
            else
                sb.Append(type);
            sb.Append("\">");
            sb.Append(content);
            sb.Append("</content>");
            return sb.ToString();
        }

        private static string constructLink(string title, Link link)
        {
            if (link == null || String.IsNullOrEmpty(link.Href)
                || String.IsNullOrEmpty(link.Type) || String.IsNullOrEmpty(link.Rel))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<buzz:attachment><activity:object-type>http://activitystrea.ms/schema/1.0/article</activity:object-type><title>");
            sb.Append(title).Append("</title>");
            sb.Append("<link rel=\"");
            sb.Append(link.Rel); //alternate
            sb.Append("\" type=\"");
            sb.Append(link.Type); //type="text/html"
            sb.Append("\" href=\"");
            sb.Append(link.Href);
            sb.Append("\" /></buzz:attachment>");
            return sb.ToString();
        }

        private static string constructPhoto(string title, Link link)
        {
            if (link == null || String.IsNullOrEmpty(link.Href)
                || String.IsNullOrEmpty(link.Type) || String.IsNullOrEmpty(link.Rel))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<buzz:attachment><activity:object-type>http://activitystrea.ms/schema/1.0/photo</activity:object-type><title>");
            sb.Append(title).Append("</title>");
            sb.Append("<link rel=\"");
            sb.Append(link.Rel); //enclosure
            sb.Append("\" type=\"");
            sb.Append(link.Type); //type="image/png"
            sb.Append("\" href=\"");
            sb.Append(link.Href);
            sb.Append("\" /></buzz:attachment>");
            return sb.ToString();
        }

        private static string constructVideo(string title, Link link)
        {
            if (link == null || String.IsNullOrEmpty(link.Href)
                || String.IsNullOrEmpty(link.Type) || String.IsNullOrEmpty(link.Rel))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<buzz:attachment><activity:object-type>http://activitystrea.ms/schema/1.0/video</activity:object-type><title>");
            sb.Append(title).Append("</title>");
            sb.Append("<link rel=\"");
            sb.Append(link.Rel); //alternate
            sb.Append("\" type=\"");
            sb.Append(link.Type); //type="text/html"
            sb.Append("\" href=\"");
            sb.Append(link.Href);
            sb.Append("\" /></buzz:attachment>");
            return sb.ToString();
        }
    }
}