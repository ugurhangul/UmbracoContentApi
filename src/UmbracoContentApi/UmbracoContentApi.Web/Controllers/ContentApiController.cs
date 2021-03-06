﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.WebApi;
using UmbracoContentApi.Core.Models;
using UmbracoContentApi.Core.Resolvers;

namespace UmbracoContentApi.Web.Controllers
{
    [RoutePrefix("api/content")]
    public class ContentApiController : UmbracoApiController
    {
        private readonly Lazy<IContentResolver> _contentResolver;

        public ContentApiController(
            Lazy<IContentResolver> contentResolver)
        {
            _contentResolver = contentResolver;
        }

        [Route("{id:guid}")]
        [ResponseType(typeof(ContentModel))]
        public IHttpActionResult Get(Guid id, int level = 0)
        {
            IPublishedContent content = Umbraco.Content(id);
            if (level <= 0)
            {
                return Ok(_contentResolver.Value.ResolveContent(content));
            }

            var dictionary = new Dictionary<string, object>
            {
                { "level", level }
            };

            return Ok(_contentResolver.Value.ResolveContent(content, dictionary));
        }
    }
}