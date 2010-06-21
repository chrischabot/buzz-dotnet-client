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

namespace Google.Api.Buzz.Utils {
 /// <summary>
 /// Provides utility methods for handling the feed.
 /// </summary>
 public class Utils {
   /// <summary>
   /// Retrieves a feed from the web given its url.
   /// </summary>
   /// <param name="feedUrl">The url of the feed to be retrieved.</param>
   /// <returns>The feed as an array of bytes.</returns>
   public static XmlDocument GetFeedXML(string url) {
     XmlDocument xmlDoc = new XmlDocument();
     xmlDoc.LoadXml(Encoding.UTF8.GetString(Utils.GetFeedFromUrl(url)));
     return xmlDoc;
   }
   
   private static byte[] GetFeedFromUrl(string feedUrl) {
     int bufferSize = 2 << 20;
     WebRequest request = HttpWebRequest.Create(feedUrl);
     WebResponse response = request.GetResponse();

     MemoryStream memStream = new MemoryStream();
     using (Stream responseStream = response.GetResponseStream()) {
       byte[] strmBuffer = new byte[bufferSize];

       int bytesRead = responseStream.Read(strmBuffer, 0, bufferSize);
       while (bytesRead != 0) {
         memStream.Write(strmBuffer, 0, bytesRead);
         bytesRead = responseStream.Read(strmBuffer, 0, bufferSize);
       }
     }
     return memStream.ToArray();
   }
   
   public static bool IsNotEmptyString(string str) {
     return (str != null && str.Length > 0);
   }
 }
}