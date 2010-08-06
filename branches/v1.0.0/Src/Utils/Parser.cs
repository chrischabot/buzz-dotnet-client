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

using Google.Api.Buzz.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Google.Api.Buzz.Utils {
  public class Parser {
    
    private bool isDebug = false;
      private Buzz buzz = null;
    
    /// <summary>
    /// The entry point method for the parser with activity feeds
    /// </summary>
    /// <param name="url">
    /// A <see cref="System.String"/> that represents the feed url.
    /// </param>
    /// <returns>
    /// A <see cref="Feed"/>
    /// </returns>
    public Feed Parse(string url) {
      Console.WriteLine(url);
      return Parse(url, String.Empty, Feed.FEED_TYPE.ACTIVITY);
  }
      public Feed Parse(string url, string postData)
      {
          Console.WriteLine(url);
          return Parse(url, postData, Feed.FEED_TYPE.ACTIVITY);
      }

      public void setBuzz(Buzz buzz)
      {
          this.buzz = buzz;
      }
      private Feed Parse(string url, Feed.FEED_TYPE type)
      {
          return Parse(url, String.Empty, type);
      }
      public ProfileFeed ParseProfile(string url, string postData)
      {
          Console.WriteLine(url);
          XmlDocument feedXml = Utils.GetFeedXML(url, postData, buzz);
          return ParseUserProfile(feedXml);
      }

      public Feed Parse(string url, string postData, Feed.FEED_TYPE type)
      {
          XmlDocument feedXml = Utils.GetFeedXML(url, postData, buzz);
          return Parse(feedXml,type);
      }
      public ProfileFeed ParseUserProfile(XmlDocument feedXml)
      {
          XmlNamespaceManager nsMgr = GetNamespaceTable(feedXml);
          ProfileFeed profileFeed = new ProfileFeed();
          profileFeed.entries = new List<BuzzUserProfile>();
          XmlElement docElement = feedXml.DocumentElement;

          profileFeed.startIndex = GetNodeValueInt(docElement, Constants.POCO, Constants.STARTINDEX, nsMgr);
          profileFeed.totalResults = GetNodeValueInt(docElement, Constants.POCO, Constants.TOTALRESULTS, nsMgr);
          profileFeed.itemsPerPage = GetNodeValueInt(docElement, Constants.POCO, Constants.ITEMSPERPAGE, nsMgr);

          XmlNodeList entryNodes = GetNodes(docElement, Constants.POCO, Constants.ENTRY, nsMgr);
          foreach (XmlElement entryNode in entryNodes)
          {
              profileFeed.entries.Add(GetUserProfile(entryNode, nsMgr));
          }
          return profileFeed;
      }

      public Feed Parse(XmlDocument feedXml, Feed.FEED_TYPE type)
      {
          XmlNamespaceManager nsMgr = GetNamespaceTable(feedXml);

          Feed feed = new Feed(type);
          XmlElement docElement = feedXml.DocumentElement;

          feed.Title = GetNodeValue(docElement, Constants.ATOM, Constants.TITLE, nsMgr);
          feed.Id = GetNodeValue(docElement, Constants.ATOM, Constants.ID, nsMgr);
          feed.Generator = GetNodeValue(docElement, Constants.ATOM, Constants.GENERATOR, nsMgr);

          XmlNodeList entryNodes = GetNodes(docElement, Constants.ATOM, Constants.ENTRY, nsMgr);

          if (type == Feed.FEED_TYPE.ACTIVITY)
          {
              foreach (XmlElement entryNode in entryNodes)
              {
                  feed.AddEntry(GetPost(entryNode, nsMgr));
              }
              if (entryNodes.Count == 0)
              {
                  Post post = GetPost(docElement, nsMgr);
                  if ( post.Author != null && !String.IsNullOrEmpty(post.Author.Name))
                    feed.AddEntry(post);
              }

              XmlNodeList feedLinks = GetNodes(docElement, Constants.ATOM, Constants.LINK, nsMgr);
              foreach (XmlElement feedLink in feedLinks)
              {
                  feed.AddLink(GetLink(feedLink));
              }

              feed.Updated = GetNodeValue(docElement, Constants.ATOM, Constants.UPDATED, nsMgr);
          }
          else if (type == Feed.FEED_TYPE.COMMENT)
          {
              feed.AddLink(GetLink((XmlElement)GetNode(docElement, Constants.ATOM, Constants.LINK, nsMgr)));
              foreach (XmlElement commentNode in entryNodes)
              {
                  feed.AddEntry(GetComment(commentNode, nsMgr));
              }
          }

          return feed;
      }
    
    private Post GetPost(XmlElement entryNode, XmlNamespaceManager nsMgr) {
      Post post = new Post();
      
      post.Title = GetNodeValue(entryNode, Constants.ATOM, Constants.TITLE, nsMgr);
      post.Published = GetNodeValue(entryNode, Constants.ATOM, Constants.PUBLISHED, nsMgr);
      post.Updated = GetNodeValue(entryNode, Constants.ATOM, Constants.UPDATED, nsMgr);
      post.Id = GetNodeValue(entryNode, Constants.ATOM, Constants.ID, nsMgr);
      post.Content = GetNodeValue(entryNode, Constants.ATOM, Constants.CONTENT, nsMgr);
      post.ActivityVerb = GetNodeValue(entryNode, Constants.ACTIVITY, Constants.VERB, nsMgr);
      
      XmlNodeList postLinks = GetNodes(entryNode, Constants.ATOM, Constants.LINK, nsMgr);
      foreach(XmlElement postLink in postLinks) {
        
        Link link = GetLink(postLink);
        Link sourcePost = null;
        if (Constants.ALTERNATE_REL.Equals(link.Rel)) {
          //TODO: temporary, remove once like is beautified
          sourcePost = link;
        }
        if (Constants.REPLIES_REL.Equals(link.Rel)) {
          //XmlDocument repliesXml = GetFeedXML(link.Href);
          //post.CommentsFeed = Parse("/Users/prashantt/Desktop/comments.xml", Feed.FEED_TYPE.COMMENT);
          post.CommentsFeed = Parse(link.Href, Feed.FEED_TYPE.COMMENT);
        } else if (Constants.LIKERS_REL.Equals(link.Rel) && link.BuzzCount > 0) {
          //TODO: do this right once oauth is done
          for (int i = 0; i < link.BuzzCount; i++) {
            Like like = new Like();
            like.SourcePost = sourcePost;
            like.Liker = new Person();
            like.Id = "";
            post.AddLike(like);
          }
        }
        post.AddLink(GetLink(postLink));
      }
      
      post.Author = GetAuthor((XmlElement) 
        GetNodeObject(entryNode, Constants.ATOM, Constants.AUTHOR, nsMgr), nsMgr);
      post.ActivityObject = GetObject((XmlElement) 
        GetNodeObject(entryNode, Constants.ACTIVITY, Constants.OBJECT, nsMgr), nsMgr);
      post.CrosspostSource = GetCrosspostSource((XmlElement)
        GetNodeObject(entryNode, Constants.CROSSPOST, Constants.SOURCE, nsMgr), nsMgr);
      post.Source = GetSource((XmlElement)
        GetNodeObject(entryNode, Constants.ATOM, Constants.SOURCE, nsMgr), nsMgr);
      post.BuzzVisibility = GetBuzzVisibility((XmlElement)
        GetNodeObject(entryNode, Constants.BUZZ, Constants.VISIBILITY, nsMgr), nsMgr);
      
      return post;
    }
    
    private Comment GetComment(XmlElement commentNode, XmlNamespaceManager nsMgr) {
      Comment comment = new Comment();
      
      comment.ActivityObject = new ActivityObject();
      comment.ActivityObject.Type = GetNodeValue(commentNode, Constants.ACTIVITY, Constants.OBJECT_TYPE, nsMgr);
      comment.Published =  GetNodeValue(commentNode, Constants.ATOM, Constants.PUBLISHED, nsMgr);
      comment.Id = GetNodeValue(commentNode, Constants.ATOM, Constants.ID, nsMgr);
      comment.Author = GetAuthor((XmlElement) GetNodeObject(commentNode, Constants.ATOM, Constants.AUTHOR, nsMgr), nsMgr);
      comment.Content = GetNodeValue(commentNode, Constants.ATOM, Constants.CONTENT, nsMgr);
      XmlNodeList commentLinks = GetNodes(commentNode, Constants.ATOM, Constants.LINK, nsMgr);
      foreach(XmlElement commentLink in commentLinks) {
        comment.AddLink(GetLink(commentLink));
      }
      comment.SourcePost = GetLink((XmlElement) GetNodeObject(commentNode, Constants.THREAD, Constants.IN_REPLY_TO, nsMgr));
      return comment;
    }
    
    private Link GetLink(XmlElement linkNode) {
      Link link = new Link();
      
      link.Href = linkNode.GetAttribute(Constants.HREF);
      link.Rel = linkNode.GetAttribute(Constants.REL);
      link.Type = linkNode.GetAttribute(Constants.TYPE);
      link.MediaHeight = linkNode.GetAttribute(Constants.MEDIA_HEIGHT);
      link.MediaWidth = linkNode.GetAttribute(Constants.MEDIA_WIDTH);
      if (Utils.IsNotEmptyString(linkNode.GetAttribute(Constants.BUZZ_COUNT))) {
        link.BuzzCount = Convert.ToInt32(linkNode.GetAttribute(Constants.BUZZ_COUNT));
      }
      if(Utils.IsNotEmptyString(linkNode.GetAttribute(Constants.THREAD_COUNT))) {
        link.ThreadCount = Convert.ToInt32(linkNode.GetAttribute(Constants.THREAD_COUNT));
      }
      link.ThreadUpdated = linkNode.GetAttribute(Constants.THREAD_UPDATED);
      
      return link;
  }

      private BuzzUserProfile GetUserProfile(XmlElement authorNode, XmlNamespaceManager nsMgr)
      {
          BuzzUserProfile author = new BuzzUserProfile();

          author.Id = GetNodeValue(authorNode, Constants.POCO, Constants.ID, nsMgr);
          author.Name = GetNodeValue(authorNode, Constants.POCO, Constants.DISPLAYNAME, nsMgr);
          author.ProfileUrl = GetNodeValue(authorNode, Constants.POCO, Constants.PROFILE_URL, nsMgr);
          author.PhotoUrl = GetNodeValue(authorNode, Constants.POCO, Constants.THUMBNAIL_URL, nsMgr);
          XmlNodeList urls = GetNodes(authorNode, Constants.POCO, Constants.URLS, nsMgr);
          author.urls = new List<Url>();
          foreach (XmlElement urlNode in urls)
          {
              author.urls.Add(GetURL(urlNode, nsMgr));
          }
          XmlNodeList photos = GetNodes(authorNode, Constants.POCO, Constants.PHOTOS, nsMgr);
          author.photos = new List<Photo>();
          foreach (XmlElement photoNode in photos)
          {
              author.photos.Add(GetPhoto(photoNode, nsMgr));
          }
          XmlNodeList organizations = GetNodes(authorNode, Constants.POCO, Constants.ORGNIZATIONS, nsMgr);
          author.organizations = new List<Organization>();
          foreach (XmlElement org in organizations)
          {
              author.organizations.Add(GetOrganization(org, nsMgr));
          }
          return author;
      }
    
    private Person GetAuthor(XmlElement authorNode, XmlNamespaceManager nsMgr) {
      Person author = new Person();
      if (authorNode == null)
          return null;
      
      author.Id = GetNodeValue(authorNode, Constants.POCO, Constants.ID, nsMgr);
      author.Name = GetNodeValue(authorNode, Constants.ATOM, Constants.NAME, nsMgr);
      author.ProfileUrl = GetNodeValue(authorNode, Constants.ATOM, Constants.URI, nsMgr);
      author.PhotoUrl = GetNodeValue(authorNode, Constants.POCO, Constants.PHOTO_URL, nsMgr);
      author.ActivityObjectType = GetNodeValue(authorNode, Constants.ACTIVITY, Constants.OBJECT_TYPE, nsMgr);
      
      return author;
  }

  private Url GetURL(XmlElement urlNode, XmlNamespaceManager nsMgr)
  {
      Url url = new Url();
      if (urlNode == null)
          return url;

      url.type = GetNodeValue(urlNode, Constants.POCO, Constants.TYPE, nsMgr);
      url.url = GetNodeValue(urlNode, Constants.POCO, Constants.VALUE, nsMgr);

      return url;
  }
      private Organization GetOrganization(XmlElement urlNode, XmlNamespaceManager nsMgr)
  {
      Organization org = new Organization();
      if (urlNode == null)
          return org;

      org.type = GetNodeValue(urlNode, Constants.POCO, Constants.TYPE, nsMgr);
      org.name = GetNodeValue(urlNode, Constants.POCO, Constants.NAME, nsMgr);

      return org;
  }
  private Photo GetPhoto(XmlElement photoNode, XmlNamespaceManager nsMgr)
  {
      Photo photo = new Photo();
      if (photoNode == null)
          return photo;

      photo.type = GetNodeValue(photoNode, Constants.POCO, Constants.TYPE, nsMgr);
      photo.url = GetNodeValue(photoNode, Constants.POCO, Constants.VALUE, nsMgr);
      photo.width = GetNodeValueInt(photoNode, Constants.POCO, Constants.WIDTH, nsMgr);
      photo.height = GetNodeValueInt(photoNode, Constants.POCO, Constants.HEIGHT, nsMgr);

      return photo;
  }
    
    private ActivityObject GetObject(XmlElement objectNode, XmlNamespaceManager nsMgr) {
      ActivityObject activityObject = new ActivityObject();
      if (objectNode == null)
          return activityObject;
      
      activityObject.Content = GetNodeValue(objectNode, Constants.ATOM, Constants.CONTENT, nsMgr);
      activityObject.Type = GetNodeValue(objectNode, Constants.ACTIVITY, Constants.OBJECT_TYPE, nsMgr);
      activityObject.Link = GetLink((XmlElement) GetNodeObject(objectNode, Constants.ATOM, Constants.LINK, nsMgr));
      
      XmlNodeList attachmentNodes = GetNodes(objectNode, Constants.BUZZ, Constants.ATTACHMENT, nsMgr);
      foreach(XmlElement attachment in attachmentNodes) {
        activityObject.AddAttachment(GetAttachment(attachment, nsMgr));
      }
      
      return activityObject;
    }
    
    private CrosspostSource GetCrosspostSource(XmlElement crosspostSrcNode, XmlNamespaceManager nsMgr) {
      CrosspostSource crosspostSource = new CrosspostSource();
      if (crosspostSrcNode == null)
          return crosspostSource;
      
      crosspostSource.Id = GetNodeValue(crosspostSrcNode, Constants.ATOM, Constants.ID, nsMgr);
      
      return crosspostSource;
    }
    
    private Source GetSource(XmlElement srcNode, XmlNamespaceManager nsMgr) {
      Source source = new Source();
      if (srcNode == null)
          return source;
      
      source.ActivityService = GetActivityService((XmlElement) 
        GetNodeObject(srcNode, Constants.ACTIVITY, Constants.SERVICE, nsMgr), nsMgr);
        
      return source;
    }
    
    private BuzzVisibility GetBuzzVisibility(XmlElement buzzVisibilityNode, XmlNamespaceManager nsMgr) {
      BuzzVisibility buzzVisibility = new BuzzVisibility();
      if (buzzVisibilityNode == null)
          return buzzVisibility;
      
      buzzVisibility.BuzzAclEntry = GetBuzzAclEntry((XmlElement)
        GetNodeObject(buzzVisibilityNode, Constants.BUZZ, Constants.ACLENTRY, nsMgr), nsMgr);
        
      return buzzVisibility;
    }
    
    private Attachment GetAttachment(XmlElement attachmentNode, XmlNamespaceManager nsMgr) {
      Attachment attachment = new Attachment();
      if (attachmentNode == null)
          return attachment;
      
      attachment.Content = GetNodeValue(attachmentNode, Constants.ATOM, Constants.CONTENT, nsMgr);
      attachment.Title = GetNodeValue(attachmentNode, Constants.ATOM, Constants.TITLE, nsMgr);
      attachment.Type = GetNodeValue(attachmentNode, Constants.ACTIVITY, Constants.OBJECT_TYPE, nsMgr);
      
      XmlNodeList attachmentLinks = GetNodes(attachmentNode, Constants.ATOM, Constants.LINK, nsMgr);
      foreach(XmlElement attachmentLink in attachmentLinks) {
        attachment.AddLink(GetLink(attachmentLink));
      }
      
      return attachment;
    }
    
    private ActivityService GetActivityService(XmlElement activitySvcNode, XmlNamespaceManager nsMgr) {
      ActivityService activityService = new ActivityService();
      if (activitySvcNode == null)
          return activityService;
      
      activityService.Title = GetNodeValue(activitySvcNode, Constants.ATOM, Constants.TITLE, nsMgr);
      
      return activityService;
    }
    
    private BuzzAclEntry GetBuzzAclEntry(XmlElement aclEntryNode, XmlNamespaceManager nsMgr) {
      BuzzAclEntry buzzAclEntry = new BuzzAclEntry();
      if (aclEntryNode == null)
          return buzzAclEntry;
      
      buzzAclEntry.Id = GetNodeValue(aclEntryNode, Constants.POCO, Constants.ID, nsMgr);
      buzzAclEntry.Uri = GetNodeValue(aclEntryNode, Constants.ATOM, Constants.URI, nsMgr);
      buzzAclEntry.Name = GetNodeValue(aclEntryNode, Constants.POCO, Constants.NAME, nsMgr);
      
      return buzzAclEntry;
    }
    
    private static XmlNamespaceManager GetNamespaceTable(XmlDocument xDoc) {
      XmlNamespaceManager nsMgr = new XmlNamespaceManager(xDoc.NameTable);
      
      nsMgr.AddNamespace(Constants.ATOM, Constants.XMLNS_ATOM);
      nsMgr.AddNamespace(Constants.THREAD, Constants.XMLNS_THREAD);
      nsMgr.AddNamespace(Constants.MEDIA, Constants.XMLNS_MEDIA);
      nsMgr.AddNamespace(Constants.ACTIVITY, Constants.XMLNS_ACTIVITY);
      nsMgr.AddNamespace(Constants.GEORSS, Constants.XMLNS_GEORSS);
      nsMgr.AddNamespace(Constants.POCO, Constants.XMLNS_POCO);
      nsMgr.AddNamespace(Constants.GD, Constants.XMLNS_GD);
      nsMgr.AddNamespace(Constants.CROSSPOST, Constants.XMLNS_CROSSPOST);
      nsMgr.AddNamespace(Constants.BUZZ, Constants.XMLNS_BUZZ);
      
      return nsMgr;
    }
    
    private XmlNode GetNode(XmlElement element, string xmlns, string nodeName, XmlNamespaceManager nsMgr) {
      string path = new StringBuilder().Append(xmlns).Append(":").
          Append(nodeName).ToString();
      try {
      	return element.SelectSingleNode(path, nsMgr);
      } catch (Exception ex) {
        if (isDebug) {
      	  Console.WriteLine("an error occurred while getting the node at " +
	  	    path + " for " + element.Name+ ".");
      	  }
      	return null;
      }
    }
    
    private XmlNodeList GetNodes(XmlElement element, string xmlns, string nodeName, XmlNamespaceManager nsMgr) {
      string path = new StringBuilder().Append(xmlns).Append(":").
          Append(nodeName).ToString();
      try {
      	return element.SelectNodes(path, nsMgr);
      } catch (Exception ex) {
        if (isDebug) {
      	  Console.WriteLine("an error occurred while getting the xml nodes at " +
		    path + " for " + element.Name + ".");
      	}
      	return null;
      }
    }
    
    private string GetNodeValue(XmlElement element, string xmlns, string nodeName, XmlNamespaceManager nsMgr) {
      string path = new StringBuilder().Append(xmlns).Append(":").
          Append(nodeName).Append("/text()").ToString();
      try {
      	return element.SelectSingleNode(path, nsMgr).Value;
      } catch (Exception ex) {
        if (isDebug) {
      	  Console.WriteLine("an error occurred while getting the node value at " +
	  	    path + " for " + element.Name+ ".");
      	  }
      	return null;
      }
  }
  private Int32 GetNodeValueInt(XmlElement element, string xmlns, string nodeName, XmlNamespaceManager nsMgr)
  {
      string path = new StringBuilder().Append(xmlns).Append(":").
          Append(nodeName).Append("/text()").ToString();
      try
      {
           return Int32.Parse( element.SelectSingleNode(path, nsMgr).Value);
      }
      catch (Exception ex)
      {
          if (isDebug)
          {
              Console.WriteLine("an error occurred while getting the node value at " +
                path + " for " + element.Name + ".");
          }
          return 0;
      }
  }
    
    private XmlNode GetNodeObject(XmlElement element, string xmlns, string nodeName, XmlNamespaceManager nsMgr) {
      string path = new StringBuilder().Append(xmlns).Append(":").
          Append(nodeName).ToString();
      try {
        return element.SelectSingleNode(path, nsMgr);
      } catch (Exception ex) {
        if (isDebug) {
      	  Console.WriteLine("an error occurred while getting the node object at " +
	  	    path + " for " + element.Name + ".");
      	}
      	return null;
      }
    }

  }
}
