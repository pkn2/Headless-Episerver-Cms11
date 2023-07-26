using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AddFindIndexcmsFram.Models.Pages
{
    [ContentType(DisplayName = "TestPage", GUID = "9C0F4D72-4093-47BC-B912-DFCD99922213", Description = "")]
    public class TestPageType : PageData
    {
        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }


        [CultureSpecific]
        [Display(
            Name = "Main",
            Description = "The main body will be shown in the main content area of the page.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Main { get; set; }
    }
}