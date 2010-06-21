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

  public class Feed {
    private string id;
    private string title;
    private IList<Entry> entries;
    private string generator;
    private FEED_TYPE type;
    private IList<Link> links;
    private string updated;
    
	public enum FEED_TYPE { ACTIVITY, COMMENT, SEARCH };
    
    public Feed (FEED_TYPE type) {
      this.type = type;
    }

    public Feed(FEED_TYPE type, string id, string title, IList<Entry> entries, string generator, 
      IList<Link> links, string updated) {
      this.id = id;
      this.title = title;
      this.updated = updated;
      this.links = links;
      this.entries = entries;
      this.generator = generator;
      this.type = type;
    }
    
    public string Id {
      get { return id; }
      set { id = value; }
    }
    
    public string Title {
      get { return title; }
      set { title = value; }
    }
    
    public string Updated {
      get { return updated; }
      set { updated = value; }
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

    public IList<Entry> Entries {
      get { return entries; }
      set { entries = value; }
    }

    public void AddEntry(Entry entry) {
      if(entries == null) {
        entries = new List<Entry>();
      }
      entries.Add(entry);
    }

    public string Generator {
      get { return generator; }
      set { generator = value; }
    }
    
    public FEED_TYPE Type {
      get { return type; }
      set { type = value; }
    }
  }
}
