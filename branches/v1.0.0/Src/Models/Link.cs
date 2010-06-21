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

namespace Google.Api.Buzz.Models {
  public class Link {
    private string rel;
    private string href;
    private string type;
    private string mediaHeight;
    private string mediaWidth;
    private int threadCount;
    private string threadUpdated;
    private int buzzCount;
    
    public Link () {
    
    }
    
    public Link(string rel, string href, string type, string mediaHeight, string mediaWidth,
      int threadCount, string threadUpdated, int buzzCount) {
      this.rel = rel;
      this.href = href;
      this.type = type;
      this.mediaHeight = mediaHeight;
      this.mediaWidth = mediaWidth;
      this.threadCount = threadCount;
      this.threadUpdated = threadUpdated;
      this.buzzCount = buzzCount;
    }
    
    public string Rel {
      get { return rel; }
      set { rel = value; }
    }
    
    public string Href {
      get { return href; }
      set { href = value; }
    }
    
    public string Type {
      get { return type; }
      set { type = value; }
    }
    
    public string MediaHeight {
      get { return mediaHeight; }
      set { mediaHeight = value; }
    }
    
    public string MediaWidth {
      get { return mediaWidth; }
      set { mediaWidth = value; }
    }
    
    public int ThreadCount {
      get { return threadCount; }
      set { threadCount = value; }
    }
    
    public string ThreadUpdated {
      get { return threadUpdated; }
      set { threadUpdated = value; }
    }
    
    public int BuzzCount {
      get { return buzzCount; }
      set { buzzCount = value; }
    }
  }
}
