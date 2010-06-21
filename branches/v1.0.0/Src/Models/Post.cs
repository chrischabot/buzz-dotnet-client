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

  public class Post : Entry {
    private string title;
    private string updated;
    private string geoPoint;
    private CrosspostSource crosspostSource;
    private Source source;
    private BuzzVisibility buzzVisibility;
    private string activityVerb;
    private Feed commentsFeed;
    private IList<Like> likes;
    
    public Post () {
    
    }

    public Post(string id, string title, string published, string updated, Person author,
      IList<Link> links, string geoPoint, ActivityObject activityObject, 
      CrosspostSource crosspostSource, Source source, BuzzVisibility buzzVisibility, 
      string activityVerb, string content, Feed commentsFeed, IList<Like> likes)
        : base(published, id, author, content, activityObject, links) {
      this.title = title;
      this.updated = updated;
      this.geoPoint = geoPoint;
      this.crosspostSource = crosspostSource;
      this.source = source;
      this.buzzVisibility = buzzVisibility;
      this.activityVerb = activityVerb;
      this.commentsFeed = commentsFeed;
      this.likes = likes;
    }
    
    public string Title {
      get { return title; }
      set { title = value; }
    }
    
    public string Updated {
      get { return updated; }
      set { updated = value; }
    }
    
    public string GeoPoint {
      get { return geoPoint; }
      set { geoPoint = value; }
    }
    
    public CrosspostSource CrosspostSource {
      get { return crosspostSource; }
      set { crosspostSource = value; }
    }
    
    public Source Source {
      get { return source; }
      set { source = value; }
    }
    
    public BuzzVisibility BuzzVisibility {
      get { return buzzVisibility; }
      set { buzzVisibility = value; }
    }
    
    public string ActivityVerb {
      get { return activityVerb; }
      set { activityVerb = value; }
    }
    
    public Feed CommentsFeed {
      get { return commentsFeed; }
      set { commentsFeed = value; }
    }
    
    public void AddComment(Comment comment) {
      if (commentsFeed == null) {
        commentsFeed = new Feed(Feed.FEED_TYPE.COMMENT);
      }
      commentsFeed.AddEntry(comment);
    }
    
    public bool HasComments() {
      return (commentsFeed != null && commentsFeed.Entries != null && commentsFeed.Entries.Count > 0);
    }
    
    public IList<Like> Likes {
      get { return likes; }
      set { likes = value; }
    }
    
    public void AddLike(Like like) {
      if (likes == null) {
        likes = new List<Like>();
      }
      //TODO: check if this liker hasn't liked already
      likes.Add(like);
    }
    
    public bool HasLikes() {
      return (likes != null && likes.Count > 0);
    }
  }
}
