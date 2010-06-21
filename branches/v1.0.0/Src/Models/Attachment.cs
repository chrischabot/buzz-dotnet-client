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

namespace Google.Api.Buzz.Models {
  public class Attachment {
    private string type;
    private string title;
    private string content;
    private IList<Link> links;
    
    public Attachment() {
    
    }
    
    public Attachment(string type, string title, string content, IList<Link> links) {
      this.type = type;
      this.title = title;
      this.content = content;
      this.links = links;
    }
    
    public string Type {
      get { return type; }
      set { type = value; }
    }
    
    public string Title {
      get { return title; }
      set { title = value; }
    }
    
    public string Content {
      get { return content; }
      set { content = value; }
    }
    
    public IList<Link> Links {
      get { return links; }
      set { links = value; }
    }
    
    public void AddLink(Link link) {
      if(links == null) {
        links = new List<Link>();
      }
      links.Add(link);
    }
  }
}
