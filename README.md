# ThisNetWorks.OrchardCore.Styles
Provides a dynamic style Content Field for Orchard Core


## Features

- Style field
- Style schemas.
- Autoprefixer support

## Getting Started

- Install the [ThisNetWorks.OrchardCore.Styles](https://www.nuget.org/packages/ThisNetWorks.OrchardCore.Styles) module to your Orchard Core Host project.
- Enable the Styles feature.
- Create a style schema.
- Add the Style field to a content type definition.
- Edit the settings for the field and either select a predefined style schema or enter a custom schema.
- Render the content item.

By default the style field will insert a `<style>` tag with the styles in the `<head>` of any content item to which it is attached.

You can customize this rendering to display via custom settings, or use the styles inline on a content item.

## Samples

There is a sample project which includes custom configurations for the CKEditor toolbar.

To install run the `Styles sample` recipe.

Example schemas are found in the _Design -> Style schemas_ menu.

An example style guide will be automatically created as the home content item.

## Style schemas

A style schema defines how the styles field will be edited, and which components will be available.

There are two types of schemas.

- Component schema
- Contained schema

### Component schema

At it's most basic a schema entry must contain a reference to a Vuejs component, and the description of how to render it's entry as CSS.

```
{
  "H2Color": {
    "component": "Color",
    "displayText": "H2 color",
    "hint": "Pick a color for the H2 element",
    "defaultValue": {
      "color": {
        "hex": "#8bc34a",
        "rgb": "rgba(139, 195, 74, 1)"
      }
    },
    "renderer": {
      "name": "LiquidJs",
      "template": "h2 { color: {{ value.hex }}; }"
    }
  }
}
```

This defines an entry for the name of the field, a VueJs component name that will render the schema, `displayText` and `hint`.

Valid `component` names are
- `SizeInput`
- `Color`
- `TextAny`

A `Media` component is in progress.

You can create your own components, as a VueJs component on the windows VueJs instance, they must be rendered to the admin view before the styles app script is called.


The `defaultValue` is the default value to be applied when creating a field, based on the component type

The `renderer` can be either a renderer name, e.g.
```
"renderer": "CssSizeEntry"
```

or a `LiquidJS` template for more complex schemas.
```
"renderer": {
  "name": "LiquidJs",
  "template": "h2 { color: {{ value.hex }}; }"
}
```    

### Contained schema

A contained schema has similar properties, but also nests a `schema` entry, which can contain either more contained schemas, or many component schemas.

```
{
  "CssVars": {
    "displayText": "Css Vars",
    "hint": "A range of css vars for the site",
    "renderer": "RootVars",
    "example": "",
    "schema": {
      "ButtonTextColor": {
        "component": "Color",
        "displayText": "Button text color",
        "hint": "Pick a color for the button text",
        "defaultValue": {
          "color": {
            "hex": "#095b9eff",
            "rgb": "rgba(9, 91, 158, 1)"
          },
          "compiled": ""
        },
        "renderer": "CssVarColor"
      },
      "ButtonBackgroundColor": {
        "component": "Color",
        "displayText": "Button background color",
        "hint": "Pick a color for the button background",
        "defaultValue": {
          "color": {
            "hex": "#4c839e",
            "rgb": "rgb(76 131 158)"
          },
          "compiled": ""
        },
        "renderer": "CssVarColor"
      },
      "BorderRadius": {
        "component": "SizeSingle",
        "displayText": "Border radius",
        "hint": "Enter a border radius (rem, em, px)",
        "defaultValue": {
          "size": {
            "size": 0.5,
            "unit": "rem"
          },
          "compiled": ""
        },
        "props": {
          "units": [
            {
              "displayText": "Rem",
              "value": "rem"
            },
            {
              "displayText": "Pixels",
              "value": "px"
            }
          ],
          "mergeUnits": true,
          "step": 0.25
        },
        "renderer": "CssVarSizeEntry"
      },
      "Border": {
        "component": "TextAny",
        "displayText": "Border",
        "hint": "Enter a border definition (1px solid transparent)",
        "defaultValue": {
          "text": "1px solid black",
          "compiled": ""
        },
        "renderer": "CssVarEntry"
      }
    }
  }
}
```

It is best to refer to the samples project to understand schemas in more detail.

## Autoprefixer

Autoprefix is automatically applied to all compiled styles.

## Style Field Properties

The `StyleField` contains two properties

`CompiledStyles` the complete compiled style.
`StyleRecord` a `JObject` containing the record that makes up this style. Each style inside the `JObject` also has a `compiled` property which can be used to retrieve individual items.

## Versions

Version tags and pre release suffixes are based of the version of Orchard Core referenced.

For version 1.0 of Orchard Core use `1.0.0`, which will use the [Orchard Core NuGet Feed](https://www.nuget.org/packages/OrchardCore/).

Prerelease versions are suffixed with the CloudSmith build of Orchard Core referenced, 
and will required a configured CloudSmith NuGet feed. Refer [Configuring a preview package source](https://docs.orchardcore.net/en/latest/docs/getting-started/preview-package-source/)

e.g. `1.2.0-preview-16541` refers to the CloudSmith Orchard Core prerelease build `v1.2.0-preview-16541`

## License

[ThisNetworks.OrchardCore.Tyles](https://github.com/ThisNetWorks/ThisNetWorks.OrchardCore.STyles/blob/master/LICENSE) licensed under the terms of the MIT License.