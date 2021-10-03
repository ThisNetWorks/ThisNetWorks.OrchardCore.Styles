using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "ThisNetWorks OrchardCore Styles",
    Author = "ThisNetWorks",
    Website = "https://github.com/ThisNetWorks/ThisNetWorks.OrchardCore.Styles",
    Version = "1.1.0",
    Description = "Style field",
    Category = "Content Management",
    Dependencies = new []
    {
        "OrchardCore.ContentTypes",
        "OrchardCore.Shortcodes"
    }
)]
