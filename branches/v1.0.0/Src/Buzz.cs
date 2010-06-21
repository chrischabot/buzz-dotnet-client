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

namespace Google.Api.Buzz {

  public class Buzz {
    Parser parser = new Parser();
    
    public enum FEED_SCOPE { SELF, PUBLIC, CONSUMPTION, LIKED };
    
    public IList<Entry> Search(string query) {
      return Search(query, null);
    }
    
    public IList<Entry> Search(string query, string geo) {
      //fix geo
      return (IList<Entry>) parser.Parse(new StringBuilder().Append(Constants.API_URI).
        Append(Constants.URI_ACTIVITIES).Append(Constants.URI_SEARCH_FOR).Append(query).ToString()).Entries;
    }
    
    public IList<Entry> GetPosts() {
      return GetPosts(Constants.URI_ME, FEED_SCOPE.SELF);
    }
    
    public IList<Entry> GetPosts(FEED_SCOPE scope) {
      return GetPosts(Constants.URI_ME, scope);
    }
    
    public IList<Entry> GetPosts(string userId, FEED_SCOPE scope) {
      if (userId == null) {
        //throw exception
      }
      //check for exception in response
      return (IList<Entry>) parser.Parse(new StringBuilder().Append(Constants.API_URI).
        Append(Constants.URI_ACTIVITIES).Append(userId).Append("/").Append(DecodeFeedScope(scope)).Append("?maxResults=100").ToString()).Entries;
    }
    
    public Post GetPost(string postId) {
//      if (!Utils.IsNotEmptyString(postId)) {
//        //exception
//      }
      Feed postFeed = parser.Parse(new StringBuilder().Append(Constants.API_URI).
        Append(Constants.URI_ACTIVITIES).Append(Constants.URI_ME).Append(Constants.URI_SELF).Append(postId).ToString());
      if (postFeed != null)
        return (Post) postFeed.Entries[0];
      return null;
    }
    
    public bool UpdatePost(string postId) {
      return false;
    }
    
    public bool DeletePost(string userId) {
      return false;
    }
    
    public IList<Like> GetLikes(string userId, string postId) {
      return null;
    }
    
    public IList<Post> GetLikedPosts(string userId) {
      return null;
    }
    
    public bool LikePost(string postId) {
      return false;
    }
    
    public bool UnlikePost(string postId) {
      return false;
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
    
    public IList<Person> Followers(string userId) {
      return null;
    }
    
    public IList<Person> Following(string userId) {
      return null;
    }
    
    public bool Follow(string userId) {
      return false;
    }
    
    public bool Unfollow(string userId) {
      return false;
    }
    
    public IList<Comment> GetComments(string userId, string postId) {
      return null;
    }
    
    public Comment GetComment(string postId, string commentId) {
      return null;
    }
    
    public string AddComment(string postId, Comment comment) {
      return null;
    }
    
    public bool UpdateComment(string postId, string commentId) {
      return false;
    }
    
    public bool DeleteComment(string postId, string commentId) {
      return false;
    }
    
    public IList<Person> SearchPeople(string query) {
      return null;
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
      else
        return Constants.URI_PUBLIC;
    }
  }
}