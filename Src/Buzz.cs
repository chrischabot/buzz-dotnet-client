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

using Google.Api.Buzz.Utils;
using Google.Api.Buzz.Models;

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Google.Api.Buzz {

  public class Buzz {

      private string consumerKey;
      private string consumerSecret;
      private string token;
      private string tokenSecret;
      private int maxResults = 20;
      private int maxComments = 3;
      private string continuationToken = String.Empty;

    Parser parser = new Parser();

      public Buzz(string consumerkey, string consumersecret)
      {
          this.consumerKey = consumerkey;
          this.consumerSecret = consumersecret;
          parser.setBuzz(this);
      }

      public string ConsumerSecret
      {
          get { return this.consumerSecret; }
          set { this.consumerSecret = value; }
      }
      // end of accessor public string ConsumerSecret

      //////////////////////////////////////////////////////////////////////
      /// <summary>accessor method public string ConsumerKey</summary> 
      /// <returns>the ConsumerKey used for the oauth request </returns>
      //////////////////////////////////////////////////////////////////////
      public string ConsumerKey
      {
          get { return this.consumerKey; }
          set { this.consumerKey = value; }
      }
      // end of accessor public string ConsumerKey

      //////////////////////////////////////////////////////////////////////
      /// <summary>accessor method public string TokenSecret</summary> 
      /// <returns>the TokenSecret for the oauth request </returns>
      //////////////////////////////////////////////////////////////////////
      public string TokenSecret
      {
          get { return this.tokenSecret; }
          set { this.tokenSecret = value; }
      }
      // end of accessor public string ConsumerSecret

      //////////////////////////////////////////////////////////////////////
      /// <summary>accessor method public string TokenSecret</summary> 
      /// <returns>the Token for the oauth request </returns>
      //////////////////////////////////////////////////////////////////////
      public string Token
      {
          get { return this.token; }
          set { this.token = value; }
      }
      public int MaxResults
      {
          get { return this.maxResults; }
          set { this.maxResults = value; }
      }
      public int MaxComments
      {
          get { return this.maxComments; }
          set { this.maxComments = value; }
      }
      public string ContinuationToken
      {
          get { return this.continuationToken; }
          set { this.continuationToken = value; }
      }

    public enum FEED_SCOPE { SELF, PUBLIC, CONSUMPTION, LIKED, GROUP };
    
    public Feed Search(string query) {
      return Search(query, null);
    }
    
    public Feed Search(string query, string geo) {
      //fix geo
        StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
      Append(Constants.URI_ACTIVITIES).Append(Constants.URI_SEARCH_FOR).Append(query).Append("&max-results=").Append(maxResults).Append("&max-comments=").Append(maxComments);
        if (!String.IsNullOrEmpty(continuationToken))
            buf.Append("&c=").Append(continuationToken);
        return parser.Parse(buf.ToString());
      //return  parser.Parse(new StringBuilder().Append(Constants.API_URI).
      //  Append(Constants.URI_ACTIVITIES).Append(Constants.URI_SEARCH_FOR).Append(query).ToString());
  }

      public Feed GetAllPublicPosts()
      {
          return GetPosts(Constants.URI_ALL, FEED_SCOPE.PUBLIC);
      }

      public Feed GetPosts()
      {
          return GetPosts(Constants.URI_ME, FEED_SCOPE.SELF);
      }

      public Feed GetPosts(FEED_SCOPE scope)
      {
          return GetPosts(Constants.URI_ME, scope);
      }
    
    public Feed GetPosts(string userId, FEED_SCOPE scope) {
      if (userId == null) {
        //throw exception
      }
      //check for exception in response
      StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
      Append(Constants.URI_ACTIVITIES).Append(userId).Append("/").Append(DecodeFeedScope(scope)).Append("?max-results=").Append(maxResults).Append("&max-comments=").Append(maxComments);
      if (!String.IsNullOrEmpty(continuationToken))
          buf.Append("&c=").Append(continuationToken);
      return parser.Parse(buf.ToString());
    }

      public Post GetPost(string postId)
      {
          //      if (!Utils.IsNotEmptyString(postId)) {
          //        //exception
          //      }
          Feed postFeed = parser.Parse(new StringBuilder().Append(Constants.API_URI).
            Append(Constants.URI_ACTIVITIES).Append(Constants.URI_ME).Append("/").Append(Constants.URI_SELF).Append("/").Append(postId).ToString());
          if (postFeed != null && postFeed.Entries.Count > 0)
              return postFeed.Entries[0] as Post;
          else
              return null;
      }
    
    public Feed UpdatePost(string content, string contentType, Link link,string linkTitle, Google.Api.Buzz.Utils.Utils.LINK_TYPE linkType,string postId) {

        string postData = Utils.Utils.constructPayload(content, contentType, link, linkTitle, linkType);
        byte[] bytearray = Utils.Utils.GetFeedFromUrl(new StringBuilder().Append(Constants.API_URI).
              Append(Constants.URI_ACTIVITIES).Append(Constants.URI_ME).Append("/").Append(Constants.URI_SELF).Append("/").Append(postId).ToString(), "PUT", postData,this);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Encoding.UTF8.GetString(bytearray));
        Feed postFeed = parser.Parse(xmlDoc,Feed.FEED_TYPE.ACTIVITY);

        return postFeed;
    }

      public string DeletePost(string postId)
      {
          StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
        Append(Constants.URI_ACTIVITIES).Append(Constants.URI_ME).Append("/").Append(Constants.URI_SELF).Append("/").Append(postId);
          byte[] bytearray = Utils.Utils.GetFeedFromUrl(buf.ToString(), "DELETE", String.Empty, this);
          string s = System.Text.ASCIIEncoding.ASCII.GetString(bytearray);
          return s;
      }
      public Feed CreatePost(string content, string contentType, Link link,string linkTitle, Google.Api.Buzz.Utils.Utils.LINK_TYPE linkType)
      {
          string postData = Utils.Utils.constructPayload(content, contentType, link, linkTitle, linkType);
          Feed postFeed = parser.Parse(new StringBuilder().Append(Constants.API_URI).
                Append(Constants.URI_ACTIVITIES).Append(Constants.URI_ME).Append("/").Append(Constants.URI_SELF).ToString(), postData);
          return postFeed;
      }
    public IList<Like> GetLikes(string userId, string postId) {
      return null;
    }
      public Feed GetLikedPosts()
      {
          return GetLikedPosts(Constants.URI_ME);
      }
      public Feed GetLikedPosts(string userId)
      {
        if (userId == null)
        {
            //throw exception
        }
        //check for exception in response
        StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
        Append(Constants.URI_ACTIVITIES).Append(userId).Append("/").Append(DecodeFeedScope(FEED_SCOPE.LIKED)).Append("?max-results=").Append(maxResults).Append("&max-comments=").Append(maxComments);
        if (!String.IsNullOrEmpty(continuationToken))
            buf.Append("&c=").Append(continuationToken);
        return parser.Parse(buf.ToString());
    }
    
    public bool LikePost(string postId) {
        StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
        Append(Constants.URI_ACTIVITIES).Append(Constants.URI_ME).Append("/").Append(DecodeFeedScope(FEED_SCOPE.LIKED)).Append("/").Append(postId);

        byte[] bytearray = Utils.Utils.GetFeedFromUrl(buf.ToString(), "PUT", String.Empty, this);
        string s = System.Text.ASCIIEncoding.ASCII.GetString(bytearray);
        return true;
    }

      public bool UnlikePost(string postId)
      {
          StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
          Append(Constants.URI_ACTIVITIES).Append(Constants.URI_ME).Append("/").Append(DecodeFeedScope(FEED_SCOPE.LIKED)).Append("/").Append(postId);

          byte[] bytearray = Utils.Utils.GetFeedFromUrl(buf.ToString(), "DELETE", String.Empty, this);
          string s = System.Text.ASCIIEncoding.ASCII.GetString(bytearray);
          return true;
    }
    
    public IList<Post> MutedPosts(string userId) {
      return null;
    }
    
    public bool MutePost(string postId) {
      return false;
    }
    
    public bool UnmutePost(string postId) {
      return false;
  }

  public ProfileFeed Followers()
  {
      return Followers(Constants.URI_ME, FEED_SCOPE.GROUP);
  }

      public ProfileFeed Followers(string userId, FEED_SCOPE scope)
      {
          StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
          Append(Constants.URI_PEOPLE).Append(userId).Append("/").Append(DecodeFeedScope(scope)).Append(Constants.URI_FOLLOWERS).Append("?max-results=").Append(maxResults);
          if (!String.IsNullOrEmpty(continuationToken))
              buf.Append("&c=").Append(continuationToken);
          return parser.ParseProfile(buf.ToString(), String.Empty);
      }

      public ProfileFeed Following()
      {
          return Following(Constants.URI_ME, FEED_SCOPE.GROUP);
      }

      public ProfileFeed Following(string userId, FEED_SCOPE scope)
      {
          StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
          Append(Constants.URI_PEOPLE).Append(userId).Append("/").Append(DecodeFeedScope(scope)).Append(Constants.URI_FOLLOWING).Append("?max-results=").Append(maxResults);
          if (!String.IsNullOrEmpty(continuationToken))
              buf.Append("&c=").Append(continuationToken);
          return parser.ParseProfile(buf.ToString(), String.Empty);
      }
    
    public bool Follow(string userId) {
        StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
        Append(Constants.URI_PEOPLE).Append(Constants.URI_ME).Append("/").Append(DecodeFeedScope(FEED_SCOPE.GROUP)).Append(Constants.URI_FOLLOWING).Append("/").Append(userId);

        byte[] bytearray = Utils.Utils.GetFeedFromUrl(buf.ToString(), "PUT", String.Empty, this);
        string s = System.Text.ASCIIEncoding.ASCII.GetString(bytearray);
        return true;
    }

      public bool Unfollow(string userId)
      {
          StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
          Append(Constants.URI_PEOPLE).Append(Constants.URI_ME).Append("/").Append(DecodeFeedScope(FEED_SCOPE.GROUP)).Append(Constants.URI_FOLLOWING).Append("/").Append(userId);

          byte[] bytearray = Utils.Utils.GetFeedFromUrl(buf.ToString(), "DELETE", String.Empty, this);
          string s = System.Text.ASCIIEncoding.ASCII.GetString(bytearray);
          return true;
    }
      public Feed GetComments(string postId)
      {
          return GetComments(Constants.URI_ME, postId);
      }
      public Feed GetComments(string userId, string postId)
      {
        Feed postFeed = parser.Parse(new StringBuilder().Append(Constants.API_URI).
              Append(Constants.URI_ACTIVITIES).Append(userId).Append("/").Append(Constants.URI_SELF).Append("/").Append(postId).Append("/").Append(Constants.URI_COMMENTS).ToString(),String.Empty, Feed.FEED_TYPE.COMMENT);
        if (postFeed != null && postFeed.Entries != null && postFeed.Entries.Count > 0)
            return postFeed;
        else
            return null;
    }
    
    public Comment GetComment(string postId, string commentId) {
      return null;
    }
    
    public string AddComment(string postId, string comment) {

        string postData = Utils.Utils.constructComment(comment);
        Feed postFeed = parser.Parse(new StringBuilder().Append(Constants.API_URI).
              Append(Constants.URI_ACTIVITIES).Append(Constants.URI_ME).Append("/").Append(Constants.URI_SELF).Append("/").Append(postId).Append("/").Append(Constants.URI_COMMENTS).ToString(), postData);
        return comment ;
    }
    
    public bool UpdateComment(string postId, string commentId) {
      return false;
    }
    
    public bool DeleteComment(string postId, string commentId) {
      return false;
    }

      public ProfileFeed SearchPeople(string query)
      {
          StringBuilder buf = new StringBuilder().Append(Constants.API_URI).
          Append(Constants.URI_PEOPLE).Append(Constants.URI_SEARCH_FOR).Append(query).Append("&max-results=").Append(maxResults);
          if (!String.IsNullOrEmpty(continuationToken))
              buf.Append("&c=").Append(continuationToken);
          return parser.ParseProfile(buf.ToString(), String.Empty);
      }
    
    public Person GetPerson(string userId) {
      return null;
    }
    
    public IList<Person> SuggestedUsers() {
      return null;
    }
    
    //TODO: figure out the Group model
    public void GetGroups() {
    
    }
    
    //TODO: will take Group as second param
    public string CreateGroup(string userId) {
      return null;
    }
    
    public bool UpdateGroup(string userId, string groupId) {
      return false;
    }
    
    public bool DeleteGroup(string userId, string groupId) {
      return false;
    }
    
    public IList<Person> GetGroupMembers(string userId, string groupId) {
      return null;
    }
    
    public bool AddGroupMember(string userId, string groupId, string memberId) {
      return false;
    }
    
    public bool RemoveGroupMember(string userId, string groupId, string memberId) {
      return false;
    }
    
    public bool Block(string userId) {
      return false;
    }
    
    public bool Unblock(string userId) {
      return false;
    }
    
    public bool IsBlocke(string userId) {
      return false;
    }
    
    public bool ReportActivity(string postId) {
      return false;
    }
    
    public bool ReportUser(string userId) {
      return false;
    }
    
    private string DecodeFeedScope(FEED_SCOPE scope) {
      if (scope == FEED_SCOPE.CONSUMPTION)
        return Constants.URI_CONSUMPTION;
      else if (scope == FEED_SCOPE.SELF)
        return Constants.URI_SELF;
      else if (scope == FEED_SCOPE.LIKED)
        return Constants.URI_LIKED;
    else if (scope == FEED_SCOPE.GROUP)
        return Constants.URI_GROUPS;
      else
        return Constants.URI_PUBLIC;
    }
  }
}