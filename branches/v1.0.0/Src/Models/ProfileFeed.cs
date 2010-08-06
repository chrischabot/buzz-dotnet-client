using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Google.Api.Buzz.Models
{
    public class ProfileFeed
    {
        public int startIndex;
        public int itemsPerPage;
        public int totalResults;
        public IList<BuzzUserProfile> entries;
    }
}
