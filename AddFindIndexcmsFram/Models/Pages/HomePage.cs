using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using System;
using System.ComponentModel.DataAnnotations;

namespace AddFindIndexcmsFram.Models.Pages
{
    [ContentType(DisplayName = "HomePage", GUID = "2e4cc62a-828b-4f45-bdde-8ab52a4f6839", Description = "")]
    public class HomePage : PageData
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