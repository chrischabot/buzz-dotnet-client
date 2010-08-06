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

  public class Like
  {
    private string id;
    private Person liker;
    private Link sourcePost;
    
    public Like() {
    
    }
    
    public Like(string id, Person liker, Link sourcePost) {
      this.id = id;
      this.liker = liker;
      this.sourcePost = sourcePost;
    }
    
    public string Id {
      get { return id; }
      set { id = value; }
    }
    
    public Person Liker {
      get { return liker; }
      set { liker = value; }
    }
    
    public Link SourcePost {
	  get { return sourcePost; }
	  set { sourcePost = value; }
	}
  }
}