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

namespace Google.Api.Buzz.Utils {

	public class Constants {
	  public const string ACTIVITY = "activity";
	  public const string ATOM = "atom";
	  public const string BUZZ = "buzz";
	  public const string CROSSPOST = "crosspost";
	  public const string GD = "gd";
	  public const string GEORSS = "georss";
	  public const string MEDIA = "media";
	  public const string POCO = "poco";
	  public const string THREAD = "thr";
	
	  public const string XMLNS_ACTIVITY = "http://activitystrea.ms/spec/1.0/";
	  public const string XMLNS_ATOM = "http://www.w3.org/2005/Atom";
	  public const string XMLNS_BUZZ = "http://schemas.google.com/buzz/2010";
	  public const string XMLNS_CROSSPOST = "http://purl.org/syndication/cross-posting";
	  public const string XMLNS_GD = "http://schemas.google.com/g/2005";
	  public const string XMLNS_GEORSS = "http://www.georss.org/georss";
	  public const string XMLNS_MEDIA = "http://search.yahoo.com/mrss/";
	  public const string XMLNS_POCO = "http://portablecontacts.net/ns/1.0";
	  public const string XMLNS_THREAD = "http://purl.org/syndication/thread/1.0";
	  
	  public const string ACLENTRY = "aclentry";
	  public const string ATTACHMENT = "attachment";
	  public const string AUTHOR = "author";
	  public const string BUZZ_COUNT = "buzz:count";
	  public const string CONTENT = "content";
	  public const string ENTRY = "entry";
	  public const string FEED = "feed";
	  public const string ID = "id";
	  public const string GENERATOR = "generator";
	  public const string HREF = "href";
	  public const string IN_REPLY_TO = "in-reply-to";
	  public const string LINK = "link";
	  public const string MEDIA_HEIGHT = "media:height";
	  public const string MEDIA_WIDTH = "media:width";
	  public const string NAME = "name";
	  public const string OBJECT = "object";
	  public const string OBJECT_TYPE = "object-type";
	  public const string PHOTO_URL = "photoUrl";
	  public const string PUBLISHED = "published";
	  public const string REL = "rel";
	  public const string SERVICE = "service";
	  public const string SOURCE = "source";
	  public const string TITLE = "title";
	  public const string THREAD_COUNT = "thr:count";
	  public const string THREAD_UPDATED = "thr:updated";
	  public const string TYPE = "type";
	  public const string UPDATED = "updated";
	  public const string URI = "uri";
	  public const string VISIBILITY = "visibility";
	  public const string VERB = "verb";
	  
	  public const string ALTERNATE_REL = "alternate";
	  public const string API_URI = "https://www.googleapis.com/buzz/v1/";
	  public const string URI_ACTIVITIES = "activities/";
	  public const string URI_CONSUMPTION = "@consumption";
	  public const string URI_LIKED = "@liked";
	  public const string URI_ME ="@me/";
	  public const string URI_PEOPLE = "people/";
	  public const string URI_PUBLIC = "@public";
	  public const string URI_SEARCH_FOR = "search?q=";
	  public const string URI_SELF = "@self";
	  public const string FEED_TYPE_ACTIVITY = "buzz#activityFeed";
	  public const string FEED_TYPE_COMMENT = "buzz#commentFeed";
	  public const string FEED_TYPE_SEARCH = "buzz#searchFeed";
	  public const string LIKERS_REL = "http://schemas.google.com/buzz/2010#likers";
	  public const string REPLIES_REL = "replies";
	}
}
