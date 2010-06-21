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

  public class Entry {
    private string published;
    private string id;
    private Person author;
    private string content;
    private ActivityObject activityObject;
    private IList<Link> links;
    
    public Entry() {
    
    }
    
    public Entry(string published, string id, Person author, string content, 
      ActivityObject activityObject, IList<Link> links) {
      this.published = published;
      this.id = id;
      this.author = author;
      this.content = content;
      this.activityObject = activityObject;
      this.links = links;
    }

    public string Published {
      get { return published; }
      set { published = value; }
    }

    public string Id {
      get { return id; }
      set { id = value; }
    }

    public Person Author {
      get { return author; }
      set { author = value; }
    }
    
    public string Content {
      get { return content; }
      set { content = value; }
    }
    
    public ActivityObject ActivityObject {
      get { return activityObject; }
      set { activityObject = value; }
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