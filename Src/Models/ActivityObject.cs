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

// Author: prashaantt@gmail.com (Prashant Tiwari)

using System;
using System.Collections.Generic;

namespace Google.Api.Buzz.Models {
  /// <summary>
  /// 
  /// </summary>
  public class ActivityObject {
    /// <summary>
    /// 
    /// </summary>
    private string type;

    /// <summary>
    /// 
    /// </summary>
    private string content;

    /// <summary>
    /// 
    /// </summary>
    private Link link;

    /// <summary>
    /// 
    /// </summary>
    private IList<Attachment> attachments;

    public ActivityObject() {

    }

    public ActivityObject(string type, string content, Link link, IList<Attachment> attachments) {
      this.type = type;
      this.content = content;
      this.link = link;
      this.attachments = attachments;
    }

    public string Type {
      get { return type; }
      set { type = value; }
    }

    public string Content {
      get { return content; }
      set { content = value; }
    }

    public Link Link {
      get { return link; }
      set { link = value; }
    }

    public IList<Attachment> Attachments {
      get { return attachments; }
      set { attachments = value; }
    }

    public void AddAttachment(Attachment attachment) {
      if (attachments == null) {
        attachments = new List<Attachment>();
      }
      attachments.Add(attachment);
    }
  }
}
