using System;
using System.Collections.Generic;
using System.Text;

namespace Google.Api.Buzz.Models
{
    public class BuzzUserProfile : Person
    {
        public IList<Url> urls;
        public IList<Photo> photos;
        public string aboutMe;
        public IList<Organization> organizations;
        public string interests;
        public string emails;

    }
}
