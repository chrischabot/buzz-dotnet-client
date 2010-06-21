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
   public class BuzzAclEntry {
     private string id;
     private string name;
     private string uri;
     
     public BuzzAclEntry() {
     
     }
     
     public BuzzAclEntry(string id, string name, string uri) {
       this.id = id;
       this.name = name;
       this.uri = uri;
     }
     
     public string Id {
       get { return id; }
       set { id = value; }
     }
     
     public string Name {
       get { return name; }
       set { name = value; }
     }
     
     public string Uri {
       get { return uri; }
       set { uri = value; }
     }
   }
 }